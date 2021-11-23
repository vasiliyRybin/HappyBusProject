﻿using System;

#nullable disable

namespace HappyBusProject
{
    public partial class Order
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        public string OrderType { get; set; }
        public int StartPointId { get; set; }
        public int EndPointId { get; set; }
        public int OrderSeatsNum { get; set; }
        public double TotalPrice { get; set; }
        public bool IsActual { get; set; }

        public virtual Car Car { get; set; }
        public virtual User Customer { get; set; }
    }
}
