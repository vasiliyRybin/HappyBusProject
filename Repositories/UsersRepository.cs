using HappyBusProject.ModelsToReturn;
using System;
using System.Linq;

namespace HappyBusProject.Repositories
{
    public class UsersRepository : IUsersRepository<UsersInfo[]>
    {
        private readonly MyShuttleBusAppNewDBContext _context;

        public UsersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        public string Create(UsersInfo usersInfo)
        {
            string check = UsersInputValidation.UsersValuesValidation(usersInfo);

            if (check != "ok") return check;

            try
            {
                User user = new()
                {
                    Id = Guid.NewGuid(),
                    FullName = usersInfo.Name,
                    Rating = 5.0,
                    PhoneNumber = usersInfo.PhoneNumber,
                    Email = usersInfo.Email.ToLower(),
                    IsInBlacklist = false,
                    RegistrationDateTime = DateTime.Now
                };

                _context.Users.Add(user);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return "User succesfully added";
                return "No changes been made";

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "POST Method");
                return e.Message;
            }
        }

        public string Delete(string name)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(name));

                if (user != null)
                {
                    _context.Remove(user);
                    Save();
                    return "User successfully deleted";
                }
                return "No changes been made";

            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "DELETE method");
                return e.Message;
            }
        }

        public UsersInfo[] GetAll()
        {
            try
            {
                var users = _context.Users.ToList();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }
                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method");
                return new UsersInfo[] { new UsersInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public UsersInfo[] GetByName(string value)
        {
            try
            {
                var users = _context.Users.Where(u => u.FullName.Contains(value)).ToList();
                UsersInfo[] result = new UsersInfo[users.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new UsersInfo { Name = users[i].FullName, Rating = users[i].Rating, PhoneNumber = users[i].PhoneNumber, Email = users[i].Email, IsInBlackList = users[i].IsInBlacklist };
                }
                return result;
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "GET Method (by value)");
                return new UsersInfo[] { new UsersInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public string Update(UsersInfo usersInfo)
        {
            string check = UsersInputValidation.UsersValuesValidation(usersInfo);
            if (check != "ok") return check;

            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(usersInfo.Name));
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(usersInfo.PhoneNumber)) user.PhoneNumber = usersInfo.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(usersInfo.Email)) user.Email = usersInfo.Email;
                    Save();
                    return "Info successfully updated";
                }

                return "No changes been made";
            }
            catch (Exception e)
            {
                LogWriter.ErrorWriterToFile(e.Message + " " + "PUT method");
                return e.Message;
            }
        }
    }
}
