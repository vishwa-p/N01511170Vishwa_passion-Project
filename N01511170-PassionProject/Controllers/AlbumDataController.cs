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