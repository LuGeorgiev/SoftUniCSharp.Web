using IRunesWebApp.Data;
using IRunesWebApp.Models;
using IRunesWebApp.Services.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunesWebApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRunesContext context;
        private readonly IHashService hashService;

        public UsersService(IRunesContext context, IHashService hashService)
        {
            this.context = context;
            this.hashService = hashService;
        }

        public bool ExistsByUsernameAndPassword(string username, string password)
        {
            var hashedPassword = this.hashService.Hash(password);

            var userExists = this.context.Users
                .Any(u => u.Username == username &&
                          u.HashedPassword == hashedPassword);

            return userExists;
        }

        public bool RegisterUser(string username, string password)
        {
            if (context.Users.Any(x=>x.Username==username))
            {
                return false;
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                HashedPassword = hashedPassword,
            };
            this.context.Users.Add(user);

            try
            {
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
