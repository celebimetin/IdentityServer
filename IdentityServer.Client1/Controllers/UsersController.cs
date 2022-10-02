using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync(_configuration["authServerUrl"]);
            if (disco.IsError) { }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = _configuration["ClientResourceOwnerMvc:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["ClientResourceOwnerMvc:ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError) return RedirectToAction();

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken, Value= token.IdentityToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken, Value= token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken, Value= token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn, Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles ="admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles = "editor")]
        public IActionResult EditorAction()
        {
            return View();
        }
    }
}