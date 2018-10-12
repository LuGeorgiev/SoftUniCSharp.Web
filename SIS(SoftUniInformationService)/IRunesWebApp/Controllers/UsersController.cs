using IRunesWebApp.Extensions;
using IRunesWebApp.Models;
using Services;
using Services.Contracts;
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

        public IHttpResponse Login(IHttpRequest request) => this.View();

        public IHttpResponse LoginPost(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);

            if (user == null)
            {
                return new RedirectResult("/Users/Login");
            }

            var response = new RedirectResult("/Home/Index");

            this.SignInUser(username, response, request);

            return response;
        }

        public IHttpResponse Register(IHttpRequest request) => this.View();

        public IHttpResponse RegisterPost(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            if (this.Context.Users.Any(x => x.Username == userName))
            {
                return new BadRequestResult("User with the same name already exists.", HttpResponseStatusCode.BadRequest);
            }

            if (password != confirmPassword)
            {
                return new RedirectResult("/Users/Register");
            }
     
            var hashedPassword = this.hashService.Hash(password);
                        
            var user = new User
            {
                Username = userName,
                HashedPassword = hashedPassword,
            };
            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            var response = new RedirectResult("/");

            this.SignInUser(userName, response, request);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.ClearParameters();

            return new RedirectResult("/");
        }
    }
}
