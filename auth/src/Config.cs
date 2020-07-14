using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthDemo
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope> { new ApiScope("self-catering-api", "Self Catering Scope") };
        
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("self-catering-api", "Self Catering Api")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },
                    
                    Scopes = { "self-catering-api" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "self-catering-client",
                    ClientName = "Self Catering API Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "self-catering-api" },
                }
            };
        }
    }
}
