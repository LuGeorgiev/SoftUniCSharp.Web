using IRunesWebApp.Models;
using Services;
using Services.Contracts;
using SIS.Http.Cookies;
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

        public IHttpResponse Login(IHttpRequest request)
            => this.View();

        public IHttpResponse PostLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();
            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users
                .FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);
            
            if(user==null)
            {
                return new RedirectResult("/login");
            }
            this.ViewBag[username] = username;//TODO refactoring needed

            var response =  new RedirectResult("/home/index");
            this.SignInUser(username, request, response);
            return response;
            //return this.View();
        }

        public IHttpResponse Register(IHttpRequest request)
            => this.View();

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            if (password!=confirmPassword)
            {
                return new BadRequestResult("Passwords do not match", HttpStatusCode.SeeOther);
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username=username,
                HashedPassword=hashedPassword                 
            };

            this.ViewBag[username] = username;//TODO refactoring needed

            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpStatusCode.InternalServerError);
            }

            var response =  new RedirectResult("/");
            this.SignInUser(username, request,response);
            return response;
        }
    }
}
