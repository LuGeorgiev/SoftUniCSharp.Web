using IRunesWebApp.Extensions;
using IRunesWebApp.Models;
using Services;
using Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Http.Cookies;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Linq;
using System.Net;

namespace IRunesWebApp.Controllers
{
    public class UsersController:BaseController
    {
        private readonly IHashService hashService;

        public UsersController()
        {
            this.hashService = new HashService();
        }

        public IActionResult Login() => this.View();

        public IActionResult LoginPost()
        {
            var username = this.Request.FormData["username"].ToString();
            var password = this.Request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);

            if (user == null)
            {
                return this.RedirectToAction("/Users/Login");
            }

            var response = new RedirectResult("/Home/Index");

            this.SignInUser(username, response, this.Request);

            return this.View("/");
        }

        public IActionResult Register() => this.View();

        public IActionResult RegisterPost()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();
            var confirmPassword = this.Request.FormData["confirmPassword"].ToString();

            if (this.Context.Users.Any(x => x.Username == userName))
            {
                return this.RedirectToAction("/users/login");
            }

            if (password != confirmPassword)
            {
                return this.RedirectToAction("/Users/Register");
            }
     
            var hashedPassword = this.hashService.Hash(password);
                        
            var user = new User
            {
                Username = userName,
                HashedPassword = hashedPassword,
            };
            this.Context.Users.Add(user);

            
                this.Context.SaveChanges();
           
            var response = new RedirectResult("/");

            this.SignInUser(userName, response, this.Request);

            return this.RedirectToAction("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.ClearParameters();

            return new RedirectResult("/");
        }
    }
}
