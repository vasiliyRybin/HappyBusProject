using System;

namespace HappyBusProject.ViewModels
{
    public class OrderViewModel
    {
        public string CustomerName { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int OrderSeatsNum { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        public double TotalPrice { get; set; }
    }
}
