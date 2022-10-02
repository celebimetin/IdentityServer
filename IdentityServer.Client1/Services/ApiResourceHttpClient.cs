using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _client;

        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            _client.SetBearerToken(accessToken);

            return _client;
        }

        public async Task<List<string>> UserSignUpViewModel(SignUpViewModel signUpViewModel)
        {
            var disco = await _client.GetDiscoveryDocumentAsync(_configuration["authServerUrl"]);
            if (disco.IsError) { throw new Exception(disco.Error); }

            var clientCredentials = new ClientCredentialsTokenRequest();
            clientCredentials.ClientId = _configuration["ClientResourceOwnerMvc:ClientId"];
            clientCredentials.ClientSecret = _configuration["ClientResourceOwnerMvc:ClientSecret"];
            clientCredentials.Address = disco.TokenEndpoint;

            var token = await _client.RequestClientCredentialsTokenAsync(clientCredentials);
            if (token.IsError) { throw new Exception(token.Error); }

            var stringContent = new StringContent(JsonConvert.SerializeObject(signUpViewModel), Encoding.UTF8, "application/json");
            _client.SetBearerToken(token.AccessToken);
            var httpResponse = await _client.PostAsync("https://localhost:5001/api/users/signup", stringContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorList = JsonConvert.DeserializeObject<List<string>>(await httpResponse.Content.ReadAsStringAsync());
                return errorList;
            }
            return null;
        }
    }
}