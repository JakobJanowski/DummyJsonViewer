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
        //Remove
       

        public ProductsController(ILogger<ProductsController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
       

        public async Task<IActionResult> Index(int?Skip)
        {
            //Calculate skip
           
            int? skip = Skip ?? 0;
            if(skip < 0)
            {
                skip = 0;
            }
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}?limit=30&skip={skip}"); 
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Got milk");
                using var responseStream = await response.Content.ReadAsStreamAsync();
              
                products = await JsonSerializer.DeserializeAsync<Products>(responseStream);
                products.skipped = skip;
                
                
                

            }
            else
            {
                _logger.LogInformation("Not Succsess");
               
                
                
            }
            
            return View(products);
        }

        public async Task<IActionResult> Detail(string id,int skip)
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

            Products finalproducts = new Products();
            finalproducts.products = new Product[1] { product };
            finalproducts.skipped = skip;
            return View(finalproducts);

        }
    }
}
