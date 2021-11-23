using System;
using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.HappyBusProject.DataLayer.InputModels.CarStateModels
{
    public class CarStatePostModel
    {
        [Required]
        public string DriverName { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
    }
}
