using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.InputModels
{
    public class OrderInputModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string StartPoint { get; set; }
        [Required]
        public string EndPoint { get; set; }
        public string OrderType { get; set; }
    }
}
