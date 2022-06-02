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
    public class SongDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SongData
        [HttpGet]
        [ResponseType(typeof(SongDTO))]
        public IHttpActionResult ListSongs()
        {
            List<Song> Songs = db.Songs.ToList();
            List<SongDTO> SongDTOs = new List<SongDTO>();

            Songs.ForEach(a => SongDTOs.Add(new SongDTO()
            {
                SongId = a.SongId,
                SongName = a.SongName,
                SingerName = a.SingerName,
                ReleaseDate = a.ReleaseDate,
                Language=a.Language
            }));
            return Ok(SongDTOs);
        }



        // GET: api/SongData/5
        [ResponseType(typeof(SongDTO))]
        [HttpGet]
       
        public IHttpActionResult FindSong(int id)
        {
            Song song = db.Songs.Find(id);
            SongDTO SongDTO=new SongDTO()
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SingerName = song.SingerName,
                ReleaseDate = song.ReleaseDate,
                Language = song.Language
                
            };
            if (song == null)
            {
                return NotFound();
            }

            return Ok(SongDTO);
        }

        // PUT: api/SongData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult UpdateSong(int id, Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != song.SongId)
            {
                return BadRequest();
            }

            db.Entry(song).State = EntityState.Modified;
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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

        // POST: api/SongData
        [ResponseType(typeof(Song))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult AddSong(Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Songs.Add(song);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = song.SongId }, song);
        }

        // DELETE: api/SongData/5
        [ResponseType(typeof(Song))]
        [HttpPost]
        /*[Authorize]*/
        public IHttpActionResult DeleteSong(int id)
        {
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return NotFound();
            }

            db.Songs.Remove(song);
            db.SaveChanges();

            return Ok(song);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SongExists(int id)
        {
            return db.Songs.Count(e => e.SongId == id) > 0;
        }
    }
}