using SIS.Framework.ActionResults.Contracts;

namespace IRunesWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(IndexViewModel model)
        {
            return this.View();
        }
    }
}
