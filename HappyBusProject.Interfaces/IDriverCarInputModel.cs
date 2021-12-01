using System;

namespace HappyBusProject.Interfaces
{
    public interface IDriverCarInputModel
    {
        public string DriverName { get; set; }
        public int DriverAge { get; set; }
        public DateTime MedicalExamPassDate { get; set; }
        public string CarBrand { get; set; }
        public int CarAge { get; set; }
        public int SeatsNum { get; set; }
        public string RegistrationNumPlate { get; set; }
    }
}
