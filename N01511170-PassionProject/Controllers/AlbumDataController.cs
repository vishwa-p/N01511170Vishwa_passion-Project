using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using N01511170_PassionProject.Models;

namespace N01511170_PassionProject.Controllers
{
    public class AlbumDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all Albums in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Albums in the database
        /// </returns>
        /// <example>
        /// GET: api/AlbumData/ListAlbums
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AlbumDTO))]
        public IHttpActionResult ListAlbums()
        {
            List<Album> Albums = db.Albums.ToList();
            List<AlbumDTO> albumDTOs = new List<AlbumDTO>();

            Albums.ForEach(a => albumDTOs.Add(new AlbumDTO()
            {
                AlbumId = a.AlbumId,
                AlbumName = a.AlbumName,
                Createdby = a.Createdby,
                RelaseDate = a.RelaseDate
                
            }));
            return Ok(albumDTOs);
        }

        /// <summary>
        /// Gathers information about all Albums
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Albums in the database
        /// </returns>
        /// <param name="id">Album ID.</param>
        /// <example>
        /// GET: api/AlbumData/ListSongsForAlbum/3
        /// </example>
        [HttpGet]
       
        public IHttpActionResult ListSongsForAlbum(int id)
        {
            List<Song> Songs = db.Songs.Where(
                k => k.Albums.Any(
                    a => a.AlbumId == id)
                ).ToList();
            List<SongDTO> SongDtos = new List<SongDTO>();

            Songs.ForEach(k => SongDtos.Add(new SongDTO()
            {
                SongId = k.SongId,  
                SongName = k.SongName,
                SingerName = k.SingerName,
                ReleaseDate = k.ReleaseDate,
                Language = k.Language
            }));

            return Ok(SongDtos);
        }

       
        [HttpGet]
       
        public IHttpActionResult ListSongsNotAssignedForAlbum(int id)
        {
            List<Song> Songs = db.Songs.Where(
                   k => !k.Albums.Any(
                       a => a.AlbumId == id)
                   ).ToList();
            List<SongDTO> SongDtos = new List<SongDTO>();

            Songs.ForEach(k => SongDtos.Add(new SongDTO()
            {
                SongId = k.SongId,
                SongName = k.SongName,
                SingerName = k.SingerName,
                ReleaseDate = k.ReleaseDate,
                Language = k.Language
            }));

            return Ok(SongDtos);
        }

        /// <summary>
        /// Associates a particular song with a particular album
        /// </summary>
        /// <param name="albumid">The album ID primary key</param>
        /// <param name="songid">The song ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AlbumData/AssociateSongWithAlbum/9/1
        /// </example>

        [HttpPost]
        [Route("api/AlbumData/AssociateSongWithAlbum/{albumId}/{songId}")]
        /*[Authorize]*/
        public IHttpActionResult AssociateSongWithAlbum(int albumId, int songId)
        {

            Album SelectedAlbum = db.Albums.Include(a => a.Songs).Where(a => a.AlbumId == albumId).FirstOrDefault();
            Song SelectedSong = db.Songs.Find(songId);

            if (SelectedAlbum == null || SelectedSong == null)
            {
                return NotFound();
            }


            SelectedAlbum.Songs.Add(SelectedSong);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular song and a particular album
        /// </summary>
        /// <param name="songid">The song ID primary key</param>
        /// <param name="albumid">The album ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AlbumData/UnassociateSongWithAlbum/9/1
        /// </example>
        [HttpPost]
        [Route("api/AlbumData/UnassociateSongWithAlbum/{albumId}/{songId}")]
        /*[Authorize]*/
        public IHttpActionResult UnassociateSongWithAlbum(int albumId, int songId)
        {

            Album SelectedAlbum = db.Albums.Include(a => a.Songs).Where(a => a.AlbumId == albumId).FirstOrDefault();
            Song SelectedSong = db.Songs.Find(songId);

            if (SelectedAlbum == null || SelectedSong == null)
            {
                return NotFound();
            }


            SelectedAlbum.Songs.Remove(SelectedSong);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns all albums in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An album in the system matching up to the song ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the album</param>
        /// <example>
        /// GET: api/albumData/FindAlbum/5
        /// </example>
        // GET: api/AlbumData/5
        [ResponseType(typeof(Album))]
        [HttpGet]
        public IHttpActionResult FindAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            
            AlbumDTO albumDTO = new AlbumDTO()
            {
                AlbumId = album.AlbumId,
                AlbumName = album.AlbumName,
                Createdby = album.Createdby,
                RelaseDate = album.RelaseDate
               
            };
            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        /// <summary>
        /// Updates a particular album in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the album ID primary key</param>
        /// <param name="album">JSON FORM DATA of an album</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/AlbumData/UpdateAlbum/5
        /// FORM DATA: album JSON Object
        /// </example>
        // PUT: api/AlbumData/5
        [ResponseType(typeof(void))]
        [HttpPost]
       /* [Authorize]*/
        public IHttpActionResult UpdateAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            db.Entry(album).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an album to the system
        /// </summary>
        /// <param name="album">JSON FORM DATA of an album</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: album ID, album Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AlbumData/AddAlbum
        /// FORM DATA: Album JSON Object
        /// </example>

        // POST: api/AlbumData
        [ResponseType(typeof(Album))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult AddAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Albums.Add(album);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumId }, album);
        }


        /// <summary>
        /// Deletes an album from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the album</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/AlbumData/DeleteAlbum/5
        /// FORM DATA: (empty)
        /// </example>
        // DELETE: api/AlbumData/5
        [ResponseType(typeof(Album))]
        [HttpPost]
       /* [Authorize]*/
        public IHttpActionResult DeleteAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            db.Albums.Remove(album);
            db.SaveChanges();

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return db.Albums.Count(e => e.AlbumId == id) > 0;
        }
    }
}