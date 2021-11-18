using HappyBusProject.DB.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace HappyBusProject
{
    public partial class Car: IEntity
    {
        public Car()
        {
            Drivers = new HashSet<Driver>();
            Orders = new HashSet<Order>();
        }

        public Guid CarId { get; set; }
        public string CarBrand { get; set; }
        public int SeatsNum { get; set; }
        public string RegistrationNumPlate { get; set; }
        public int CarAge { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
