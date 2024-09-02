using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public class UserLoginDTO
    {
       
        public string Username { get; set; }


        [MinLength(6, ErrorMessage = "Password has to be minmum 6 chars long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;
    }
}
