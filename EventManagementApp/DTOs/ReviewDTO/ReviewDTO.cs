using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.ReviewDTO
{
    public class ReviewDTO
    {
        [Required]
        [Range(0,5, ErrorMessage = "Rating should be in between 0 - 5")]
        public float Rating { get; set; }
        public string Comments { get; set; }
    }
}
