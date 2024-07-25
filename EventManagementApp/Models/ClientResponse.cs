using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.Models
{
    public class ClientResponse
    {
        [Key]
        public int ClientResponseId { get; set; }
        public int QuotationResponseId { get; set; } // Foreign Key
        public ClientDecision ClientDecision { get; set; }
        public DateTime ClientResponseDate { get; set; } = DateTime.Now;
        public QuotationResponse QuotationResponse { get; set; }
        public Order Order { get; set; }
        public Review Review { get; set; }

    }
}
