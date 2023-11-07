using DSCC_CW1_14973_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DSCC_CW1_14973_MVC.Controllers
{
    public class BookController : Controller
    {

        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // setting base url to fetch data from aws api link

        string BaseUrl = "http://ec2-13-49-225-142.eu-north-1.compute.amazonaws.com/";

        // GET: BookController

        // getting all the books with async
        public async Task<ActionResult> Index()
        {
            List<Book> BookInfo = new List<Book>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Book/GetBooks");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    BookInfo = JsonConvert.DeserializeObject<List<Book>>(PrResponse);
                }

                return View(BookInfo);
            }
        }

        // GET: BookController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Book book = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Book/GetBook/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var authorResponse = response.Content.ReadAsStringAsync().Result;
                    book = JsonConvert.DeserializeObject<Book>(authorResponse);
                }

                return View(book);
            }
        }

        // GET: BookController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the Book object to JSON
                    var authorJson = JsonConvert.SerializeObject(book);
                    var content = new StringContent(authorJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/Book/AddBook", content);

                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");
                    }
                    else
                    {

                        ModelState.AddModelError(string.Empty, "An error occurred during creation.");
                        return View(book);
                    }
                }
            }

            // If the model state is not valid, return the Create view with validation errors
            return View(book);
        }

        // GET: BookController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Book book = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Book/GetBook/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var authorResponse = response.Content.ReadAsStringAsync().Result;
                    book = JsonConvert.DeserializeObject<Book>(authorResponse);

                    return View(book);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Book book)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the author object and send it in the request body for the PUT request
                    var jsonAuthor = JsonConvert.SerializeObject(book);
                    var content = new StringContent(jsonAuthor, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"api/Book/PutBook/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful update, e.g., redirect to the details page
                        return RedirectToAction("Details", new { id });
                    }
                }
            }

            // If model validation fails or the PUT request fails, return to the edit view to show validation errors or handle errors.
            return View("Edit", book);
        }

        // GET: BookController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"api/Book/DeleteBook/{id}");

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
