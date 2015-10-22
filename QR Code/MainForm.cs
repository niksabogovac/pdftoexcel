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
    /// <summary>
    /// Main form of project. Used for data input and generating reports.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Private members
        /// <summary>
        /// Unique item that identifies every worker that enters new data to database.
        /// </summary>
        private string jmbg;

        #region Box status
        /// <summary>
        /// Indicates whether the green box is opened or now.
        /// </summary>
        private bool greenBoxOpened;

        /// <summary>
        /// Indicates whether the red box is opened or now.
        /// </summary>
        private bool redBoxOpened;

        /// <summary>
        /// Indicates whether the yellow box is opened or now.
        /// </summary>
        private bool yellowBoxOpened;

        /// <summary>
        /// Indicates whether the blue box is opened or now.
        /// </summary>
        private bool blueBoxOpened;
        #endregion

        #region Number of files in boxes

        /// <summary>
        /// Number of files in currently opened green box.
        /// </summary>
        private int greenBoxNumOfFiles;

        /// <summary>
        /// Number of files in currently opened red box.
        /// </summary>
        private int redBoxNumOfFiles;
        /// <summary>
        /// Number of files in currently opened yellow box.
        /// </summary>
        private int yellowBoxNumOfFiles;
        /// <summary>
        /// Number of files in currently opened blue box.
        /// </summary>
        private int blueBoxNumOfFiles;

        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializePrivateMembers();
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="name">Name of the logged user.</param>
        /// <param name="jmbg">Unique parameter of every user.</param>
        public MainForm(string name, string jmbg)
        {
            InitializePrivateMembers();
            InitializeComponent();
            lWorker.Text += name;
            this.jmbg = jmbg;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Initializes private status members to default values.
        /// </summary>
        private void InitializePrivateMembers()
        {
            greenBoxOpened = false;
            redBoxOpened = false;
            yellowBoxOpened = false;
            blueBoxOpened = false;

            greenBoxNumOfFiles = 0;
            redBoxNumOfFiles = 0;
            yellowBoxNumOfFiles = 0;
            blueBoxNumOfFiles = 0;

            // Used for signalising when some codes are not properly inserted (box or order code).
            errorProvider = new System.Windows.Forms.ErrorProvider();
            errorProvider.BlinkRate = 1000;
            errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;

        }

        // NOT FINISHED IMPLEMENTING. CHECK THEIR EXAMPLE TO SEE MISTAKES AND PROBLEM.
        /// <summary>
        /// Calculates box code of existing opened boxes from input document type.
        /// </summary>
        /// <param name="doctype">Input document type.</param>
        /// <returns>Output box code.</returns>
        private string GetBoxCodeFromDocType(string doctype)
        {      
            switch (Helper.DoctypeBoxCode[doctype])
            {
                case BoxTypeEnum.KREDITI:
                    return tbRed.Text;
                case BoxTypeEnum.OROCENJA:
                    return tbBlue.Text;
                case BoxTypeEnum.POZAJMICE:
                    return tbGreen.Text;
                case BoxTypeEnum.RACUNI:
                    return tbYellow.Text;
                default:
                    return null;
            }          
        }

        /// <summary>
        /// Inserts into BankTable database table.
        /// </summary>
        /// <param name="id">Unique id.</param>
        /// <param name="boxCode">Code of open box.</param>
        /// <param name="code">Read QR code.</param>
        private void InsertoBankTable(string id, string boxCode, string code)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[BankTable] VALUES (@id, @orderNum, @boxCode, @jmbg, @code)", conn);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@orderNum", tbOrderNum.Text);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            command.Parameters.AddWithValue("@jmbg", jmbg);
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Using parameter input code parses json object and gets clientinfos and values.
        /// </summary>
        /// <param name="code">Unparsed json objects.</param>
        private void GetQrCodeAndWrite(string code)
        {
            
            string startCode = code;
            // ID of scanned code.
            string id = null;
            // Type of docyment of scanned code.
            string doctype = null;

            // Remove all non-ASCII characters.
            code = Regex.Replace(code, @"[^\u0000-\u007F]", string.Empty);

            // Remove { and }.
            code = Regex.Replace(code, "{", string.Empty);
            code = Regex.Replace(code, "}", string.Empty);

            // Separate client infos and remove ".
            string[] stringSeparators = new string[] { "," };
            string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);



            foreach (string clientInfo in tokens)
            {
                string tmp = Regex.Replace(clientInfo, "\"", string.Empty);
                string[] tmpSeparator = new string[] { ":" };
                string[] tmpTokens = tmp.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (tmpTokens[0].Equals("id"))
                {
                    // Get id.
                    id = tmpTokens[1];
                }
                else if (tmpTokens[0].Equals("doctype"))
                {
                    // Get type of document.
                    doctype = tmpTokens[1];
                }

                // Both found break.
                if (id != null && doctype != null)
                {
                    break;
                }
            }

            string boxCode = GetBoxCodeFromDocType(doctype);

            try
            {
                InsertoBankTable(id, boxCode, startCode);
                lNotification.Text = "Uspešno ste upisali.";

            }
            catch (SqlException)
            {
                lNotification.Text = "QR kod koji ste uneli ne može biti raspoređen ili je kod već upisan.";
            }
        }

        /// <summary>
        /// Using parameter box code checks database for existing box and if true checks types of boxes, if not creates new box.
        /// NOT FINISHED.
        /// </summary>
        /// <param name="boxCode">Code of box.</param>
        /// <returns>Indicator of success. 
        /// -1 failed.
        /// 0 new table created.
        /// >= 1 found existing table, current number of files.</returns>
        private int OpenOrCreateBox(string boxCode, BoxTypeEnum boxType)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            // Try to find table.
            if (reader.HasRows)
            {
                reader.Read();
                var code = (string)reader["Code"];
                var type = (int)reader["Type"];
                var fileNum = (int)reader["NumberOfFIles"];
                reader.Close();
                // Box must be opened sa same type as previously.
                if (type == (int)boxType)
                {
                    conn.Close();
                    return fileNum;
                }
                else
                {
                    // Box types are not same.
                    conn.Close();
                    return -1;
                }

            }
            else if (!reader.HasRows)
            {
                // Create new box entry.
                reader.Close();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO [QRCode].[dbo].[Box] VALUES(@boxCode, @boxType, @numberOfFiles)", conn);
                insertCommand.Parameters.AddWithValue("@boxCode", boxCode);
                insertCommand.Parameters.AddWithValue("@boxType", (int)boxType);
                insertCommand.Parameters.AddWithValue("@numberOfFiles", (int)0);
                insertCommand.ExecuteNonQuery();
                conn.Close();
                return 0;
            }
            else
            {
                conn.Close();
                return -1;
            }
            
        }

        #region Event handlers
        /// <summary>
        /// Triggered when QR Code text box has focus.
        /// </summary>
        /// <param name="sender">Text field.</param>
        /// <param name="e">Following arguments.</param>
        private void QrCodeEntered(object sender, EventArgs e)
        {
            
            // Check if all boxes are opened.
            if (!(greenBoxOpened && redBoxOpened && yellowBoxOpened && blueBoxOpened))
            {
                ((TextBox)sender).Clear();
                errorProvider.SetError((TextBox)sender, "Sve kutije moraju biti otvorene da bi se moglo skenirati.");
            }
            else
            {
                errorProvider.SetError((TextBox)sender, string.Empty);
            }

            if (tbGreen.Text.Equals(string.Empty))
            {
                // If green box code is not entered.
                // Clear the current input.
                ((TextBox)sender).Clear();
                tbGreen.Focus();
            }
            else if (tbRed.Text.Equals(string.Empty))
            {
                // If red box code is not entered.
                // Clear the current input.
                ((TextBox)sender).Clear();
                tbRed.Focus();
            }
            else if (tbYellow.Text.Equals(string.Empty))
            {
                // If yellow box code is not entered.
                // Clear the current input.
                ((TextBox)sender).Clear();
                tbYellow.Focus();
            }
            else if (tbBlue.Text.Equals(string.Empty))
            {
                // If blue box code is not entered.
                // Clear the current input.
                ((TextBox)sender).Clear();
                tbBlue.Focus();
            }
            else if (tbOrderNum.Text.Equals(string.Empty))
            {
                // If order number is not entered
                // Clear the current input.
                ((TextBox)sender).Clear();
                tbOrderNum.Focus();
            }

        }

        /// <summary>
        /// Triggered when open/close button is clicked and changes the current state of green box.
        /// </summary>
        /// <param name="sender">Open or close button of green box.</param>
        /// <param name="e">Following arguments.</param>
        private void OpenCloseGreenBoxMouseClick(object sender, EventArgs e)
        {
            if (greenBoxOpened)
            {
                // Box was opened, remove box code and close it.
                greenBoxOpened = false;
                lNumFilesGreen.Text = string.Empty;
                lStatusGreen.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbGreen.Clear();
                greenBoxNumOfFiles = 0;
            }
            else if (greenBoxOpened == false)
            {
                // Box was closed, open is possible only if box code is entered.
                if (tbGreen.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbGreen, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbGreen.Focus();
                    
                }
                else
                {
                    greenBoxNumOfFiles = OpenOrCreateBox(tbGreen.Text, BoxTypeEnum.POZAJMICE);
                    if (greenBoxNumOfFiles == -1)
                    {
                        MessageBox.Show("Neuspešno otvorena kutija, proverite da li je i prošli put otvarati kao tog tipa - POZAJMICE.");
                    }
                    else
                    {
                        errorProvider.SetError(tbGreen, string.Empty);
                        greenBoxOpened = true;
                        lStatusGreen.Text = "Status: Otvorena";
                        ((Button)sender).Text = "Zatvori";
                        lNumFilesGreen.Text = "Broj fajlova u kutiji: " + greenBoxNumOfFiles;
                    }
                }
            }
        }

        /// <summary>
        /// Triggered when open/close button is clicked and changes the current state of red box.
        /// </summary>
        /// <param name="sender">Open or close button of red box.</param>
        /// <param name="e">Following arguments.</param>
        private void OpenCloseRedBoxMouseClick(object sender, EventArgs e)
        {
            if (redBoxOpened)
            {
                // Box was opened, remove box code and close it.
                redBoxOpened = false;
                lStatusRed.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                lNumFilesRed.Text = string.Empty;
                redBoxNumOfFiles = 0;
                tbRed.Clear();
            }
            else if (redBoxOpened == false)
            {
                // Box was closed, open is possible only if box code is entered.
                if (tbRed.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbRed, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbRed.Focus();
                }
                else
                {
                    
                    redBoxNumOfFiles = OpenOrCreateBox(tbRed.Text, BoxTypeEnum.KREDITI);
                    if (redBoxNumOfFiles == -1)
                    {
                        MessageBox.Show("Neuspešno otvorena kutija, proverite da li je i prošli put otvarati kao tog tipa - KREDITI.");
                    }
                    else
                    {
                        errorProvider.SetError(tbRed, string.Empty);
                        redBoxOpened = true;
                        lStatusRed.Text = "Status: Otvorena";
                        ((Button)sender).Text = "Zatvori";
                        lNumFilesRed.Text = "Broj fajlova u kutiji: " + redBoxNumOfFiles;
                    }
                }
            }
        }

        /// <summary>
        /// Triggered when open/close button is clicked and changes the current state of yellow box.
        /// </summary>
        /// <param name="sender">Open or close button of yellow box.</param>
        /// <param name="e">Following arguments.</param>
        private void OpenCloseYellowBoxMouseClick(object sender, EventArgs e)
        {
            if (yellowBoxOpened)
            {
                // Box was opened, remove box code and close it.
                yellowBoxOpened = false;
                lStatusYellow.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbYellow.Clear();
                yellowBoxNumOfFiles = 0;
                lNumFilesYellow.Text = string.Empty;
            }
            else if (yellowBoxOpened == false)
            {
                // Box was closed, open is possible only if box code is entered.
                if (tbYellow.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbYellow, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbYellow.Focus();
                }
                else
                {
                    yellowBoxNumOfFiles = OpenOrCreateBox(tbYellow.Text, BoxTypeEnum.RACUNI);
                    if (yellowBoxNumOfFiles == -1)
                    {
                        MessageBox.Show("Neuspešno otvorena kutija, proverite da li je i prošli put otvarati kao tog tipa - RAČUNI.");
                    }
                    else
                    {
                        errorProvider.SetError(tbYellow, string.Empty);
                        yellowBoxOpened = true;
                        lStatusYellow.Text = "Status: Otvorena";
                        ((Button)sender).Text = "Zatvori";
                        lNumFilesYellow.Text = "Broj fajlova u kutiji: " + yellowBoxNumOfFiles; 
                    }

                }
            }
        }

        /// <summary>
        /// Triggered when open/close button is clicked and changes the current state of blue box.
        /// </summary>
        /// <param name="sender">Open or close button of blue box.</param>
        /// <param name="e">Following arguments.</param>
        private void OpenCloseBlueBoxMouseClick(object sender, EventArgs e)
        {
            if (blueBoxOpened)
            {
                // Box was opened, remove box code and close it.
                blueBoxOpened = false;
                lStatusBlue.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbBlue.Clear();
                blueBoxNumOfFiles = 0;
                lNumFilesBlue.Text = string.Empty;
            }
            else if (blueBoxOpened == false)
            {
                // Box was closed, open is possible only if box code is entered.
                if (tbBlue.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbBlue, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbBlue.Focus();
                }
                else
                {
                    blueBoxNumOfFiles = OpenOrCreateBox(tbBlue.Text, BoxTypeEnum.OROCENJA);
                    if (blueBoxNumOfFiles == -1)
                    {
                        MessageBox.Show("Neuspešno otvorena kutija, proverite da li je i prošli put otvarati kao tog tipa - OROČENJA.");
                    }
                    else
                    {
                        errorProvider.SetError(tbBlue, string.Empty);
                        blueBoxOpened = true;
                        lStatusBlue.Text = "Status: Otvorena";
                        ((Button)sender).Text = "Zatvori";
                        lNumFilesBlue.Text = "Broj fajlova u kutiji: " + blueBoxNumOfFiles;
                    }

                }
            }
        }

        /// <summary>
        /// Triggers when new data is added.
        /// </summary>
        /// <param name="sender">Add data button.</param>
        /// <param name="e">Following arguments.</param>
        private void AddDataMouseClick(object sender, MouseEventArgs e)
        {
            // Check if qr code is properly scanned.
            if (tbQr.Text.Equals(string.Empty))
            {
                errorProvider.SetError((Button)sender, "Unesite odgovarajući QR kod.");
                tbQr.Focus();
            }
            else
            {
                errorProvider.SetError((Button)sender, string.Empty);
                GetQrCodeAndWrite(tbQr.Text); 
            }

        }

        /// <summary>
        /// Triggered when QrCode is scanned or manually entered.
        /// </summary>
        /// <param name="sender">Text field.</param>
        /// <param name="e">Following arguments.</param>
        private void QrCodeValueChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int textLength = textBox.Text.Length;
            textBox.Text.Trim();
            if (textLength > 0 && textBox.Text.Last().Equals('}'))
            {
                GetQrCodeAndWrite(textBox.Text);
                textBox.Clear();
                textBox.SelectionStart = 0;
            }
        }

        #endregion



        #endregion
    }
}
