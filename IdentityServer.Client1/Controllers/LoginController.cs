using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            HttpClient client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["authServerUrl"]);
            if (disco.IsError) { throw new Exception(disco.Error); }

            var passwordTokenRequest = new PasswordTokenRequest();
            passwordTokenRequest.Address = disco.TokenEndpoint;
            passwordTokenRequest.UserName = loginViewModel.Email;
            passwordTokenRequest.Password = loginViewModel.Password;
            passwordTokenRequest.ClientId = _configuration["ClientResourceOwnerMvc:ClientId"];
            passwordTokenRequest.ClientSecret = _configuration["ClientResourceOwnerMvc:ClientSecret"];

            var token = await client.RequestPasswordTokenAsync(passwordTokenRequest);
            if (token.IsError) 
            {
                ModelState.AddModelError("", "");
                throw new Exception(token.Error); 
            }

            var userInfoRequest = new UserInfoRequest();
            userInfoRequest.Token = token.AccessToken;
            userInfoRequest.Address = disco.UserInfoEndpoint;

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);
            if (userInfo.IsError) { throw new Exception(userInfo.Error); }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) }
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return RedirectToAction("Index", "User");
        }
    }
}