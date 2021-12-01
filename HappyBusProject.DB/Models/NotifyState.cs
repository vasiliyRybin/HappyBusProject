using HappyBusProject.Interfaces;
using System;

namespace HappyBusProject
{
    public class NotifyState : IBaseEntity
    {
        public Guid OrderID { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        public bool IsNotified { get; set; }
    }
}
