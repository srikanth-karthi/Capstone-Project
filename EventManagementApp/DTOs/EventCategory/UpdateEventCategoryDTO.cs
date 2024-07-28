namespace EventManagementApp.DTOs.EventCategory
{
    public class UpdateEventCategoryDTO
    {
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public string? Poster { get; set; }
        public bool? IsActive { get; set; }
    }
}
