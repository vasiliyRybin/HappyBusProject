using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.ViewModels
{
    public class OrderViewModel
    {
        public DateTime OrderDateTime { get; set; }
        public string OrderType { get; set; }
        public int StartPointId { get; set; }
        public int EndPointId { get; set; }
        public double TotalPrice { get; set; }
    }
}
