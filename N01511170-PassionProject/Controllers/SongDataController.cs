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

        /// <summary>
        /// Returns all songs in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all songs in the database
        /// </returns>
        /// <example>
        /// GET: api/SongData/ListSongs
        /// </example>
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


        /// <summary>
        /// Returns all Songs in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Song in the system 
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the song</param>
        /// <example>
        /// GET: api/songData/FindSong/5
        /// </example>
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

        /// <summary>
        /// Updates a particular song in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the song ID primary key</param>
        /// <param name="song">JSON FORM DATA of a song</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SongData/UpdateSong/5
        /// FORM DATA: song JSON Object
        /// </example>

        // PUT: api/SongData/5
        [ResponseType(typeof(void))]
        [HttpPost]
    /*    [Authorize]*/
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

        /// <summary>
        /// Adds a song to the system
        /// </summary>
        /// <param name="song">JSON FORM DATA of  song</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: song ID, song Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/songData/AddSong
        /// FORM DATA: song JSON Object
        /// </example>
        // POST: api/SongData
        [ResponseType(typeof(Song))]
        [HttpPost]
       /* [Authorize]*/
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

        /// <summary>
        /// Deletes a song from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the song</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SongData/DeleteSong/5
        /// FORM DATA: (empty)
        /// </example>
        // DELETE: api/SongData/5
        [ResponseType(typeof(Song))]
        [HttpPost]
    /*    [Authorize]*/
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