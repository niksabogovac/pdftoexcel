﻿using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    /// <summary>
    /// Form used user identification.
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginForm"/> class.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click on cancel button.
        /// </summary>
        /// <param name="sender">Sending object aka the button itself.</param>
        /// <param name="e">Following arguments.</param>
        private void BCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Handles the click on cancel button.
        /// </summary>
        /// <param name="sender">Sending object aka the button itself.</param>
        /// <param name="e">Following arguments.</param>
        private void BOK_Click(object sender, EventArgs e)
        {
            string errorMsg;
            if (DatabaseManager.CheckIfUserExists(tbName.Text,tbPass.Text,out errorMsg))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Retry;
            }

            Close();
        }
    }
}
