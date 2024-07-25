using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.QuotationRequest
{
    public class CreateQuotationResponseDTO
    {
        [Required]
        public int? QuotationRequestId { get; set; }

        [Required]
        public RequestStatus RequestStatus { get; set; }

        public Currency? Currency { get; set; }

        public double? QuotedAmount { get; set; }
        
        [Required]
        public string ResponseMessage { get; set; }
    }
}
