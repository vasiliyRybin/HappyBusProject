using HappyBusProject.ModelsToReturn;
using System.Linq;
using System.Text.RegularExpressions;

namespace HappyBusProject
{
    public static class UsersInputValidation
    {
        public static void AssignEmptyStringsToNullValues(UsersInfo usersInfo)
        {
            if (string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) usersInfo.PhoneNumber = string.Empty;
            if (string.IsNullOrWhiteSpace(usersInfo.Email)) usersInfo.Email = string.Empty;
        }

        public static string UsersValuesValidation(UsersInfo usersInfo)
        {
            if (string.IsNullOrWhiteSpace(usersInfo.Name) && string.IsNullOrWhiteSpace(usersInfo.PhoneNumber) && string.IsNullOrWhiteSpace(usersInfo.Email)) return "Name, phone and email fields empty";
            if (usersInfo.Name.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(usersInfo.Name)) return "Invalid name";
            if (!string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) if (usersInfo.PhoneNumber.Length > 13 || usersInfo.PhoneNumber[1..].Any(c => !char.IsDigit(c))) return "Invalid phone number";
            if (!string.IsNullOrWhiteSpace(usersInfo.Email)) if (usersInfo.Email.Length > 30 || !new Regex(pattern: @"^([.,0-9a-zA-Z_-]{1,20}@[a-zA-Z]{1,10}.[a-zA-Z]{1,3})").IsMatch(usersInfo.Email)) return "Invalid E-Mail address type";
            if (usersInfo.PhoneNumber.StartsWith("80")) usersInfo.PhoneNumber = "375" + usersInfo.PhoneNumber[2..];

            return "ok";
        }
    }
}
