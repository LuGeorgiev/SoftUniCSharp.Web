using SIS.HTTP.Cookies;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.ViewModels.Users;

namespace TORSHIA.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        
        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        
        [HttpPost("/Users/Register")]
        public IHttpResponse Register(RegisterViewModel model)
        {

            // Validate
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid email with length of 4 or more characters.");
            }

            if (this.db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestError("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 3)
            {
                return this.BadRequestError("Please provide password of length 3 or more.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(model.Password);

            var role = Role.User;
            if (!this.db.Users.Any())
            {
                role = Role.Admin;
            }

            // Create user
            var user = new User
            {
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                Password = hashedPassword,
                Role = role,
            };
            this.db.Users.Add(user);

            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // Redirect
            return this.Redirect("/Users/Login");
        }

        
        [HttpPost("/Users/Login")]
        public IHttpResponse Login(LogInViewModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);
            var user = this.db.Users.FirstOrDefault(x =>
                x.Username == model.Username.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            var mvcUser = new MvcUserInfo { Username = user.Username, Role = user.Role.ToString(), Info = user.Email };
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }
               
        public IHttpResponse Register()
        {
            return this.View();
        }

        public IHttpResponse Login()
        {
            return this.View();
        }
                
        [Authorize]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

    }
}
