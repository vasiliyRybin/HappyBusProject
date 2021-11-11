using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.InputModels.OrdersInputModels
{
    public class OrdersInputModelPutMethod
    {
        [Required]
        public string FullName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int OrderSeatsNum { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
    }
}
