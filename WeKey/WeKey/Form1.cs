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
        const string GistUrl = "https://gist.github.com/Altyd/9d822a3525f4e09222e975523ed52fb8";
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

                string[] storedHashes = await FetchHashesFromGist();
                if (storedHashes.Contains(hashedPassword, StringComparer.OrdinalIgnoreCase)) // Case insensitive comparison
                {
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
        static async Task Main(string[] args)
        {

        }

        private static async Task<string[]> FetchHashesFromGist()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(GistUrl);
                return content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(hash => hash.Trim()).ToArray();
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

