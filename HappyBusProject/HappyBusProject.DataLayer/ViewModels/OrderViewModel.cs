using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.ViewModels
{
    public class OrderViewModel
    {
        public DateTime OrderDateTime { get; set; }
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public double TotalPrice { get; set; }
    }
}
