using System;
using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.InputModels
{
    public class CarStatePostModel
    {
        [Required]
        public string DriverName { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
    }
}
