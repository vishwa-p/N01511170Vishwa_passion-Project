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
    public class AlbumController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AlbumController()
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

        // GET: Album
        [HttpGet]
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of animals
            //curl https://localhost:44370/api/albumdata/listalbums


            string url = "albumdata/listalbums";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AlbumDTO> albums = response.Content.ReadAsAsync<IEnumerable<AlbumDTO>>().Result;
            //Debug.WriteLine("Number of animals received : ");
            //Debug.WriteLine(animals.Count());


            return View(albums);
        }

        // GET: album/Details/5
        public ActionResult Details(int id)
        {

            string url = "albumdata/findalbum/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;



            AlbumDTO SelectedAlbum = response.Content.ReadAsAsync<AlbumDTO>().Result;
            return View(SelectedAlbum);
        }
        [HttpPost]
        /*[Authorize]*/
        public ActionResult AssociateSong(int id, int songID)
        {
           /* GetApplicationCookie();//get token credentials*/
            

            //call our api to associate animal with keeper
            string url = "albumdata/AssociateSongWithAlbum/" + id + "/" + songID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Edit/" + id);
        }

        [HttpGet]
       /* [Authorize]*/
        public ActionResult UnAssociateSong(int id, int songID)
        {
         

            //call our api to unassociate animal with keeper
            string url = "albumdata/UnassociateSongWithAlbum/" + id + "/" + songID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Edit/" + id);
        }

        // GET: album/Create
        [HttpGet]
        public ActionResult New()
        {


            return View();
        }

        // POST: album/Create
        [HttpPost]
        public ActionResult Create(Album album)
        {

            try
            {
                string url = "albumdata/addalbum";
                string jsonpayload = jss.Serialize(album);
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

        // GET: album/Edit/5
        public ActionResult Edit(int id)
        {
            AlbumViewModel albumviewmodel=new AlbumViewModel();

            //the existing animal information
            string url = "albumdata/findalbum/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AlbumDTO SelectedAlbum = response.Content.ReadAsAsync<AlbumDTO>().Result;
           

            albumviewmodel.AlbumDto = SelectedAlbum;

            //show associated keepers with this animal
            url = "albumdata/ListSongsForAlbum/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SongDTO> AssignedSongs = response.Content.ReadAsAsync<IEnumerable<SongDTO>>().Result;

            albumviewmodel.AlreadyAdded = AssignedSongs;

            url = "albumdata/ListSongsNotAssignedForAlbum/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SongDTO> NotAssignedSongs = response.Content.ReadAsAsync<IEnumerable<SongDTO>>().Result;

            albumviewmodel.AvailableSongs = NotAssignedSongs;


            return View(albumviewmodel);

            
        }

        // POST: album/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Album album)
        {
            try
            {

                // TODO: Add update logic here
                string url = "albumdata/updatealbum/" + id;
                string jsonpayload = jss.Serialize(album);
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

        // GET: album/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            Song song = new Song();

            //the existing animal information
            string url = "albumdata/findalbum/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AlbumDTO SelectedAlbum = response.Content.ReadAsAsync<AlbumDTO>().Result;

            return View(SelectedAlbum);
        }

        // POST: album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                string url = "albumdata/deletealbum/" + id;
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
