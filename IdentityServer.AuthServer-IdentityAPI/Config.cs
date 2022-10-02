using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer.AuthServer_IdentityAPI
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" },
                    ApiSecrets = new[] {new Secret("secretapi1".Sha256())}
                },
                new ApiResource("resource_api2")
                {
                    Scopes = { "api2.read", "api2.write" , "api2.update" },
                    ApiSecrets = new[] {new Secret("secretapi2".Sha256())}
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read", "API.1 için okuma izni"),
                new ApiScope("api1.write", "API.1 için yazma izni"),
                new ApiScope("api1.update", "API.1 için güncelleme izni"),

                new ApiScope("api2.read", "API.2 için okuma izni"),
                new ApiScope("api2.write", "API.2 için yazma izni"),
                new ApiScope("api2.update", "API.2 için güncelleme izni"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
                {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName = "Client 1",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read" }
                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client 2",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.update", "api2.read", "api2.write", "api2.update" }
                },
                new Client()
                {
                    ClientId = "Client1-Mvc",
                    RequirePkce = false,
                    ClientName = "Client1-Mvc",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = new List<string>{"https://localhost:5003/signout-callback-oidc"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    AccessTokenLifetime = (int)(DateTime.Now.AddHours(2) - DateTime.Now).TotalSeconds,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddHours(60) - DateTime.Now).TotalSeconds,
                    RequireConsent = false
                },
                new Client()
                {
                    ClientId = "Client2-Mvc",
                    RequirePkce = false,
                    ClientName = "Client2-Mvc",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:5005/signin-oidc" },
                    PostLogoutRedirectUris = new List<string>{"https://localhost:5005/signout-callback-oidc"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    AccessTokenLifetime = (int)(DateTime.Now.AddHours(2) - DateTime.Now).TotalSeconds,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddHours(60) - DateTime.Now).TotalSeconds,
                    RequireConsent = false
                },
                new Client()
                {
                    ClientId = "js-client",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientName = "Js Client(Angular)",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    RedirectUris = {"http://localhost:4200/callback"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200"}
                },
                new Client()
                {
                    ClientId = "Client1-ResourceOwner-Mvc",
                    RequirePkce = false,
                    ClientName = "Client1-ResourceOwner-Mvc",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    AccessTokenLifetime = (int)(DateTime.Now.AddHours(2) - DateTime.Now).TotalSeconds,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddHours(60) - DateTime.Now).TotalSeconds,
                },
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource()
                {
                    Name="CountryAndCity", DisplayName="Country And City", Description="Country and city the users",
                    UserClaims=new[]{"country","city"}
                },
                new IdentityResource()
                {
                    Name="Roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new[]{"role"}
                }
            };
        }
    }
}