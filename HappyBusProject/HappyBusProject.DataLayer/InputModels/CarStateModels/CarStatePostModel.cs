using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
