/* BLACKJACK V5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * 
 * RULESFORM CLASS:
 *  - Loads the rules and how to play rules.
 *  - When the player cliks on X label, the form closes.
 */
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
    public partial class Rules : Form
    {
        System.Media.SoundPlayer close_form = new System.Media.SoundPlayer("Resources/close.wav"); // shuffle music

        public Rules()
        {
            InitializeComponent();
        }

        private void Rules_Load(object sender, EventArgs e)
        {

        }
        /* Closes the form if user selects close label */
        private void closeBox_Click(object sender, EventArgs e)
        {
            close_form.Play();
            // closes rules form and frees memory
            this.Dispose(); 
        }
        /* Closes the form if user selects close button */
        private void closeForm(object sender, FormClosedEventArgs e)
        {
            close_form.Play();
            // closes rules form and frees memory
            this.Dispose(); 
        }
    }
}
