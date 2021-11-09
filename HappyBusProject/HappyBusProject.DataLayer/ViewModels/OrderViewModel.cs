using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.ViewModels
{
    public class OrderViewModel
    {
        public DateTime OrderDateTime { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DepartureTime { get; set; }
        public double TotalPrice { get; set; }
    }
}
