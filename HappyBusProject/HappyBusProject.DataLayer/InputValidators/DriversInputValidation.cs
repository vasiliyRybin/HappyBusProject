using HappyBusProject.ModelsToReturn;
using System;
using System.Text.RegularExpressions;

namespace HappyBusProject.InputValidators
{
    public static class DriversInputValidation
    {
        public static bool IsEmptyInputValues(DriverCarPreResultModel driverCar)
        {
            if (string.IsNullOrWhiteSpace(driverCar.CarBrand)
                || string.IsNullOrWhiteSpace(driverCar.SeatsNum)
                || string.IsNullOrWhiteSpace(driverCar.RegistrationNumPlate)
                || string.IsNullOrWhiteSpace(driverCar.CarAge)
               )
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(driverCar.DriverName) || string.IsNullOrWhiteSpace(driverCar.DriverAge))
            {
                return false;
            }

            return true;
        }

        public static bool PutMethodInputValidation(DriverCarPreResultModel driverCar, out string errorMessage)
        {
            if (driverCar.CarBrand.Length > 30)
            {
                errorMessage = "Invalid brand name";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool DriversInputValidator(DriverCarPreResultModel driverCar, out int numSeats, out int carAgeInt, out int driverAgeInt, out DateTime dateTimeResult, out string errorMessage)
        {
            if (driverCar.CarBrand.Length > 30)
            {
                errorMessage = "Invalid brand name";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (!int.TryParse(driverCar.SeatsNum, out numSeats) || numSeats <= 8 || numSeats > 50)
            {
                errorMessage = "Invalid number of seats or value too low";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (driverCar.RegistrationNumPlate.Length > 9 || !new Regex(@"\d{4}\s\w{2}.\d{1}").IsMatch(driverCar.RegistrationNumPlate))
            {
                errorMessage = "Invalid registration plate number";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (!int.TryParse(driverCar.CarAge, out carAgeInt) || carAgeInt < 0 || carAgeInt > 15)
            {
                errorMessage = "Invalid car age or car age is too big";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (driverCar.DriverName.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(driverCar.DriverName))
            {
                errorMessage = "Invalid name";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (!int.TryParse(driverCar.DriverAge, out driverAgeInt) || driverAgeInt < 21 || driverAgeInt > 65)
            {
                errorMessage = "Invalid age.";
                SetDefaultValues(out numSeats, out carAgeInt, out driverAgeInt, out dateTimeResult);
                return false;
            }
            if (!DateTime.TryParse(driverCar.ExamPass, out DateTime resultExamPass))
            {
                errorMessage = string.Empty;
                dateTimeResult = DateTime.Parse("1900-01-01 00:00:00");
                return false;
            }


            errorMessage = string.Empty;
            dateTimeResult = resultExamPass;
            return true;
        }

        private static void SetDefaultValues(out int numSeats, out int carAgeInt, out int driverAgeInt, out DateTime dateTimeResult)
        {
            numSeats = -1;
            carAgeInt = -1;
            driverAgeInt = -1;
            dateTimeResult = DateTime.Parse("1900-01-01 00:00:00");
        }
    }
}
