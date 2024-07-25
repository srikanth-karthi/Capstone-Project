using EventManagementApp.DTOs.Quotation;
using EventManagementApp.Enums;
using EventManagementApp.Models;

namespace EventManagementApp.DTOs.QuotationRequest
{
    public class UserQuotationRequestDTO : BasicQuotationRequestDTO
    {
        public UserQuotationResponseDTO QuotationResponse { get; set; }
    }
}
