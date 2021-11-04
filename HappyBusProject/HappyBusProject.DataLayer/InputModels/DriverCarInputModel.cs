using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.ModelsToReturn
{
    public class DriverCarInputModel
    {
        [Required]
        public string DriverName { get; set; }
        [Required]
        public string DriverAge { get; set; }
        public string ExamPass { get; set; } = "1900-01-01";
        [Required]
        public string CarBrand { get; set; }
        [Required]
        public string CarAge { get; set; }
        [Required]
        public string SeatsNum { get; set; }
        [Required]
        public string RegistrationNumPlate { get; set; }
    }
}
