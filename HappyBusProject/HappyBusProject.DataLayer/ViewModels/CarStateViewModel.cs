using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.ViewModels
{
    public class CarStateViewModel
    {
        public string DriverName { get; set; }
        public string CarBrand { get; set; }
        public bool IsBusyNow { get; set; }
        public int SeatsNum { get; set; }
        public int FreeSeatsNum { get; set; }
    }
}
