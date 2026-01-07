using DummyJsonViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace DummyJsonViewer.Controllers
{
    public class ProductsController : Controller
    {
        const string BASE_URL = "https://dummyjson.com";
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _clientFactory;

       
        
        public Products products { get; set; }
        //Remove
        public bool GetStudentsError { get; private set; }

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
            message.RequestUri = new Uri($"{BASE_URL}/products?limit=30&skip={skip}"); 
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
                GetStudentsError = true;
                
                
            }
            
            return View(products);
        }
    }
}
