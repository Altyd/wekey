using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Security.Cryptography;

namespace WeKey
{
    public partial class Form1 : Form
    {
        const string PastebinUrl = "https://gist.githubusercontent.com/Altyd/9d822a3525f4e09222e975523ed52fb8/raw/6776a72535d6b927783c8f6150a478ba400a90e7/"; 

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string password = key.Text;

                string hashedPassword = ComputeSha256Hash(password);
                MessageBox.Show($"Computed Hash: {hashedPassword}"); // Debug output

                string[] storedHashes = await FetchHashesFromPastebin();
                MessageBox.Show($"Fetched Hashes from Github:\n\n{string.Join("\n", storedHashes)}"); // Display the fetched hashes

                if (storedHashes.Contains(hashedPassword, StringComparer.OrdinalIgnoreCase)) // Case insensitive comparison
                {
                    MessageBox.Show($"Fetched Hashes from Github:\n\n{string.Join("\n", storedHashes)}"); // Display the fetched hashes
                    MessageBox.Show("Password is correct.");
                }
                else
                {
                    MessageBox.Show("Password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private static async Task<string[]> FetchHashesFromPastebin()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(PastebinUrl);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Failed to fetch Github content. HTTP Status: {response.StatusCode}");
                    return new string[0];
                }

                string content = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Raw Content from Github:\n\n{content}"); // Display raw content

                return content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(hash => hash.Trim())
                              .Where(hash => !string.IsNullOrEmpty(hash))
                              .ToArray();
            }
        }

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
    }
}
