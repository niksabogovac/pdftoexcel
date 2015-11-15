using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QR_Code
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
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=DMTBJFE;Integrated Security=True");
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[Users] WHERE [Name]= @name", conn);
                command.Parameters.AddWithValue("@name", tbPass.Text);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("Uspesno ste se prijavili.");
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Neuspesno ste se prijavili pokusajte ponovo.");
                    DialogResult = DialogResult.Cancel;

                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Neuspesno ste se prijavili pokusajte ponovo.");
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }
    }
}
