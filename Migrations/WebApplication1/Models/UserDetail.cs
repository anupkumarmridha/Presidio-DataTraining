using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordHashKey { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Status { get; set; }
    }
}
