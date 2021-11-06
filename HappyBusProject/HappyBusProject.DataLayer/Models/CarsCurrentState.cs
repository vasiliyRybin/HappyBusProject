using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.Models
{
    public class CarsCurrentState
    {
        public Guid Id { get; set; }
        public bool IsBusyNow { get; set; }
        public int SeatsNum { get; set; }
        public int FreeSeatsNum { get; set; }
    }
}
