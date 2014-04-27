using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITTT_Final
{
    public partial class Form2 : Form
    {
        Bitmap picture;
        public Form2()
        {
            InitializeComponent();
        }
        public void PrepareForm(string fileName, string msg)
        {
            picture = new Bitmap(fileName);
            richTextBox1.Text = msg;
            if (msg.Contains("Miasto"))
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.Image = (Image)picture;
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            picture.Dispose();
        }
    }
}
