using ExcelLibrary.SpreadSheet;
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

        /// <summary>
        /// Reades input box code type from database.
        /// </summary>
        /// <param name="boxCode">Input box code.</param>
        /// <returns>Output box type.</returns>
        private int GetTypeFromBoxCode(string boxCode)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [Type] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                return (int)reader[0];
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Calculates box code of existing opened boxes from input document type.
        /// </summary>
        /// <param name="doctype">Input document type.</param>
        /// <returns>Output box code.</returns>
        private string GetBoxCodeFromDocType(string doctype)
        {
            // If current doctype is historic get its current doctype.
            if (Helper.ClientInfoNamesHistoricalNames.ContainsKey(doctype))
            {
                doctype = Helper.ClientInfoNamesHistoricalNames[doctype];
            }
             
            if (Helper.DoctypeBoxCode.ContainsKey(doctype))
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
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Inserts into BankTable database table.
        /// </summary>
        /// <param name="id">Unique id.</param>
        /// <param name="boxCode">Code of open box.</param>
        /// <param name="code">Read QR code.</param>
        private void InserToBankTable(string id, string boxCode, string code)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[BankTable] VALUES (@id, @orderNum, @boxCode, @date, @jmbg, @code)", conn);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@orderNum", tbOrderNum.Text);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@jmbg", jmbg);
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Updates values of nubmer of files in box when new QrCode values is scaned.
        /// </summary>
        /// <param name="boxCode"></param>
        /// <param name="numberOfFiles"></param>
        private void UpdateBoxTable(string boxCode, int numberOfFiles)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = @fileNum WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@fileNum", numberOfFiles);
            command.Parameters.AddWithValue("@boxCode", boxCode);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets the number of files in box from database.
        /// </summary>
        /// <param name="boxCode">Unique identifier of box.</param>
        /// <returns></returns>
        private int GetNumberOfFilesFromBox(string boxCode)
        {
            int fileNum = -1;

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                fileNum = (int)reader["NumberOfFiles"];
            }
            return fileNum;
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
            code = Regex.Replace(code, " ", string.Empty);
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

                // Both found - break.
                if (id != null && doctype != null)
                {
                    break;
                }
            }

            string boxCode = GetBoxCodeFromDocType(doctype);
            
            
            
            
            if (boxCode == null)
            {
                lNotification.Text = "QR kod koji ste uneli ne može biti raspoređen jer ne postoji doctype - " + doctype + ".";
                return;
            }
            try
            {
                InserToBankTable(id, boxCode, startCode);
                int boxType = GetTypeFromBoxCode(boxCode);
                switch(boxType)
                {
                    case 86:
                        
                        greenBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                        UpdateBoxTable(boxCode, ++greenBoxNumOfFiles);
                        lNumFilesGreen.Text = "Broj fajlova u kutiji: " + greenBoxNumOfFiles;
                        ColorPanels(true, false, false, false);
                        break;
                    case 148:
                        redBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                        UpdateBoxTable(boxCode, ++redBoxNumOfFiles);
                        lNumFilesRed.Text = "Broj fajlova u kutiji: " + redBoxNumOfFiles;
                        ColorPanels(false, true, false, false);
                        break;
                    case 82:
                        yellowBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                        UpdateBoxTable(boxCode, ++yellowBoxNumOfFiles);
                        lNumFilesYellow.Text = "Broj fajlova u kutiji: " + yellowBoxNumOfFiles;
                        ColorPanels(false, false, true, false);
                        break;
                    case 83:
                        blueBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                        UpdateBoxTable(boxCode, ++blueBoxNumOfFiles);
                        lNumFilesBlue.Text = "Broj fajlova u kutiji: " + blueBoxNumOfFiles;
                        ColorPanels(false, false, false, true);
                        break;
                    default:
                        lNotification.Text = "Nije moguće.";
                        return;
                }
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

        /// <summary>
        /// Sets background color of panel depending on input parameters.
        /// </summary>
        /// <param name="green">Should green box be colored.</param>
        /// <param name="red">Should red box be colored.</param>
        /// <param name="yellow">Should yellow box be colored.</param>
        /// <param name="blue">Should blue box be colored.</param>
        private void ColorPanels(bool green,bool red, bool yellow, bool blue)
        {
            if (green)
            {
                pGreen.BackColor = Color.Green;
            }
            else
            {
                pGreen.BackColor = Color.White;
            }
            if (red)
            {
                pRed.BackColor = Color.Red;
            }
            else
            {
                pRed.BackColor = Color.White;
            }
            if (yellow)
            {
                pYellow.BackColor = Color.Yellow;
            }
            else
            {
                pYellow.BackColor = Color.White;
            }
            if (blue)
            {
                pBlue.BackColor = Color.Blue;
            }
            else
            {
                pBlue.BackColor = Color.White;
            }

        }
        
        /// <summary>
        /// Calculates number of needed codes for RW reports.
        /// </summary>
        /// <param name="numberOfFiles">Number of files.</param>
        /// <returns>Calculated number of boxes</returns>
        private int CalculateNumberOfCodes(int numberOfFiles)
        {
            int ret = 0;
            ret = numberOfFiles / 30;
            if (numberOfFiles % 30 != 0)
            {
                ret++;
            }
            return ret;
        }

        /// <summary>
        /// Validates if codes are inserted into database for box.
        /// </summary>
        /// <param name="boxCode">Box code.</param>
        /// <param name="fileNum">Current number of files.</param>
        /// <returns>Difference between number of codes in database and number of files in box</returns>
        private int CheckNumberOfCodes(string boxCode,int fileNum)
        {
            int ret = 0;
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[RWTable] WHERE [BoxCode] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.CommandText = "select @@ROWCOUNT";
            int totalCodes = (int)command.ExecuteScalar();
            // Difference between current number of files and inserted number of codes in database.
            ret = CalculateNumberOfCodes(fileNum) - totalCodes;
            return ret;
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
                // Check if codes for RW report are inserted into database.
                int newCodes = CheckNumberOfCodes(tbGreen.Text,greenBoxNumOfFiles);
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbGreen.Text);
                    diag.ShowDialog();
                }
                greenBoxOpened = false;
                lNumFilesGreen.Text = string.Empty;
                lStatusGreen.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbGreen.Clear();
                greenBoxNumOfFiles = 0;
                tbGreen.Enabled = true;
                tbGreen.Focus();
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
                        tbGreen.Enabled = false;
                        //Focus next open text box.
                        if (tbRed.Enabled)
                        {
                            tbRed.Focus();
                        }
                        else if (tbYellow.Enabled)
                        {
                            tbYellow.Focus();
                        }
                        else if (tbBlue.Enabled)
                        {
                            tbBlue.Focus();
                        }
                        else
                        {
                            tbQr.Focus();
                        }
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
                // Check if codes for RW report are inserted into database.
                int newCodes = CheckNumberOfCodes(tbRed.Text, redBoxNumOfFiles);
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbRed.Text);
                    diag.ShowDialog();
                }
                redBoxOpened = false;
                lStatusRed.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                lNumFilesRed.Text = string.Empty;
                redBoxNumOfFiles = 0;
                tbRed.Clear();
                tbRed.Enabled = true;
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
                        tbRed.Enabled = false;
                        //Focus next open text box.
                        if (tbGreen.Enabled)
                        {
                            tbGreen.Focus();
                        }
                        else if (tbYellow.Enabled)
                        {
                            tbYellow.Focus();
                        }
                        else if (tbBlue.Enabled)
                        {
                            tbBlue.Focus();
                        }
                        else
                        {
                            tbQr.Focus();
                        }
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
                // Check if codes for RW report are inserted into database.
                int newCodes = CheckNumberOfCodes(tbYellow.Text, yellowBoxNumOfFiles);
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbYellow.Text);
                    diag.ShowDialog();
                }
                yellowBoxOpened = false;
                lStatusYellow.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbYellow.Clear();
                yellowBoxNumOfFiles = 0;
                lNumFilesYellow.Text = string.Empty;
                tbYellow.Enabled = true;
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
                        tbYellow.Enabled = false;
                        //Focus next open text box.
                        if (tbGreen.Enabled)
                        {
                            tbGreen.Focus();
                        }
                        else if (tbRed.Enabled)
                        {
                            tbRed.Focus();
                        }
                        else if (tbBlue.Enabled)
                        {
                            tbBlue.Focus();
                        }
                        else
                        {
                            tbQr.Focus();
                        }
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
                // Check if codes for RW report are inserted into database.
                int newCodes = CheckNumberOfCodes(tbBlue.Text, blueBoxNumOfFiles);
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbBlue.Text);
                    diag.ShowDialog();
                }
                blueBoxOpened = false;
                lStatusBlue.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbBlue.Clear();
                blueBoxNumOfFiles = 0;
                lNumFilesBlue.Text = string.Empty;
                tbBlue.Enabled = true;
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
                        tbBlue.Enabled = false;
                        //Focus next open text box.
                        if (tbGreen.Enabled)
                        {
                            tbGreen.Focus();
                        }
                        else if (tbRed.Enabled)
                        {
                            tbRed.Focus();
                        }
                        else if (tbYellow.Enabled)
                        {
                            tbYellow.Focus();
                        }
                        else
                        {
                            tbQr.Focus();
                        }
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

        /// <summary>
        /// Triggered when report from menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportMenuStripItemClick(object sender, EventArgs e)
        {
            ReportDialog diag = new ReportDialog();
            diag.ShowDialog();
        }

        #endregion


        #endregion
    }
}
