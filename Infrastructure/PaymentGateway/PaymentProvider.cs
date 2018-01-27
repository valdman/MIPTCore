using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CapitalManagment;
using Microsoft.Extensions.Options;
using DonationManagment;
using DonationManagment.Infrastructure;
using Journalist.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Models;
using RestSharp;
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
        {
            var targetRoute = new Uri(_paymentGatewaySettings.BankApiUri, _paymentGatewaySettings.SinglePaymentRouteUri);
            
            var userDataParameters = new PaymentJsonParams
            {
                Email = fromUser.Email,
                FullName = $"{fromUser.FirstName} {fromUser.LastName}"
            };
            
            var payload = new ParametrizedPaymentRequestModel()
            {
                OrderNumber = donation.Id,
                Amount = (int) Math.Round(donation.Value * 100),
                ReturnUrl = _paymentGatewaySettings.ReturnUrl.ToString(),
                JsonParams = SerializeWithoutNullToLowCamelcase(userDataParameters)
            };

            var bankResponse = RequestBankAsync(targetRoute, credentials, payload);

            return new DonationPaymentInformation(donation.Id, bankResponse.FormUrl, bankResponse.OrderId);
        }

        public DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation, CapitalCredentials credentials, User fromUser)
        {
            var targetRoute = new Uri(_paymentGatewaySettings.BankApiUri, _paymentGatewaySettings.RecurrentPaymentRouteUri);
            var requrrentParametersAndUserData = new PaymentJsonParams
            {
                RecurringExpiry = 21200101,
                RecurringFrequency = 28,
                Email = fromUser.Email,
                FullName = $"{fromUser.FirstName} {fromUser.LastName}"
            };
            
            var payload = new ParametrizedPaymentRequestModel()
            {
                OrderNumber = donation.Id,
                Amount = (int) Math.Round(donation.Value * 100),
                ReturnUrl = _paymentGatewaySettings.ReturnUrl.ToString(),
                JsonParams = SerializeWithoutNullToLowCamelcase(requrrentParametersAndUserData),
                ClientId = donation.UserId
            };

            var bankResponse = RequestBankAsync(targetRoute, credentials, payload);

            return new DonationPaymentInformation(donation.Id, bankResponse.FormUrl, bankResponse.OrderId);
        }

        private PaymentResponse RequestBankAsync(Uri requestUri, CapitalCredentials credentials, object requestPayload)
        {
            var keyValuePayload = requestPayload.ToKeyValue();
            var authentificationPayload = new MerchantCredentials
            {
                UserName = credentials.MerchantLogin,
                Password = credentials.MerchantPassword
            }.ToKeyValue();

            var payload = authentificationPayload.Union(keyValuePayload);
            payload = payload.Select(pair => new KeyValuePair<string, string>(FirstCharacterToLower(pair.Key), pair.Value));

            var client = new RestClient(requestUri); //{Proxy = new WebProxy("http://127.0.0.1:8080")}; //For test uses;

            var request = new RestRequest("", Method.GET);
            var payloadArray = payload as KeyValuePair<string, string>[] ?? payload.ToArray();
            
            foreach (var querryParam in payloadArray)
            {
                request.AddParameter(querryParam.Key, querryParam.Value);
            }
            
            _logger.LogInformation($"Payment operation To {requestUri} with query data {payloadArray.JoinStringsWith(",")}");

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

        private string SerializeWithoutNullToLowCamelcase(object @object)
        {
            return JsonConvert.SerializeObject(@object,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}