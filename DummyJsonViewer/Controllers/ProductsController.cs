using DummyJsonViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace DummyJsonViewer.Controllers
{
    public class ProductsController : Controller
    {
        const string BASE_URL = "https://dummyjson.com/products";
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public Products products { get; set; }
        
       

        public ProductsController(ILogger<ProductsController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
       

        public async Task<IActionResult> Index(int? Limit,int? PageNum)
        {
            //Calculate skip
            int limit = Limit ?? 30;
           
            int pageNum = PageNum ?? 0;
            if(pageNum < 0)
            {
                pageNum = 0;
            }

            int totalPages = 0;
            //Figure out total pages and total products
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}?limit=0");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Got milk");
                using var responseStream = await response.Content.ReadAsStreamAsync();

                var Allproducts = await JsonSerializer.DeserializeAsync<Products>(responseStream);
                int TotalProducts = Allproducts.products.Length;
                ViewBag.TotalProducts = TotalProducts;
                totalPages = (int)Math.Round((Double)(TotalProducts / limit));
                ViewBag.TotalPages = totalPages;

            }

            if (totalPages < pageNum)
            {
                pageNum = totalPages;
            }

            message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}?limit={limit}&skip={pageNum*limit}"); 
            message.Headers.Add("Accept", "application/json");

            client = _clientFactory.CreateClient();

            response = await client.SendAsync(message);
            
            if (response.IsSuccessStatusCode)
            {
              
                using var responseStream = await response.Content.ReadAsStreamAsync();
              
                products = await JsonSerializer.DeserializeAsync<Products>(responseStream);

            }
            else
            {
                _logger.LogInformation("Not Succsess");
               
                
                
            }

            


            ViewBag.PageNum = pageNum;

            return View(products);
        }

        public async Task<IActionResult> Detail(string id,int PageNum)
        {
            if (id == null)
                return NotFound();

            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}/{id}");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);

            Product product = null;

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                product = await JsonSerializer.DeserializeAsync<Product>(responseStream);
            }
           

            if (product == null)
                return NotFound();

            ViewBag.PageNum = PageNum;
            return View(product);

        }
    }
}
