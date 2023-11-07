using DSCC_CW1_14973_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DSCC_CW1_14973_MVC.Controllers
{
    public class AuthorController : Controller
    {

        private readonly IConfiguration _configuration;

        public AuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // setting base url to fetch data from aws api link

        string BaseUrl = "http://ec2-13-49-225-142.eu-north-1.compute.amazonaws.com/";

        // GET: AuthorController

        // getting all the books with async
        public async Task<ActionResult> Index()
        {
            List<Author> AuthorInfo = new List<Author>();
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Author/GetAuthors");

                if(Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    AuthorInfo = JsonConvert.DeserializeObject<List<Author>>(PrResponse);
                }

                return View(AuthorInfo);
            }
           
        }

        // GET: AuthorController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Author author = null;
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Author/GetAuthor/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var authorResponse = response.Content.ReadAsStringAsync().Result;
                    author = JsonConvert.DeserializeObject<Author>(authorResponse);
                }

                return View(author);
            }
        }

        // GET: AuthorController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the Author object to JSON
                    var authorJson = JsonConvert.SerializeObject(author);
                    var content = new StringContent(authorJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/Author/PostAuthor", content);

                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");
                    }
                    else
                    {

                        ModelState.AddModelError(string.Empty, "An error occurred during creation.");
                        return View(author);
                    }
                }
            }

            // If the model state is not valid, return the Create view with validation errors
            return View(author);
        }

        // GET: AuthorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Author author = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Author/GetAuthor/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var authorResponse = response.Content.ReadAsStringAsync().Result;
                    author = JsonConvert.DeserializeObject<Author>(authorResponse);

                    return View(author);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Author author)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the author object and send it in the request body for the PUT request
                    var jsonAuthor = JsonConvert.SerializeObject(author);
                    var content = new StringContent(jsonAuthor, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"api/Author/PutAuthor/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful update, e.g., redirect to the details page
                        return RedirectToAction("Details", new { id });
                    }
                }
            }

            // If model validation fails or the PUT request fails, return to the edit view to show validation errors or handle errors.
            return View("Edit", author);
        }

        // GET: AuthorController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }


        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"api/Author/DeleteAuthor/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            
           
        }
    }
}
