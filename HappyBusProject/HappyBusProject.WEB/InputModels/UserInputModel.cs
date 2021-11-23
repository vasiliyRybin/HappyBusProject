using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.HappyBusProject.DataLayer.InputModels
{
    public class UserInputModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
