using System;

namespace HappyBusProject.HappyBusProject.DataLayer.Models
{
    public class CarsCurrentState
    {
        public Guid Id { get; set; }
        public bool IsBusyNow { get; set; }
        public int SeatsNum { get; set; }
        public int FreeSeatsNum { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
