using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates; // Added
using System.Net.Security; // Added

namespace WeKey
{
    public class wekeyapi
    {
        // URL of the raw Github Gist containing password hashes
        private const string GistUrl = "https://gist.githubusercontent.com/Altyd/9d822a3525f4e09222e975523ed52fb8/raw/6776a72535d6b927783c8f6150a478ba400a90e7/";
        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = CustomCertificateValidation
        };
        // HttpClient is intended to be instantiated once and re-used throughout the life of an application
        private static readonly HttpClient _httpClient = new HttpClient(_handler);

        /// <summary>
        /// Checks if the provided password matches any of the hashed passwords stored in the Gist.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <returns>True if the password's hash is found in the Gist, otherwise false.</returns>
        public async Task<bool> CheckPasswordAsync(string password)
        {
            try
            {

                string hashedPassword = ComputeSha256Hash(password);
                string[] storedHashes = await FetchHashesFromGist();

                return storedHashes.Contains(hashedPassword);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches the list of password hashes from the Github Gist.
        /// </summary>
        /// <returns>An array of password hashes.</returns>
        private static async Task<string[]> FetchHashesFromGist()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(GistUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch Github content. HTTP Status: {response.StatusCode}");
            }

            string content = await response.Content.ReadAsStringAsync();

            // Splitting by new line and carriage return, then trimming and removing any empty lines
            return content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(hash => hash.Trim())
                          .Where(hash => !string.IsNullOrEmpty(hash))
                          .ToArray();
        }

        /// <summary>
        /// Computes the SHA256 hash of the provided data.
        /// </summary>
        /// <param name="rawData">The string to hash.</param>
        /// <returns>The SHA256 hash.</returns>
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        private static bool CustomCertificateValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            // Here, you compare the actual certificate's thumbprint with your expected thumbprint
            const string knownGitHubThumbprint = "B6 17 4D 4C 3E C5 AE 76 86 10 AD 70 55 44 B4 60 0E 51 4A 55 71 8A 94 C6 36 56 3C BA A4 DD 66 4C";

            return certificate.Thumbprint == knownGitHubThumbprint;
        }
    }
}
