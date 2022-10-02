﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.AuthServer
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
                }
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
                new ApiScope("api2.update", "API.2 için güncelleme izni")
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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
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
        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser{SubjectId="1", Username="mcelebi", Password="password",
                    Claims= new List<Claim>()
                    {
                        new Claim("given_name", "Metin"),
                        new Claim("family_name", "Çelebi"),
                        new Claim("country", "Türkiye"),
                        new Claim("city", "İstanbul"),
                        new Claim("role", "admin")
                    }},
                new TestUser{SubjectId="2", Username="ccelebi", Password="password",
                    Claims= new List<Claim>()
                    {
                        new Claim("given_name", "Çetin"),
                        new Claim("family_name", "Çelebi"),
                        new Claim("country", "Türkiye"),
                        new Claim("city", "Samsun"),
                        new Claim("role", "editor")
                    }}
            };
        }
    }
}