namespace EventManagementApp.Exceptions
{
    public class NoQuotationResponseFoundException : Exception
    {
        public NoQuotationResponseFoundException(): base("Quotation Response is not found for given Id") { }
    }
}
