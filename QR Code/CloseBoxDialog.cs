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

        /// <summary>
        /// Filenumbers that are already in database.
        /// </summary>
        private List<string> PreviousFilenums;

        private Regex reg = new Regex(@"([A-Z]){6}[0-9]{2}\-[0-9]{6}");
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseBoxDialog"/> class.
        /// </summary>
        public CloseBoxDialog()
        {
            InitializeComponent();
            QRCodes = new List<string>();
            Filenums = new List<string>();
            PreviousFilenums = LoadPreviousFilenums();
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
            PreviousFilenums = LoadPreviousFilenums();
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
            if (!PreviousFilenums.Contains(tbCode.Text) && !Filenums.Contains(tbCode.Text))
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

            /*try
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
            }*/
            
        }

        /// <summary>
        /// Writes appropriate box codes with inserted codes.
        /// </summary>
        private void InsertIntoRWTable()
        {

            SqlConnection conn = new SqlConnection(Helper.ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[RWTable] VALUES (@boxCode, @code,@qrid)", conn);
            //command.Parameters.AddWithValue("@boxCode", boxCode);
            // Counter for QRCodes.
            int k = 0;
            for(int i = 0; i < QRCodes.Count;i++)
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
            conn.Close();

            /*
            SqlConnection conn = new SqlConnection(Helper.ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[RWTable] VALUES (@boxCode, @code)", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
            conn.Close();*/
        }

        /// <summary>
        /// Loads previous codes(filenums) from database to prevent inserting equal codes.
        /// </summary>
        /// <returns>Previous filenums.</returns>
        private List<string> LoadPreviousFilenums()
        {
            List<string> ret = new List<string>();
            SqlConnection conn = new SqlConnection( Helper.ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [Code] FROM [QRCode].[dbo].[RWTable]", conn);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                ret.Add((string)reader[0]);
            }
            return ret;
        }

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            if (reg.IsMatch(tbCode.Text))
            {
                if (!PreviousFilenums.Contains(tbCode.Text) && !Filenums.Contains(tbCode.Text))
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

      
    }
}
