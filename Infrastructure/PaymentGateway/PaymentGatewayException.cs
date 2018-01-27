using Common.DomainSteroids;

namespace PaymentGateway
{
    public class PaymentGatewayException : DomainException
    {
        private readonly string _content;
        
        public PaymentGatewayException(string content)
        {
            _content = content;
        }
        public override string Message => $"Payment gateway response failed: {_content}";
    }
}