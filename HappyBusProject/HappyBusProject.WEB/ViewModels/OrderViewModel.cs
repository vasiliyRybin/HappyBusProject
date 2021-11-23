using System;

namespace HappyBusProject.HappyBusProject.DataLayer.ViewModels
{
    public class OrderViewModel
    {
        public DateTime OrderDateTime { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        public double TotalPrice { get; set; }
    }
}
