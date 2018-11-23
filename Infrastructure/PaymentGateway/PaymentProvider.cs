using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CapitalManagment;
using Microsoft.Extensions.Options;
using DonationManagment;
using DonationManagment.Infrastructure;
using Journalist.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway.Models;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp.Serializers;
using UserManagment;

namespace PaymentGateway
{
    public class PaymentProvider : IPaymentProvider
    {
        private readonly PaymentGatewaySettings _paymentGatewaySettings;
        private readonly ILogger<PaymentProvider> _logger;
        
        public PaymentProvider(IOptions<PaymentGatewaySettings> paymentOptions, ILogger<PaymentProvider> logger)
        {
            _logger = logger;
            _paymentGatewaySettings = paymentOptions.Value;
        }

        public DonationPaymentInformation InitiateSinglePaymentForDonation(Donation donation, CapitalCredentials credentials, User fromUser)
            => RegisterOrder(new PaymentRequestModel
            {
                Sector = credentials.MerchantLogin,
                Password = credentials.MerchantPassword,
                Reference = donation.Id,

                Amount = (int) Math.Ceiling(donation.Value * 100),
                Currency = (int) CurrencyCodes.Rub,
                Description = $"Однократное пожертвование в капитал",
                Email = fromUser.Email,
                FirstName = fromUser.FirstName,
                LastName = fromUser.LastName,
                Url = _paymentGatewaySettings.ReturnSuccessUrl.ToString()
            }, donation, credentials, _paymentGatewaySettings.RegisterOrderRoute);

        public DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation, CapitalCredentials credentials, User fromUser) 
            => RegisterOrder(new PaymentRequestModel
            {
                Sector = credentials.MerchantLogin,
                Password = credentials.MerchantPassword,
                Reference = donation.Id,
                
                Amount = (int)Math.Ceiling(donation.Value * 100), 
                Currency = (int)CurrencyCodes.Rub,
                Description = $"Регулярное пожертвование в капитал",
                Email = fromUser.Email,
                FirstName = fromUser.FirstName,
                LastName = fromUser.LastName,
                RecurringPeriod = DateTime.Now.Day.ToString(),
                Url = _paymentGatewaySettings.ReturnSuccessUrl.ToString()
            }, donation, credentials, _paymentGatewaySettings.RegisterOrderRoute);

        public void CancelRequrrentPayment(Donation donation, CapitalCredentials credentials)
            => RegisterOrder(new CancelRecurringPaymentModel
            {
                Sector = credentials.MerchantLogin,
                Id = donation.Id.ToString(),
                Password = credentials.MerchantPassword,
            }, donation, credentials, _paymentGatewaySettings.CancellationRoute);

        private DonationPaymentInformation RegisterOrder(object requestModel, Donation donation, CapitalCredentials credentials, Uri to)
        {
            var targetRoute = new Uri(_paymentGatewaySettings.BankApiUri, to);
            
            var bankResponse = RequestBankAsync(targetRoute, requestModel);

            return new DonationPaymentInformation(
                donation.Id, 
                CreatePaymentUri(bankResponse.Id, credentials.MerchantLogin, credentials.MerchantPassword),
                bankResponse.Id);
        }

        private PaymentResponse RequestBankAsync(Uri requestUri, object requestPayload)
        {
            var keyValuePayload = requestPayload.ToKeyValue();

            var client = new RestClient(requestUri) {Proxy = new WebProxy("http://127.0.0.1:8080")}; //For test uses;

            var request = new RestRequest("", Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            foreach (var bodyKeyValue in keyValuePayload)
            {
                request.AddParameter(PascalToSnake(bodyKeyValue.Key), bodyKeyValue.Value);
            }
            
            _logger.LogInformation($"Payment operation To {requestUri} with query data {null}");

            var httpResponse = client.Execute(request);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = $"Payment gateway error: {(int) httpResponse.StatusCode}";
                _logger.LogError(errorMessage);
                throw new PaymentGatewayException(errorMessage);
            }
            
            var response = new XmlDeserializer().Deserialize<PaymentResponse>(httpResponse);
            
            if (response.Code > 0)
            {
                var errorMessage = $"Payment failed with code '{response.Code}': {response.Description}";
                _logger.LogError(errorMessage);
                throw new PaymentGatewayException(errorMessage);
            }
            
            _logger.LogInformation($"Payment gateway responded {JsonConvert.SerializeObject(response)}");
            
            return response;
        }

        private string CreatePaymentUri(string bankOrderId, string sectorLogin, string sectorPass)
        {
            var signature = SignatureHelper.SignVia(sectorLogin, bankOrderId, sectorPass);
            var baseUrl = new Uri(_paymentGatewaySettings.BankApiUri, _paymentGatewaySettings.PaymentRoute);
            return baseUrl + $"?sector={sectorLogin}&id={bankOrderId}&signature={signature}";
        }

        private string PascalToSnake(string stringInPascalCase)
        {
            const string strRegex = @"((?<=.)[A-Z][a-zA-Z]*)|((?<=[a-zA-Z])\d+)";
            var transformRegex = new Regex(strRegex, RegexOptions.Multiline);

            const string strReplace = @"_$1$2";

            return transformRegex.Replace(stringInPascalCase, strReplace).ToLower();
        }
    }
}