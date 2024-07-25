using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.User
{
    public class UserOrderListReturnDTO
    {
        public int OrderId { get; set; }
        public double TotalAmount { get; set; }
        public Currency Currency { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public UserQuotationRequestDTO EventDetails { get; set; }
    }
}
