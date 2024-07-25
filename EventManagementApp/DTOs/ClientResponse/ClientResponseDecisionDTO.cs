using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.ClientResponse
{
    public class ClientResponseDecisionDTO
    {
        public ClientDecision ClientDecision { get; set; }
        public int? OrderId { get; set; }
        public bool? IsPaid { get; set; }
    }
}
