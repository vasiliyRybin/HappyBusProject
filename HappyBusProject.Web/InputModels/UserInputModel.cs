using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.InputModels
{
    public class UserInputModel
    {
        [Required]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
