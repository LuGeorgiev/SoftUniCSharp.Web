using IRunesWebApp.Services.Contracts;
using IRunesWebApp.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;

namespace IRunesWebApp.Controllers
{
    public class UsersController:Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Login() => this.View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid.HasValue || !ModelState.IsValid.Value)
            {
                return this.RedirectToAction("/users/login");
            }

            var userExists = this.usersService
                .ExistsByUsernameAndPassword(
                    model.Username,
                    model.Password);

            if (!userExists)
            {
                return this.RedirectToAction("/users/login");
            }

            this.Request.Session.AddParameter("username", model.Username);

            return this.RedirectToAction("/home/indexloggedin");
        }
        

        public IActionResult Register() => this.View();

        public IActionResult RegisterPost(RegisterViewModel model)
        {
            if (!ModelState.IsValid.HasValue || !ModelState.IsValid.Value)
            {
                return this.RedirectToAction("/users/register");
            }

            var userName = model.Username;
            var password = model.Password.Trim();
            var confirmPassword = model.ConfirmPassword.Trim();

            if (password!=confirmPassword)
            {
                return this.RedirectToAction("/users/login");
            }

            var wasSuccessfullyRegistered = this.usersService.RegisterUser(userName, password);

            if (!wasSuccessfullyRegistered)
            {
                return this.RedirectToAction("/users/login");
            }

            this.Request.Session.AddParameter("username", model.Username);
            return this.RedirectToAction("/home/indexloggedin");
        }

        public IActionResult Logout()
        {
            this.Request.Session.ClearParameters();

            return this.RedirectToAction("/");
        }
    }
}
