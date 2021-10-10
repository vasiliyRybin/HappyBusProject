using System;
using System.Collections.Generic;

#nullable disable

namespace HappyBusProject
{
    public partial class UsersRatingHistory
    {
        public Guid RecordId { get; set; }
        public Guid UserId { get; set; }
        public int Value { get; set; }
        public DateTime RatedWhenDateTime { get; set; }
        public int RouteStartPointId { get; set; }
        public int RouteEndPointId { get; set; }
        public string Comment { get; set; }

        public virtual User User { get; set; }
    }
}
