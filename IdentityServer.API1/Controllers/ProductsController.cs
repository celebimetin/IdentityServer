using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product {Id = 1, Name = "kalem-1", Price = 10, Stock = 100},
                new Product {Id = 2, Name = "kalem-2", Price = 10, Stock = 100},
                new Product {Id = 3, Name = "kalem-3", Price = 10, Stock = 100},
            };
            return Ok(productList);
        }

        [Authorize(Policy = "CreateOrUpdate")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }

        [Authorize(Policy = "CreateOrUpdate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"Id'si {id} olan ürün güncellendi");
        }
    }
}