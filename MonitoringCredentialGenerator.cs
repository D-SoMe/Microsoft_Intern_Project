using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using Azure.Core;
using System.Threading;

namespace Microsoft.Cosmic.Monitoring.Library
{
    public class MonitoringCredentialGenerator : IMonitoringCredentialGenerator
    {
        public async Task<TokenCredential> GetToken(string ring, string managedIdentityId)
        {
            if (!DataRingConfigurations.DataRingConfigs.TryGetValue(ring, out DataRing data))
            {
                throw new KeyNotFoundException($"The specified ring '{ring}' was not found in the configurations.");
            }
            var resourceIdentifier = new ResourceIdentifier(managedIdentityId);
            ManagedIdentityCredential credentials = new ManagedIdentityCredential();
            ClientAssertionCredential tokenCredential = null;
            try
            {
                KeyVaultCertificateWithPolicy certificate = await FetchCertificate(data.KeyvaultUrl, data.CertificateName, credentials);
                X509Certificate2 x509Certificate = new X509Certificate2(certificate.Cer);

                var jwtToken = CreateJwtToken(data.AzureAdAudienceClaimUrl, data.ClientId, x509Certificate);
                var signedToken = await SignJwtToken(jwtToken, certificate.KeyId, credentials);

                Task<string> AssertionCallback(CancellationToken cancellationToken)
                {
                    return Task.FromResult(signedToken);
                }

                    tokenCredential = new ClientAssertionCredential(
                    data.TenantId,
                    data.ClientId,
                    AssertionCallback,
                    new ClientAssertionCredentialOptions
                    {
                        AuthorityHost = new Uri(data.AuthenticationEndpoint)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception from Library 1 {ring} -- {managedIdentityId}");
            }

            return tokenCredential;
        }

        private async Task<KeyVaultCertificateWithPolicy> FetchCertificate(string keyVaultUrl, string certificateName, ManagedIdentityCredential credentials)
        {
            
            var certificateClient = new CertificateClient(new Uri(keyVaultUrl), credentials);
         
            return await certificateClient.GetCertificateAsync(certificateName);
        }

        private JwtSecurityToken CreateJwtToken(string azureAdAudienceClaimUrl, string clientId, X509Certificate2 x509Certificate)
        {
            var header = new JwtHeader
            {
                { "alg", SecurityAlgorithms.RsaSha256 },
                { "typ", "JWT" },
                { "x5t", Base64UrlEncoder.Encode(x509Certificate.GetCertHash()) }
            };

            var x5cList = new List<string> { Convert.ToBase64String(x509Certificate.Export(X509ContentType.Cert)) };
            header["x5c"] = x5cList;

            var now = DateTime.UtcNow;
            var payload = new JwtPayload
            {
                { "aud", azureAdAudienceClaimUrl },
                { "iss", clientId },
                { "sub", clientId },
                { "jti", Guid.NewGuid().ToString() },
                { "nbf", new DateTimeOffset(now).ToUnixTimeSeconds() },
                { "exp", new DateTimeOffset(now.AddMinutes(10)).ToUnixTimeSeconds() }
            };

            return new JwtSecurityToken(header, payload);
        }

        private async Task<string> SignJwtToken(JwtSecurityToken jwtToken, Uri keyId, TokenCredential credentials)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var unsignedToken = tokenHandler.WriteToken(jwtToken).Split('.')[0] + "." + tokenHandler.WriteToken(jwtToken).Split('.')[1];

            var digest = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

            var keyClient = new CryptographyClient(keyId, credentials);
            var signResult = await keyClient.SignAsync(SignatureAlgorithm.RS256, digest);

            return $"{unsignedToken}.{Base64UrlEncoder.Encode(signResult.Signature)}";
        }
    }
}








