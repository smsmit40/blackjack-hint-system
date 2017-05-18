/* BLACKJACK V5
 * MAY 2017
 * Copyright (C) Gerardo Vela (DEYOX). All rights reserved
 * 
 * Program CLASS:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BJ5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new form1());
            Application.Run(new main());
        }
    }
}
