using System;
using System.Collections.Generic;

#nullable disable

namespace HappyBusProject
{
    public partial class Car
    {
        public Car()
        {
            Drivers = new HashSet<Driver>();
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Brand { get; set; }
        public int SeatsNum { get; set; }
        public string RegistrationNumPlate { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
