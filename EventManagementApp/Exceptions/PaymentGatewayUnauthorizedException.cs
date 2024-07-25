namespace EventManagementApp.Exceptions
{
    public class PaymentGatewayUnauthorizedException: Exception
    {
        public PaymentGatewayUnauthorizedException(): base("Payment API Key is missing or Invalid") { }    
    }
}
