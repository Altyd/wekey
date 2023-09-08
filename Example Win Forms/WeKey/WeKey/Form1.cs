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

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            wekeyapi manager = new wekeyapi();
            bool isPasswordCorrect = await manager.CheckPasswordAsync(key.Text);
            if (isPasswordCorrect)
            {
                //Whatever you want to happen if the password is correct
                MessageBox.Show("Key is correct.");
            }
            else
            {
                //Whatever you want to happen if the password is incorrect
                MessageBox.Show("Key is incorrect.");
            }
        }
    }
}
