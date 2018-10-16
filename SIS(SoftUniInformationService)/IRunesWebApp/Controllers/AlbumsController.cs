using IRunesWebApp.Extensions;
using IRunesWebApp.Models;
using IRunesWebApp.ViewModels.Albums;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework.Routing;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IRunesWebApp.Controllers
{
    public class AlbumsController: BaseController
    {
        [HttpGet("/Albums/All")]
        public IHttpResponse All()
        {
            if (!this.IsAuthenticated(this.Request))
            {
                return this.Redirect("/Users/Login");
            }

            var albums = this.Context.Albums;

            var listOfAlbums = string.Empty;

            if (albums.Any())
            {
                foreach (var album in albums)
                {
                    var albumHtml = $@"<h1><a id=""albums"" href =""/Albums/Details?id={album.Id}"">{album.Name}</a></h1><br/>" + Environment.NewLine;

                    listOfAlbums += albumHtml;
                }

                this.ViewBag["albumList"] = listOfAlbums;
            }
            else
            {
                this.ViewBag["albumList"] = "There are currently no albums.";
            }

            return this.View();
        }

        [HttpGet("/Albums/Create")]
        public IHttpResponse Create()
        {
            if (!this.IsAuthenticated(this.Request))
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost("/Albums/Create")]
        public IHttpResponse CreatePost(CreatePostAlbumModel model)
        {
            if (this.Context.Albums.Any(x => x.Name == model.AlbumName))
            {
                return new BadRequestResult("Album with the same name already exists.", HttpResponseStatusCode.BadRequest);
            }

            // Create album
            var album = new Album
            {
                Name = model.AlbumName,
                Cover = model.AlbumCover
            };
            this.Context.Albums.Add(album);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            return this.Redirect("/Albums/All");
        }

        [HttpGet("/Albums/Details")]
        public IHttpResponse Details()
        {
            if (!this.IsAuthenticated(this.Request))
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.Request.QueryData.ContainsKey("id"))
            {
                return new BadRequestResult("Must provide a proper album id in query!", HttpResponseStatusCode.BadRequest);
            }

            var albumId = this.Request.QueryData["id"].ToString();

            var album = this.Context.Albums.FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                return new BadRequestResult($"Album with id {albumId} not found!", HttpResponseStatusCode.NotFound);
            }

            this.ViewBag["albumCover"] = album.Cover;
            this.ViewBag["albumName"] = album.Name;
            this.ViewBag["albumId"] = album.Id;

            var listOfTracks = string.Empty;

            decimal albumPrice = 0;

            if (album.Tracks.Any())
            {
                foreach (var track in album.Tracks)
                {
                    var trackId = track.Track.Id;
                    var trackHtml = $@"<li><a class=""text-primary"" href =""/Tracks/Details?albumId={albumId}&trackId={trackId}"">{track.Track.Name}</a></li><br/>" + Environment.NewLine;

                    listOfTracks += trackHtml;

                    albumPrice += track.Track.Price;
                }
                this.ViewBag["trackList"] = listOfTracks;
            }
            else
            {
                this.ViewBag["trackList"] = "There are currently no tracks.";
            }

            this.ViewBag["price"] = albumPrice.ToString("F2");

            return this.View("Details");
        }
    }
}
