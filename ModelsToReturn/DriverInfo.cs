namespace HappyBusProject.ModelsToReturn
{
    public class DriverInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Rating { get; set; }
        public string CarBrand { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
