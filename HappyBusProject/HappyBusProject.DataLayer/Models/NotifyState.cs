using System;

namespace HappyBusProject.HappyBusProject.DataLayer.Models
{
    public class NotifyState
    {
        public Guid OrderID { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        public bool IsNotified { get; set; }
    }
}
