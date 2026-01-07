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

        /* Method used to get a array of products for the index page
        * Limit - How many products should be listed on each page, unused but functionality exists to modify if needed
        * PageNum - The current page number, used to determine which set of products to get
        * 
        * Returns - A Products object which is an array of Product, The page current Page number, number of pages and total products are put in the ViewBag
        */
        public async Task<IActionResult> Index(int? Limit,int? PageNum)
        {
           
            //Again Limit is currently unused, if you want to use it then edit this part
            int limit = Limit ?? 30;

            //Figure out total pages and total products
            int totalPages = 0;
         
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
            else
            {
                //No products have been found
                //No products found return nothing
                return View(new Products());
            }

            //Figure out current Page Number
            int pageNum = PageNum ?? 0;
            if (pageNum < 0)
            {
                pageNum = 0;
            }

            if (totalPages < pageNum)
            {
                pageNum = totalPages;
            }

            //Make request for products to be displayed
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
                //No products found return nothing
                return View(new Products());
            }

            ViewBag.PageNum = pageNum;

            return View(products);
        }

        /*
        * Method used for the detail page to load more details about a certain product
        * id - the id of the Product in question. Primary Key
        * PageNum - the page this product was on, used solely for the go back button and is not changed in this method
        * 
        * Returns - The individual product that was searched for. Pagenum is passed straight back into the viewbag
        */
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
