using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int EventCategoryId { get; set; } 
        public int ClientResponseId { get; set; }
        public int UserId { get; set; } 
        public double TotalAmount { get; set; }
        public Currency Currency { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public EventCategory EventCategory { get; set; }
        public ClientResponse ClientResponse { get; set; }
        public User User { get; set; }



    }
}
