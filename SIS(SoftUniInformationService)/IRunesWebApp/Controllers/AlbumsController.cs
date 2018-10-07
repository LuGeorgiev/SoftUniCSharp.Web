using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunesWebApp.Controllers
{
    public class AlbumsController: BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var albums = this.Context.Albums;
            var listOfAlbums = string.Empty;
            if (albums.Any())
            {
                foreach (var album in albums)
                {

                }
            }

            return this.View();
        }
    }
}
