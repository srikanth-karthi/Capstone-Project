namespace EventManagementApp.Exceptions
{
    public class NoQuotationRequestFoundException:Exception
    {
        public NoQuotationRequestFoundException(): base("Quotation Request is not found for given Id") { }
    }
}
