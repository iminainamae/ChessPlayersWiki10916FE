using ChessPlayersWiki10916FE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ChessPlayersWiki10916FE.Controllers
{
    public class PlayerController : Controller
    {
        //providing a link to AWS EC2 instance 
        public const string url = "http://ec2-16-171-232-143.eu-north-1.compute.amazonaws.com/";
        //representing it as a web url       
        public Uri address = new Uri(url);
        //performing http requests
        HttpClient client;

        //constructor
        public PlayerController()
        {
            //performing http requests
            client = new HttpClient();
            //binding the link
            client.BaseAddress = address;
        }

        //managing request headers
        public void HeaderClearing()
        {
            //clearing all headers
            client.DefaultRequestHeaders.Clear();
            //setting to accept json data
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: PlayerController
        public async Task<ActionResult> Index()
        {
            //taking the whole body of players
            List<Player> player = new List<Player>();
            //clearing request headers
            HeaderClearing();
            //sending get request
            HttpResponseMessage response = await client.GetAsync("api/Player");

            if (response.IsSuccessStatusCode)
            {
                //reading request response as string
                var res = response.Content.ReadAsStringAsync().Result;
                //convert request response to json
                player = JsonConvert.DeserializeObject<List<Player>>(res);
            }
            //returning the whole body of players
            return View(player);
        }

        // GET: PlayerController/Details/5
        public ActionResult Details(int id)
        {
            //creating player instance
            Player player = new Player();
            //clearing request headers
            HeaderClearing();
            //sending get by id request by the base link with specific id
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Player/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                //reading request response as string
                string res = response.Content.ReadAsStringAsync().Result;
                //convert request response to json
                player = JsonConvert.DeserializeObject<Player>(res);
            }
            //returning the info of the player with the id
            return View(player);
        }

        // GET: PlayerController/Create
        public ActionResult Create()
        {
            //opening the create form
            return View();
        }

        // POST: PlayerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                //converting the data in the form to json
                string pl = JsonConvert.SerializeObject(player);
                //setting the json as the body of request
                StringContent content = new StringContent(pl, Encoding.UTF8, "application/json");
                //sending a post request
                HttpResponseMessage createHttpResponseMessage = client.PostAsync(client.BaseAddress + "api/Player/", content).Result;
                if (createHttpResponseMessage.IsSuccessStatusCode)
                {
                    //returning to the main page
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(player);
        }

        // GET: PlayerController/Edit/5
        public ActionResult Edit(int id)
        {
            //taking the whole body of players
            Player player = new Player();
            //sending get by id request by the base link with specific
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Player/" + id).Result;
            
            if (response.IsSuccessStatusCode)
            {
                //reading request response as string
                string details = response.Content.ReadAsStringAsync().Result;
                //setting to accept json data
                player = JsonConvert.DeserializeObject<Player>(details);
            }
            //returning the edit form
            return View(player);
        }

        // POST: PlayerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player player, int id)
        {
            //set the original id
            player.playerId = id;

            if (ModelState.IsValid)
            {
                //converting the data in the form to json
                string pl = JsonConvert.SerializeObject(player);
                //setting the json as the body of request
                StringContent content = new StringContent(pl, Encoding.UTF8, "application/json");
                //sending edit request by the base link with specific
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "api/Player/" + player.playerId, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            //returning to the main page
            return View(player);
        }

        //GET: PlayerController/Delete/5
        public ActionResult Delete(int id)
        {
            Player pl = new Player();

            HeaderClearing();
            //sending get by id request by the base link with specific id
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Player/" + id).Result;
            //reading request response as string
            string details = response.Content.ReadAsStringAsync().Result;
            //convert request response to json
            pl = JsonConvert.DeserializeObject<Player>(details);
            //displaying the delete form
            return View(pl);
        }

        // POST: PlayerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Player player, int id)
        {
            //converting the data in the form to json
            string pl = JsonConvert.SerializeObject(id);
            //setting the json as the body of request
            StringContent content = new StringContent(pl, Encoding.UTF8, "application/json");
            //sending delete request by the base link with specific id
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "api/Player/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            //returning to the main page
            return View(id);
        }
    }
}
