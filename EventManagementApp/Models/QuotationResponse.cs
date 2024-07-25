using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.Models
{
    public class QuotationResponse
    {
        [Key]
        public int QuotationResponseId {  get; set; }
        public int QuotationRequestId { get; set; } 
        public RequestStatus RequestStatus { get; set; }
        public double? QuotedAmount { get; set; }
        public Currency? Currency { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ResponseDate { get; set; } = DateTime.Now;
        public QuotationRequest QuotationRequest { get; set; }
        public ClientResponse ClientResponse { get; set; }

    }
}
