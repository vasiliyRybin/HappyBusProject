using System;

namespace HappyBusProject.InputModels
{
    public class CarStateInputModel
    {
        public CarStateInputModel()
        {
            IsBusyNow = false;
        }

        public bool IsBusyNow { get; set; }
        public int FreeSeatsNum { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
