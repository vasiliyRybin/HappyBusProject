using HappyBusProject.Interfaces;
using System;

#nullable disable

namespace HappyBusProject
{
    public partial class DriversRatingHistory : IBaseEntity
    {
        public Guid RecordId { get; set; }
        public Guid DriverId { get; set; }
        public int Value { get; set; }
        public Guid WhoRatedId { get; set; }
        public DateTime RatedWhenDateTime { get; set; }
        public int RouteStartPointId { get; set; }
        public int RouteEndPointId { get; set; }
        public string Comment { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual User WhoRated { get; set; }
    }
}
