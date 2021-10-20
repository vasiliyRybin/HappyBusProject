using HappyBusProject.ModelsToReturn;
using System;
using System.Linq;

namespace HappyBusProject.Repositories
{
    public class UsersRepository : IUsersRepository<UsersInfo[]>
    {
        private readonly MyShuttleBusAppNewDBContext _context;
        private bool disposedValue = false;

        public UsersRepository(MyShuttleBusAppNewDBContext myShuttleBusAppNewDBContext)
        {
            _context = myShuttleBusAppNewDBContext;
        }

        public string Create(string name, string phoneNumber, string email)
        {
            string check = AppTools.UsersValuesValidation(name, ref phoneNumber, ref email);

            if (check != "ok") return check;

            try
            {
                User user = new()
                {
                    Id = Guid.NewGuid(),
                    FullName = name,
                    Rating = 5.0,
                    PhoneNumber = phoneNumber,
                    Email = email.ToLower(),
                    IsInBlacklist = false,
                    RegistrationDateTime = DateTime.Now
                };

                _context.Users.Add(user);
                int successUpdate = _context.SaveChanges();
                if (successUpdate > 0) return "User succesfully added";
                Dispose();
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method(param)");
                Dispose();
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
                    Dispose();
                    return "User successfully deleted";
                }
                Dispose();
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "DELETE method");
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
                Dispose();
                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                Dispose();
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
                Dispose();
                return result;
            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "GET Method");
                Dispose();
                return new UsersInfo[] { new UsersInfo { ErrorMessage = DateTime.Now + " " + e.Message } };
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public string Update(string name, string phoneNumber, string email)
        {
            string check = AppTools.UsersValuesValidation(name, ref phoneNumber, ref email);
            if (check != "ok") return check;

            try
            {
                var user = _context.Users.FirstOrDefault(c => c.FullName.Contains(name));
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(phoneNumber)) user.PhoneNumber = phoneNumber;
                    if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
                    Save();
                    Dispose();
                    return "Info successfully updated";
                }

                Dispose();
                return "No changes been made";

            }
            catch (Exception e)
            {
                AppTools.ErrorWriterTpFile(e.Message + " " + "PUT method");
                Dispose();
                return e.Message;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
