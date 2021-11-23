using HappyBusProject.ModelsToReturn;
using System;
using System.Text.RegularExpressions;

namespace HappyBusProject.InputValidators
{
    public static class DriversInputValidation
    {
        public static bool IsEmptyInputValues(DriverCarInputModel driverCar)
        {
            if (string.IsNullOrWhiteSpace(driverCar.CarBrand) || string.IsNullOrWhiteSpace(driverCar.RegistrationNumPlate) || string.IsNullOrWhiteSpace(driverCar.DriverName))
            {
                return false;
            }

            return true;
        }

        public static bool PutMethodInputValidation(DriverCarInputModel driverCar, out string errorMessage)
        {
            if (driverCar.CarBrand.Length > 30)
            {
                errorMessage = "Invalid brand name";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool DriversInputValidator(DriverCarInputModel driverCar, out string errorMessage)
        {
            if (driverCar.CarBrand.Length > 30)
            {
                errorMessage = "Invalid brand name";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.SeatsNum <= 8 || driverCar.SeatsNum > 50)
            {
                errorMessage = "Invalid number of seats or value too low";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.RegistrationNumPlate.Length > 9 || !new Regex(@"\d{4}\s\w{2}.\d{1}").IsMatch(driverCar.RegistrationNumPlate))
            {
                errorMessage = "Invalid registration plate number";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.CarAge < 0 || driverCar.CarAge > 15)
            {
                errorMessage = "Invalid car age or car age is too big";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.DriverName.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(driverCar.DriverName))
            {
                errorMessage = "Invalid name";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.DriverAge < 21 || driverCar.DriverAge > 65)
            {
                errorMessage = "Invalid age.";
                SetDefaultValues(driverCar);
                return false;
            }
            if (driverCar.MedicalExamPassDate < DateTime.Parse("01.01.2020") || driverCar.MedicalExamPassDate > DateTime.Now)
            {
                errorMessage = "Invalid ExamPass date";
                SetDefaultValues(driverCar);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        private static void SetDefaultValues(DriverCarInputModel driverCar)
        {
            driverCar.SeatsNum = -1;
            driverCar.SeatsNum = -1;
            driverCar.DriverAge = -1;
            driverCar.MedicalExamPassDate = DateTime.Parse("1900-01-01 00:00:00");
        }
    }
}
