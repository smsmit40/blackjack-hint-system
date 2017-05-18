using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BJ5
{
    public partial class form1 : Form
    {
        System.Media.SoundPlayer intro = new System.Media.SoundPlayer(); // intro music
        public form1()
        {
            InitializeComponent();
            intro.SoundLocation = "Resources/oo_intro.wav";
            intro.Play();
        }

        private void form1_Load(object sender, EventArgs e)
        {
            //makes image equals to the size of the window
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = this.BackgroundImage.Width;
            this.Height = this.BackgroundImage.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            this.Close();
        }
    }
}
