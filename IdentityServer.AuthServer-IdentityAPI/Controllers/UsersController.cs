using IdentityServer.AuthServer.IdentityAPI.Models;
using IdentityServer.AuthServer_IdentityAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.AuthServer.IdentityAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = userViewModel.UserName;
            user.Email = userViewModel.Email;
            user.City = userViewModel.City;

            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }
            return Ok("üye kayıt edildi.");
        }
    }
}