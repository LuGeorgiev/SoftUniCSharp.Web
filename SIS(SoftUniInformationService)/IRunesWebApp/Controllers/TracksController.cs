using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using IRunesWebApp.Models;
using SIS.Framework.ActionResults.Contracts;

namespace IRunesWebApp.Controllers
{
    public class TracksController : BaseController
    {
        public IActionResult Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return this.View("/Users/Login");
            }
                      

            var albumId = request.QueryData["albumId"].ToString();
            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);
            this.ViewBag["albumId"] = albumId;

            return this.View("/albums/all");
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            var trackName = HttpUtility.UrlDecode(request.FormData["trackName"].ToString());
            var trackLink = HttpUtility.UrlDecode(request.FormData["trackLink"].ToString());
            var trackPrice = decimal.Parse(request.FormData["trackPrice"].ToString());

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return new BadRequestResult("Must provide a proper album id in query!", HttpResponseStatusCode.BadRequest);
            }

            var albumId = request.QueryData["albumId"].ToString();
            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                return new BadRequestResult($"Album with id {albumId} does not exist!", HttpResponseStatusCode.NotFound);
            }

            if (album.Tracks.Any(x => x.Track.Name == trackName))
            {
                return new BadRequestResult("Track with the same name already exists.", HttpResponseStatusCode.BadRequest);
            }

            // Create track
            var track = new Track
            {
                Name = trackName,
                Link = trackLink,
                Price = trackPrice
            };
            this.Context.Tracks.Add(track);

            this.Context.TrackAblums.Add(new TrackAblum { AlbumId = albumId, TrackId = track.Id });

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            return new RedirectResult($"/Albums/Details?id={albumId}");
        }

        public IActionResult Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return this.View("/Users/Login");
            }

            //if (!request.QueryData.ContainsKey("albumId"))
            //{
            //    return new BadRequestResult("Must provide a proper album id in query!", HttpResponseStatusCode.BadRequest);
            //}

            //if (!request.QueryData.ContainsKey("trackId"))
            //{
            //    return new BadRequestResult("Must provide a proper track id in query!", HttpResponseStatusCode.BadRequest);
            //}

            var albumId = request.QueryData["albumId"].ToString();
            var trackId = request.QueryData["trackId"].ToString();

            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);
            var track = this.Context.Tracks.FirstOrDefault(x => x.Id == trackId);

            //if (album == null)
            //{
            //    return new BadRequestResult($"Album with id {albumId} not found!", HttpResponseStatusCode.NotFound);
            //}
            //if (track == null)
            //{
            //    return new BadRequestResult($"Track with id {trackId} not found!", HttpResponseStatusCode.NotFound);
            //}

            this.ViewBag["trackLink"] = track.Link;
            this.ViewBag["trackName"] = track.Name;
            this.ViewBag["trackPrice"] = track.Price.ToString("F2");
            this.ViewBag["albumId"] = album.Id;

            return this.View("Details");
        }
    }
}
