using Chushka.Models;
using Chushka.Models.Enums;
using Chushka.ViewModels.Users;
using SIS.HTTP.Cookies;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chushka.Controllers
{
    public class UsersController:BaseController
    {
        public IHttpResponse Login()
        {
            return View("/users/login");
        }

        [HttpPost]
        public IHttpResponse Login(LogInViewModel model)
        {
            var user = this.db.Users
                .FirstOrDefault(x => x.Password == model.Password && x.Username == model.Username);

            if (user==null)
            {
                return this.BadRequestErrorWithView("Username or password was not correct");
            }

            var mvcUser = new MvcUserInfo
            {
                Username =user.Username,
                Role = user.Role.ToString(),
                Info = user.FullName
            };
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);
            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);

            return Redirect("/");
        }

        public IHttpResponse Register()
        {
            return View("/users/register");
        }

        [HttpPost]
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (model.Password!=model.ConfirmPassword)
            {
                this.BadRequestErrorWithView("Password and Confirm Password have to be equal");
            }
            var role = Role.User;
            if (this.db.Users.Any())
            {
                role = Role.Admin;
            }

            var user = new User
            {
               FullName=model.FullName,
               Username=model.Username,
               Password=model.Password,
               Email=model.Email,
               Role=role
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();

            return Redirect("/users/login");
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
