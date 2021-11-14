using System;
using System.Collections.Generic;

#nullable disable

namespace HappyBusProject
{
    public partial class Driver
    {
        public Driver()
        {
            DriversRatingHistories = new HashSet<DriversRatingHistory>();
        }

        public Guid Id { get; set; }
        public string DriverName { get; set; }
        public Guid CarId { get; set; }
        public double Rating { get; set; }
        public int DriverAge { get; set; }
        public DateTime? MedicalExamPassDate { get; set; }

        public virtual Car Car { get; set; }
        public virtual ICollection<DriversRatingHistory> DriversRatingHistories { get; set; }
    }
}
