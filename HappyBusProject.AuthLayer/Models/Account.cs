using System;

namespace HappyBusProject.AuthLayer.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role[] Roles { get; set; }
    }

    public enum Role
    {
        User,
        Driver,
        Admin
    }
}
