using System;
using System.Collections.Generic;

#nullable disable

namespace HappyBusProject
{
    public partial class User
    {
        public User()
        {
            DriversRatingHistories = new HashSet<DriversRatingHistory>();
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public double Rating { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsInBlacklist { get; set; }
        public DateTime RegistrationDateTime { get; set; }

        public virtual UsersRatingHistory UsersRatingHistory { get; set; }
        public virtual ICollection<DriversRatingHistory> DriversRatingHistories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
