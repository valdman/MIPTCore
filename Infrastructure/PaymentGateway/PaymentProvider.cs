using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using DonationManagment;
using DonationManagment.Infrastructure;
using Journalist.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway.Models;
using RestSharp;

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
        
        public DonationPaymentInformation InitiateSinglePaymentForDonation(Donation donation)
        {
            var targetRoute = new Uri(_paymentGatewaySettings.BankApiUri, _paymentGatewaySettings.SinglePaymentRouteUri);
            var payload = new PaymentRequestModel
            {
                OrderNumber = donation.Id,
                Amount = (int) Math.Round(donation.Value * 100),
                ReturnUrl = _paymentGatewaySettings.ReturnUrl.ToString()
            };

            var bankResponse = RequestBankAsync(targetRoute, payload);

            return new DonationPaymentInformation(donation.Id, bankResponse.FormUrl);
        }

        public DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation)
        {
            var targetRoute = new Uri(_paymentGatewaySettings.BankApiUri, _paymentGatewaySettings.RecurrentPaymentRouteUri);
            var requrrenParameters = new RequrrentPaymetParameters
            {
                RecurringExpiry = 21200101,
                RecurringFrequency = 28
            };
            
            var payload = new ParametrizedPaymentRequestModel()
            {
                OrderNumber = donation.Id,
                Amount = (int) Math.Round(donation.Value * 100),
                ReturnUrl = _paymentGatewaySettings.ReturnUrl.ToString(),
                JsonParams = JsonConvert.SerializeObject(requrrenParameters),
                ClientId = donation.UserId
            };

            var bankResponse = RequestBankAsync(targetRoute, payload);

            return new DonationPaymentInformation(donation.Id, bankResponse.FormUrl);
        }

        private PaymentResponse RequestBankAsync(Uri requestUri, object requestPayload)
        {
            var keyValuePayload = requestPayload.ToKeyValue();
            var authentificationPayload = new MerchantCredentials
            {
                UserName = _paymentGatewaySettings.MerchantLogin,
                Password = _paymentGatewaySettings.MerchantPassword
            }.ToKeyValue();

            var payload = authentificationPayload.Union(keyValuePayload);
            payload = payload.Select(pair => new KeyValuePair<string, string>(FirstCharacterToLower(pair.Key), pair.Value));

            var client = new RestClient(requestUri) {Proxy = new WebProxy("http://127.0.0.1:8080")}; //For test uses;

            var request = new RestRequest("", Method.GET);
            foreach (var querryParam in payload)
            {
                request.AddParameter(querryParam.Key, querryParam.Value);
            }
            
            _logger.LogInformation($"Payment operation to {requestUri} with query data {keyValuePayload.JoinStringsWith(",")}");

            var httpResponse = client.Execute(request);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = $"Payment gateway error: {(int) httpResponse.StatusCode}";
                _logger.LogError(errorMessage);
                throw new PaymentGatewayException(errorMessage);
            }
            
            var response = JsonConvert.DeserializeObject<PaymentResponse>(httpResponse.Content);
            
            _logger.LogInformation($"Payment gateway responded {JsonConvert.SerializeObject(response)}");
            
            return response;
        }
        
        private string FirstCharacterToLower(string str)
        {
            if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
                return str;

            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}