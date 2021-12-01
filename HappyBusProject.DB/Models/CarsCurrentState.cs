using HappyBusProject.Interfaces;
using System;

namespace HappyBusProject
{
    public class CarsCurrentState : IBaseEntity
    {
        public Guid Id { get; set; }
        public bool IsBusyNow { get; set; }
        public int SeatsNum { get; set; }
        public int FreeSeatsNum { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
