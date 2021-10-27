#nullable enable
#pragma warning disable CS8618

namespace HappyBusProject.ModelsToReturn
{
    public class UsersViewModel
    {
        public string? Name { get; set; }
        public double Rating { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsInBlackList { get; set; }
    }
}
