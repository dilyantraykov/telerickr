﻿namespace Telerickr.Services.Controllers
{
    using System.Web.Http;
    using System.Linq;
    using Data;
    using Telerickr.Models;
    using Models;

    public class AlbumsController : ApiController
    {
        private readonly IRepository<Album> albums;
        private readonly IRepository<User> users;

        public AlbumsController()
        {
            var db = new TelerickrDbContext();
            this.albums = new GenericRepository<Album>(db);
            this.users = new GenericRepository<User>(db);
        }

        public IHttpActionResult Get()
        {
            var result = this.albums
                .All()
                .Select(AlbumResponceModel.FromModel)
                .ToList();

            return this.Ok(result);
        }

        public IHttpActionResult Get(int id)
        {
            var result = this.albums
                .All()
                .Where(a => a.Id == id)
                .Select(AlbumResponceModel.FromModel)
                .ToList();

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [Authorize]
        public IHttpActionResult Post(AlbumRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUser = this.users
                .All()
                .FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var newAlbum = new Album
            {
                Title = model.Title,
                User = currentUser
            };

            this.albums.Add(newAlbum);
            this.albums.SaveChanges();

            return this.Ok(newAlbum.Id);
        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            var currentUser = this.users
                .All()
                .FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var result = this.albums
                .All()
                .Where(a => a.Id == id && a.User.UserName == currentUser.UserName)
                .FirstOrDefault();

            if (result == null)
            {
                return this.NotFound();
            }

            this.albums.Delete(result);
            this.albums.SaveChanges();

            return this.Ok("Album deleted");
        }
    }
}
