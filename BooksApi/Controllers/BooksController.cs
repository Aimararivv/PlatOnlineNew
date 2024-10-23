using BooksApi.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BooksApi.Controllers
{
    public class BooksController : Controller
    {
        private static readonly string apiUrl = "https://www.googleapis.com/books/v1/volumes?q=";
        private static readonly string apiKey = "";

        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Error = "Debe ingresar un termino de busqueda.";
                return View("Index");
            }
            var books = await SearchBooksAsync(query);
            return View("Index", books);
        }
        public async Task<GoogleBooksResponse> SearchBooksAsync(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"{apiUrl}{query}&key={apiKey}");
                    if (response.IsSuccessStatusCode)
                    {
                        // Usar Newtonsoft.Json para deserializar el contenido JSON
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<GoogleBooksResponse>(jsonResponse);
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo obtener los libros. Intentalo nuevamente.";
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al realizar la solicitud: {ex.Message}";
                    return null;
                }
            }
        }
    }
}