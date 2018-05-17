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
        #region Private members

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
        /// Regex for file numbers.
        /// </summary>
        private Regex fileNumberRegex = new Regex(@"(RSRFBA)[0-9]{2}\-[0-9]{6}");

        /// <summary>
        /// Organizational unit for associated with current codes (and file numbers).
        /// </summary>
        private string organizationalUnit;

        /// <summary>
        /// Length of file number.
        /// </summary>
        private const short fileNumberLength = 15;

        /// <summary>
        /// Lenegth of organizational unit.
        /// </summary>
        private const short orgUnitLength = 5;

        #endregion

        #region Constructors

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
            lFileNum.Text = "Preostali broj kodova za unos: " + codeNums;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds entries to SQL table - RWTable.
        /// Maps QR ID with box code,File numbers Organizational units.
        /// </summary>
        /// 
        private void InsertIntoRWTable()
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[RWTable] VALUES (@boxCode, @code,@qrid, @orgUnit)", Helper.GetConnection()))
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
                    command.Parameters.AddWithValue("@orgUnit", organizationalUnit);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }

        }

        /// <summary>
        /// Method that is invoked when text in tbFileNumCode is entered.
        /// </summary>
        /// <param name="sender">Invoking object.</param>
        /// <param name="e">Following args.</param>
        private void tbFileNumCodeTextChanged(object sender, EventArgs e)
        {
            if (fileNumberRegex.IsMatch(tbFileNumCode.Text) && tbFileNumCode.Text.Length == fileNumberLength)
            {
                if (!CheckPreviousCodes(tbFileNumCode.Text) && !Filenums.Contains(tbFileNumCode.Text))
                {
                    Filenums.Add(tbFileNumCode.Text);
                    lFileNum.Text = "Preostali broj kodova za unos: " + --codeNums;
                    tbFileNumCode.Clear();
                    if (codeNums > 0)
                    {
                        tbFileNumCode.Focus();
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
                using (SqlCommand command = new SqlCommand("SELECT [Code] FROM [QRCode].[dbo].[RWTable] WHERE [Code] = @rwCode", conn))
                {
                    command.CommandTimeout = 3600;
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
                    
                
            } 
            catch(Exception e)
            {
                MessageBox.Show("Desila se greska!");
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Method that is invoked when tbFileNumCode gains focus.
        /// Checks if organizational unit is entered correctly.
        /// </summary>
        /// <param name="sender">Invoking object.</param>
        /// <param name="e">Following args.</param>
        private void tbFileNumCodeEnter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(organizationalUnit))
            {
                tbOrgUnit.Focus();
            }
        }

        /// <summary>
        /// Method that is invoked when tbOrgUnit text is entered.
        /// Checks if the entered text is in correct format and sets internal value.
        /// </summary>
        /// <param name="sender">Invoking object.</param>
        /// <param name="e">Following args.</param>
        private void tbOrgUnitTextChanged(object sender, EventArgs e)
        {
            organizationalUnit = tbOrgUnit.Text;
        }

    }
}
