namespace EventManagementApp.DTOs.EventCategory
{
    public class UpdateEventCategoryDTO
    {
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
