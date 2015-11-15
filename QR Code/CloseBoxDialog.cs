using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    public partial class CloseBoxDialog : Form
    {
        
        /// <summary>
        /// Represents the number of codes needed to be inserted. 
        /// </summary>
        private int codeNums;

        /// <summary>
        /// Box code of box being closed.
        /// </summary>
        private string boxCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseBoxDialog"/> class.
        /// </summary>
        public CloseBoxDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseBoxDialog"/> class.
        /// </summary>
        /// <param name="_boxNums">Number of box codes.</param>
        public CloseBoxDialog(int codeNums, string originalBoxCode)
        {
            boxCode = originalBoxCode;
            this.codeNums = codeNums;
            InitializeComponent();
            lText.Text = "Preostali broj kodova za unos: " + codeNums;
        }

        /// <summary>
        /// Triggered when confirm button is clicked.
        /// </summary>
        /// <param name="sender">Button being pressed.</param>
        /// <param name="e">Following arguments.</param>
        private void BConfirmClick(object sender, EventArgs e)
        {
            try
            {
                InsertIntoRWTable(tbCode.Text);
                lText.Text = "Preostali broj kodova za unos: " + --codeNums;
                tbCode.Clear();
                if (codeNums > 0)
                {
                    tbCode.Focus();
                }
                else
                {
                    this.Dispose();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Nije unešen ispravan kod ili je već prethodno unešen.");
            }
            
        }

        /// <summary>
        /// Writes appropriate box codes with inserted codes.
        /// </summary>
        private void InsertIntoRWTable(string code)
        {
            SqlConnection conn = new SqlConnection("Data Source=DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[RWTable] VALUES (@boxCode, @code)", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
        }

      
    }
}
