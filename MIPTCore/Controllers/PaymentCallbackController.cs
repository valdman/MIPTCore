using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DonationManagment.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MIPTCore.Models;
using Newtonsoft.Json;

namespace MIPTCore.Controllers
{
    public class PaymentCallbackController : Controller
    {
        [HttpGet("~/payment/callback")]
        public async Task<IActionResult> HandlePaymentCallback([FromQuery] PaymentCallbackModel paymentCallbackModel)
        {
            _logger.LogInformation($"Callback {JsonConvert.SerializeObject(paymentCallbackModel)}");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!CheckCallBackIntegrity(paymentCallbackModel))
                return Forbid("Broken callback integrity");

            if (!paymentCallbackModel.Operation.Equals("deposited") ||
                paymentCallbackModel.Status != PaymentOperationStatus.Success)
            {
                _logger.LogWarning(
                    $"Callback {JsonConvert.SerializeObject(paymentCallbackModel)} handled, nothing happend");
                return Ok();
            }

            var donationToConfirm = await _donationManager.GetDonationByIdAsync(paymentCallbackModel.OrderNumber);
            if (donationToConfirm == null)
            {
                _logger.LogError($"Donation #{paymentCallbackModel.OrderNumber} not found");
                return BadRequest(
                    $"Donation with id {paymentCallbackModel.OrderNumber} doesn't exist (OrderNumber ivalid)");
            }

            await _donationManager.ConfirmDonation(donationToConfirm);
                
                
            _logger.LogInformation($"Donation {JsonConvert.SerializeObject(donationToConfirm)} confirmed via callback");
            return Ok("Donation confirmed");
        }

        private bool CheckCallBackIntegrity(PaymentCallbackModel paymentCallbackModel)
        {
            return true;
        }
        
        public PaymentCallbackController(IDonationManager donationManager, ILogger<PaymentCallbackController> logger)
        {
            _donationManager = donationManager;
            _logger = logger;
        }
        
        private readonly IDonationManager _donationManager;
        private readonly ILogger<PaymentCallbackController> _logger;
    }
}