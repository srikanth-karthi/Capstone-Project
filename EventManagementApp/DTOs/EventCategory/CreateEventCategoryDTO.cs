using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.EventCategory
{
    public class CreateEventCategoryDTO
    {
        public string EventName { get; set; }
        public string Poster { get; set; }
        [Required]
        public bool IsService { get; set; }
        public string Description { get; set; }
    }
}
