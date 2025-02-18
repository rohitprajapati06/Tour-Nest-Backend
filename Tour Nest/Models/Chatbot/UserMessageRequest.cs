using System.ComponentModel.DataAnnotations;

namespace TourNest.Models.Chatbot
{
    public class UserMessageRequest
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Content is too long.")]
        public string Content { get; set; }
    }
}
