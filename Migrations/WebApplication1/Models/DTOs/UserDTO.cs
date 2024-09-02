using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public class UserDTO : User
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string Password { get; set; }
    }
}
