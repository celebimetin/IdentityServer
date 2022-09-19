using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();
            HttpClient httpClient = new HttpClient();

            var discovory = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovory.IsError) { }

            ClientCredentialsTokenRequest clientCredentials = new ClientCredentialsTokenRequest();
            clientCredentials.ClientId = _configuration["Client:ClientId"];
            clientCredentials.ClientSecret = _configuration["Client:ClientSecret"];
            clientCredentials.Address = discovory.TokenEndpoint;
            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentials);

            if (token.IsError) { }

            httpClient.SetBearerToken(token.AccessToken);
            var response = await httpClient.GetAsync("https://localhost:5007/api/products/getproducts");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            return View(products);
        }
    }
}