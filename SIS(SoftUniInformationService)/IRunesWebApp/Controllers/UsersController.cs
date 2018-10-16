using IRunesWebApp.Extensions;
using IRunesWebApp.Models;
using IRunesWebApp.ViewModels.Users;
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

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/Users/Login")]
        public IHttpResponse Login() => this.View();

        [HttpPost("/Users/Login")]
        public IHttpResponse LoginPost(LoginPostViewModel model)
        {    
            var hashedPassword = this.hashService.Hash(model.Password);

            var user = this.Context
                .Users
                .FirstOrDefault(u => u.Username == model.Username && u.HashedPassword == hashedPassword);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            var response =this.Redirect("/");

            this.SignInUser(model.Username);

            return response;
        }

        [HttpGet("/Users/Register")]
        public IHttpResponse Register() => this.View();

        [HttpPost("/Users/Register")]
        public IHttpResponse RegisterPost(RegisterPostViewModel model)
        {          

            if (this.Context.Users.Any(x => x.Username == model.Username))
            {
                return new BadRequestResult("User with the same name already exists.", HttpResponseStatusCode.BadRequest);
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }
     
            var hashedPassword = this.hashService.Hash(model.Password);
                        
            var user = new User
            {
                Username = model.Username,
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
            this.SignInUser(model.Username);
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
