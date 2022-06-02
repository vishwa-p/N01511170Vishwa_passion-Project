using N01511170_PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace N01511170_PassionProject.Controllers
{
    public class SongController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SongController()
        {
           /* HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };*/

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44370/api/");
        }

        // GET: Song
        [HttpGet]
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of animals
            //curl https://localhost:44370/api/songdata/listsongs


            string url = "songdata/listsongs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<SongDTO> songs = response.Content.ReadAsAsync<IEnumerable<SongDTO>>().Result;
            //Debug.WriteLine("Number of animals received : ");
            //Debug.WriteLine(animals.Count());


            return View(songs);
        }

        // GET: Song/Details/5
        public ActionResult Details(int id)
        {

            string url = "songdata/findsong/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            

            SongDTO SelectedSong = response.Content.ReadAsAsync<SongDTO>().Result;
            return View(SelectedSong);
        }

        // GET: Song/Create
        [HttpGet]
        public ActionResult New()
        {
            

            return View();
        }

        // POST: Song/Create
        [HttpPost]
        public ActionResult Create(Song song)
        {
            
            try
            {
                string url = "songdata/addsong";
                string jsonpayload = jss.Serialize(song);
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }

                // TODO: Add insert logic here

                /*return RedirectToAction("Index");*/
            }
            catch
            {
                return View();
            }
        }

        // GET: Song/Edit/5
        public ActionResult Edit(int id)
        {
            Song song = new Song();

            //the existing animal information
            string url = "songdata/findsong/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SongDTO SelectedSong = response.Content.ReadAsAsync<SongDTO>().Result;
            
            return View(SelectedSong);
        }

        // POST: Song/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Song song)
        {
            try
            {
                // TODO: Add update logic here
                string url = "songdata/updatesong/" + id;
                string jsonpayload = jss.Serialize(song);
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    //No image upload, but update still successful
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
/*                return RedirectToAction("Index");*/

            }
            catch
            {
                return View();
            }
        }

        // GET: Song/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            Song song = new Song();

            //the existing animal information
            string url = "songdata/findsong/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SongDTO SelectedSong = response.Content.ReadAsAsync<SongDTO>().Result;

            return View(SelectedSong);
        }

        // POST: Song/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                string url = "songdata/deletesong/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
             /*   return RedirectToAction("Index");*/
            }
            catch
            {
                return View();
            }
        }
    }
}
