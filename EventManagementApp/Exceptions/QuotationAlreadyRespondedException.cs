namespace EventManagementApp.Exceptions
{
    public class QuotationAlreadyRespondedException:Exception
    {
        public QuotationAlreadyRespondedException(): base("Quotation is already responded") { }
    }
}
