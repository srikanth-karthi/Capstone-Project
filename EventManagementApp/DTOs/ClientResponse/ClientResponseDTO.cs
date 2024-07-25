using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.ClientResponse
{
    public class ClientResponseDTO
    {
        public int QuotationResponseId { get; set; }
        public ClientDecision ClientDecision { get; set; }
    }
}
