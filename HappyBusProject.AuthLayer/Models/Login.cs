using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.AuthLayer.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string UserLogin { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
