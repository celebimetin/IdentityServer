using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        public IActionResult GetPictures()
        {
            var pictureList = new List<Picture>()
            {
                new Picture {Id = 1, Name = "Doğa resim-1", Url = "1.jpg"},
                new Picture {Id = 2, Name = "Doğa resim-2", Url = "2.jpg"}
            };
            return Ok(pictureList);
        }
    }
}