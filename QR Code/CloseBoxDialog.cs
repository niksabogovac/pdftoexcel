using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// List of codes to be inserted into database.
        /// </summary>
        private List<string> QRCodes;

        /// <summary>
        /// List of filenumbers to be inserted into database.
        /// </summary>
        private List<string> Filenums;



        private Regex reg = new Regex(@"(RSRFBA)[0-9]{2}\-[0-9]{6}");
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseBoxDialog"/> class.
        /// </summary>
        public CloseBoxDialog()
        {
            InitializeComponent();
            QRCodes = new List<string>();
            Filenums = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseBoxDialog"/> class.
        /// </summary>
        /// <param name="_boxNums">Number of box codes.</param>
        public CloseBoxDialog(int codeNums, string originalBoxCode, List<string> QRCodes)
        {
            this.QRCodes = QRCodes;
            Filenums = new List<string>();
            boxCode = originalBoxCode;
            this.codeNums = codeNums;
            InitializeComponent();
            lText.Text = "Preostali broj kodova za unos: " + codeNums;
        }



        /// <summary>
        /// Writes appropriate box codes with inserted codes.
        /// </summary>
        private void InsertIntoRWTable()
        {

            SqlConnection conn = Helper.GetConnection();

            using (SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[RWTable] VALUES (@boxCode, @code,@qrid)",conn))
            {
                // Counter for QRCodes.
                int k = 0;
                for (int i = 0; i < QRCodes.Count; i++)
                {
                    command.Parameters.AddWithValue("@boxCode", boxCode);
                    command.Parameters.AddWithValue("@qrid", QRCodes[i]);
                    if (i != 0 && i % 20 == 0)
                    {
                        k++;
                    }
                    command.Parameters.AddWithValue("@code", Filenums[k]);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }

        }

       

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            if (reg.IsMatch(tbCode.Text) && tbCode.Text.Length == 15)
            {
                if (!CheckPreviousCodes(tbCode.Text) && !Filenums.Contains(tbCode.Text))
                {
                    Filenums.Add(tbCode.Text);
                    lText.Text = "Preostali broj kodova za unos: " + --codeNums;
                    tbCode.Clear();
                    if (codeNums > 0)
                    {
                        tbCode.Focus();
                    }
                    else
                    {
                        InsertIntoRWTable();
                        this.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Nije unešen ispravan kod ili je već prethodno unešen.");
                }
            }
        }

        /// <summary>
        /// Checks if RWTable code is previously added.
        /// </summary>
        /// <param name="rwCode">Code to be added.</param>
        /// <returns>Indicator if code exists. TRUE - exists or error, FALSE - doesn't</returns>
        private bool CheckPreviousCodes(string rwCode)
        {
            try
            {
                SqlConnection conn = Helper.GetConnection();
                SqlCommand command = new SqlCommand("SELECT [Code] FROM [QRCode].[dbo].[RWTable] WHERE [Code] = @rwCode", conn);
                command.Parameters.AddWithValue("rwCode", rwCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Code already used
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            } 
            catch(Exception e)
            {
                return true;
            }
            
        }
      
    }
}
