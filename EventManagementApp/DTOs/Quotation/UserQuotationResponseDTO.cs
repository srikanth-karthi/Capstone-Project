using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.Enums;
using EventManagementApp.Models;

namespace EventManagementApp.DTOs.Quotation
{
    public class UserQuotationResponseDTO
    {
        public int QuotationResponseId { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public double? QuotedAmount { get; set; }
        public Currency? Currency { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ResponseDate { get; set; }
        public ClientResponseDecisionDTO ClientResponse { get; set; }

    }
}
