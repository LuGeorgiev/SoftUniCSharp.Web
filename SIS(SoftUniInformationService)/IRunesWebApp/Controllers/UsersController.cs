using IRunesWebApp.Extensions;
using IRunesWebApp.Models;
using Services;
using Services.Contracts;
using SIS.Http.Cookies;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework.Routing;
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

        [HttpGet("/Users/Login")]
        public IHttpResponse Login() => this.View();

        [HttpPost("/Users/Login")]
        public IHttpResponse LoginPost()
        {
            var username = this.Request.FormData["username"].ToString();
            var password = this.Request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            var response =this.Redirect("/Home/Index");

            this.SignInUser(username);

            return response;
        }

        [HttpGet("/Users/Register")]
        public IHttpResponse Register() => this.View();

        [HttpPost("/Users/Register")]
        public IHttpResponse RegisterPost()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();
            var confirmPassword = this.Request.FormData["confirmPassword"].ToString();

            if (this.Context.Users.Any(x => x.Username == userName))
            {
                return new BadRequestResult("User with the same name already exists.", HttpResponseStatusCode.BadRequest);
            }

            if (password != confirmPassword)
            {
                return this.Redirect("/Users/Register");
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

            var response = this.Redirect("/");
            this.SignInUser(userName);
            return response;
        }

        [HttpGet("/Users/Logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie("Irunes_auth"))
            {
                return this.Redirect("/");
            }
            //TODO
            var cookie = this.Request.Cookies.GetCookie("Irunes_auth");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);

            //this.Request.Session.ClearParameters();

            return this.Redirect("/");
        }
    }
}
