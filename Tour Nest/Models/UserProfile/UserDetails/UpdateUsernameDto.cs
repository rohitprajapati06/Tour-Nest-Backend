using System.ComponentModel.DataAnnotations;

namespace TourNest.Models.UserProfile.UserDetails
{
    public class UpdateUsernameDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; } = null!;
    }
}

