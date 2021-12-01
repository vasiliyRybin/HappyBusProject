using HappyBusProject.Interfaces;
using System;

namespace HappyBusProject.InputModels
{
    public class PutMethodDriverInputModel : IDriverCarInputModel
    {
        public string DriverName { get; set; }
        public DateTime MedicalExamPassDate { get; set; }
        public string CarBrand { get; set; }
        public int CarAge { get; set; }
        public int SeatsNum { get; set; }
        public string RegistrationNumPlate { get; set; }
        public int DriverAge { get; set; }
    }
}
