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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[Users] WHERE [JMBG]=" + tbPass.Text,conn);

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
            catch(Exception o)
            {
                MessageBox.Show("Neuspesno ste se prijavili pokusajte ponovo.");
                DialogResult = DialogResult.Cancel;
            }   
            
            Close();
        }
    }
}
