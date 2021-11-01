using HappyBusProject.HappyBusProject.DataLayer.InputModels;
using System.Linq;
using System.Text.RegularExpressions;

namespace HappyBusProject
{
    public static class UsersInputValidation
    {
        public static void AssignEmptyStringsToNullValues(UserInputModel usersInfo)
        {
            if (string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) usersInfo.PhoneNumber = string.Empty;
            if (string.IsNullOrWhiteSpace(usersInfo.Email)) usersInfo.Email = string.Empty;
        }

        public static bool UsersValuesValidation(UserInputModel usersInfo, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(usersInfo.FullName) && string.IsNullOrWhiteSpace(usersInfo.PhoneNumber) && string.IsNullOrWhiteSpace(usersInfo.Email))
            {
                errorMessage = "Name, phone and email fields empty";
                return false;
            }
            if (usersInfo.FullName.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(usersInfo.FullName))
            {
                errorMessage = "Invalid name";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) if (usersInfo.PhoneNumber.Length > 13 || usersInfo.PhoneNumber[1..].Any(c => !char.IsDigit(c)))
                {
                    errorMessage = "Invalid phone number";
                    return false;
                }
            if (!string.IsNullOrWhiteSpace(usersInfo.Email)) if (usersInfo.Email.Length > 30 || !new Regex(pattern: @"^([.,0-9a-zA-Z_-]{1,20}@[a-zA-Z]{1,10}.[a-zA-Z]{1,3})").IsMatch(usersInfo.Email))
                {
                    errorMessage = "Invalid E-Mail address type";
                    return false;
                }
            if (usersInfo.PhoneNumber.StartsWith("80")) usersInfo.PhoneNumber = "375" + usersInfo.PhoneNumber[2..];

            errorMessage = string.Empty;
            return true;
        }
    }
}
