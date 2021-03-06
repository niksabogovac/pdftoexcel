﻿using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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

        /// <summary>
        /// Regex za kodove kutije.
        /// </summary>
        private Regex boxCodeRegex = new Regex(@"(RSRFBA)[0-9]{2}C-[0-9]{5,6}");

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

        #region Control number of files in boxes

        /// <summary>
        /// Number of files in green box before scaning.
        /// </summary>
        private int greenBoxNumOfFilesCtrl;

        /// <summary>
        /// Number of files in red box before scaning.
        /// </summary>
        private int redBoxNumOfFilesCtrl;
        /// <summary>
        /// Number of files in yellow box before scaning.
        /// </summary>
        private int yellowBoxNumOfFilesCtrl;
        /// <summary>
        /// Number of files in blue box before scaning.
        /// </summary>
        private int blueBoxNumOfFilesCtrl;

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
            if (jmbg.Equals("h.bogovac") || jmbg.Equals("admin1"))
            {
                izbrišiToolStripMenuItem.Enabled = true;
                izbrišiToolStripMenuItem.Visible = true;
            }
            else
            {
                izbrišiToolStripMenuItem.Enabled = false;
                izbrišiToolStripMenuItem.Visible = false;
            }
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
            

            try
            {
                Helper.UpdateDocTypesFromDatabase();
            } catch(SqlException e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        /// <summary>
        /// Reades input box code type from database.
        /// </summary>
        /// <param name="boxCode">Input box code.</param>
        /// <returns>Output box type.</returns>
        private int GetTypeFromBoxCode(string boxCode)
        {
            SqlConnection conn = Helper.GetConnection();
            //conn.Open();
            using (SqlCommand command = new SqlCommand("SELECT [Type] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn))
            {
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int ret = (int)reader[0];
                        return ret;
                    }
                    else
                    {
                        reader.Close();
                        return -1;
                    }
                }
                
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
        private void InsertToBankTable(string id, string boxCode, string code)
        {
            SqlConnection conn = Helper.GetConnection();
            using (SqlCommand command = new SqlCommand("INSERT INTO [QRCode].[dbo].[BankTable] VALUES (@id, @orderNum, @boxCode, @date, @jmbg, @code,@takeoverDate)", conn))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@orderNum", tbOrderNum.Text);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@jmbg", jmbg);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@takeoverDate", dtpTakeover.Value);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates values of nubmer of files in box when new QrCode values is scaned.
        /// </summary>
        /// <param name="boxCode"></param>
        /// <param name="numberOfFiles"></param>
        private void UpdateBoxTable(string boxCode)
        {
            SqlConnection conn = Helper.GetConnection();
            using (SqlCommand command = new SqlCommand("UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = (SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode) + 1  WHERE [Code] = @boxCode", conn))
            {
                command.Parameters.AddWithValue("@boxCode", boxCode);
                command.ExecuteNonQuery();
            }
            
        }

        /// <summary>
        /// Gets the number of files in box from database.
        /// </summary>
        /// <param name="boxCode">Unique identifier of box.</param>
        /// <returns></returns>
        private int GetNumberOfFilesFromBox(string boxCode)
        {
            int fileNum = -1;

            SqlConnection conn = Helper.GetConnection();
            using (SqlCommand command = new SqlCommand("SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn))
            {
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        fileNum = (int)reader["NumberOfFiles"];
                    }
                }

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

            try
            {
                // Remove all non-ASCII characters.
                code = Regex.Replace(code, @"[^\u0000-\u007F]", string.Empty);
                // Remove \000021 from code.
                code = Regex.Replace(code, @"\\000021", string.Empty);
                // Remove { and }.
                code = Regex.Replace(code, "{", string.Empty);
                code = Regex.Replace(code, "}", string.Empty);
                code = Regex.Replace(code, " ", string.Empty);
                code = Regex.Replace(code, "\r\n", string.Empty);
                // Separate client infos and remove ".
                string[] stringSeparators = new string[] { "," };
                string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string clientInfo in tokens)
                {
                    string tmp = Regex.Replace(clientInfo, "\"", string.Empty);
                    string[] tmpSeparator = new string[] { ":" };
                    string[] tmpTokens = tmp.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (Helper.CheckId(tmpTokens[0]))
                    {
                        // Get id.
                        id = tmpTokens[1];
                    }
                    else if (Helper.CheckDocType(tmpTokens[0]))
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
                    //MessageBox.Show("QR kod koji ste uneli ne može biti raspoređen jer ne postoji doctype - " + doctype + ".");
                    //lNotification.Text = string.Empty;
                    lNotification.Text = "";
                    Thread.Sleep(100);
                    lNotification.Text = "Ne postoji doctype - " + doctype + ".";
                    lNotification.ForeColor = Color.Red;
                    return;
                }
                string errorMessage = string.Empty;
            
                    InsertToBankTable(id, boxCode, startCode);
                    int boxType = GetTypeFromBoxCode(boxCode);
                    switch(boxType)
                    {
                        case 86:
                            UpdateBoxTable(boxCode);
                            greenBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                            lNumFilesGreen.Text = "Broj fajlova u kutiji: " + greenBoxNumOfFiles;
                            ColorPanels(true, false, false, false);
                            break;
                        case 148:
                            UpdateBoxTable(boxCode);
                            redBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                            lNumFilesRed.Text = "Broj fajlova u kutiji: " + redBoxNumOfFiles;
                            ColorPanels(false, true, false, false);
                            break;
                        case 82:
                            UpdateBoxTable(boxCode);
                            yellowBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                            lNumFilesYellow.Text = "Broj fajlova u kutiji: " + yellowBoxNumOfFiles;
                            ColorPanels(false, false, true, false);
                            break;
                        case 83:
                            UpdateBoxTable(boxCode);
                            blueBoxNumOfFiles = GetNumberOfFilesFromBox(boxCode);
                            lNumFilesBlue.Text = "Broj fajlova u kutiji: " + blueBoxNumOfFiles;
                            ColorPanels(false, false, false, true);
                            break;
                        default:
                            lNotification.Text = "Nije moguće.";
                            return;
                    }
                    lNotification.Text = "";
                    Thread.Sleep(100);
                    lNotification.Text = "Uspešno ste upisali.";
                    lNotification.ForeColor = Color.Green;

            }
            catch (SqlException e)
            {
                string c = string.Empty;
                try
                {
                    SqlConnection conn = Helper.GetConnection();
                    
                    using (SqlCommand command = new SqlCommand("SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id", conn))
                    {
                        command.CommandTimeout = 500;
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                c = " u kutiju: " + (string)reader[0];
                            }

                        }

                    }

                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
                finally
                {
                    lNotification.Text = "";
                    Thread.Sleep(100);
                    lNotification.Text = "QR kod je već upisan" + c;
                    lNotification.ForeColor = Color.Red;
                }
                
                
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
            SqlConnection conn = Helper.GetConnection();
            //conn.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn))
            {
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Try to find table.
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var code = (string)reader["Code"];
                        var type = (int)reader["Type"];
                        var fileNum = (int)reader["NumberOfFIles"];
                        // Box must be opened sa same type as previously.
                        if (type == (int)boxType)
                        {
                            return fileNum;
                        }
                        else
                        {
                            // Box types are not same.
                            return -1;
                        }

                    }
                    else if (!reader.HasRows)
                    {
                        reader.Close();
                        reader.Dispose();
                        // Create new box entry.
                        using (SqlCommand insertCommand = new SqlCommand("INSERT INTO [QRCode].[dbo].[Box] VALUES(@boxCode, @boxType, @numberOfFiles)", conn))
                        {
                            insertCommand.Parameters.AddWithValue("@boxCode", boxCode);
                            insertCommand.Parameters.AddWithValue("@boxType", (int)boxType);
                            insertCommand.Parameters.AddWithValue("@numberOfFiles", (int)0);
                            insertCommand.ExecuteNonQuery();
                        }
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
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
            ret = numberOfFiles / 20;
            if (numberOfFiles % 20 != 0)
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
        private int CheckNumberOfCodes(string boxCode,int fileNum,ref List<string>QRIDs)
        {
            int ret = 0;
            SqlConnection conn = Helper.GetConnection();
            //conn.Open();
            using (SqlCommand command = new SqlCommand("SELECT  [ID] FROM [QRCode].[dbo].[BankTable] WHERE [MBR] = @mbr AND [ID] NOT IN (SELECT [QRID] from [QRCode].[dbo].[RWTable] WHERE [BoxCode] = @boxCode) AND [BoxCode] = @boxCode", conn))
            {
                command.CommandTimeout = 3600;
                command.Parameters.AddWithValue("@mbr", jmbg);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Fill database with QRIDs.
                        while (reader.Read())
                        {
                            QRIDs.Add((string)reader[0]);
                        }
                    }
                }
                command.CommandText = "select @@ROWCOUNT";
                int totalCodes = (int)command.ExecuteScalar();
                // Difference between current number of files and inserted number of codes in database.
                ret = CalculateNumberOfCodes(totalCodes);
            }
            
            return ret;
        }

        /// <summary>
        /// Deletes data from tables BankTable, Box and RW table.
        /// </summary>
        private void DeleteDatabaseData()
        {
            SqlConnection conn = Helper.GetConnection();
            using (SqlCommand command = new SqlCommand("DELETE FROM [QRCode].[dbo].[BankTable]", conn))
            {
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [QRCode].[dbo].[Box]";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [QRCode].[dbo].[RWTable]";
                command.ExecuteNonQuery();
            }
        }

        private static ISet<string> GetFileNumbersForBox(string boxCode)
        {
            HashSet<string> fileNumbers = new HashSet<string>();
            string selectPart   =  "SELECT DISTINCT Code ";
            string fromPart     =  "FROM [QRCode].[dbo].[BankTable] b INNER JOIN [QRCode].[dbo].[RWTable] r on b.ID = r.QRID ";
            string wherePart    = $"WHERE Code IS NOT NULL and b.BoxCode = '{boxCode}'";
            using (SqlCommand command = new SqlCommand(selectPart + fromPart + wherePart, Helper.GetConnection()))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fileNumbers.Add((string)reader["Code"]);
                    }
                }
            }

            return fileNumbers;
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
                // List for QR Codes that dont have Filenumber added.
                // If check box for closing was selected all codes from this box must have filenumbers assigned!
                ColorButtons(1);

                List<string> QRIDs = new List<string>();
                int newCodes = CheckNumberOfCodes(tbGreen.Text,greenBoxNumOfFiles, ref QRIDs);


                if (cbCloseGreen.Checked)
                {
                    if (newCodes > 0)
                    {
                        MessageBox.Show("Potrebno je uneti " + newCodes.ToString() + " filenumber za kodove koje ste skenirali!");
                        return;
                    } 
                    else
                    {
                        CloseBoxValidator validator = new CloseBoxValidator(GetFileNumbersForBox(tbGreen.Text));
                        if (validator.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        greenBoxOpened = false;
                        lNumFilesGreen.Text = string.Empty;
                        lStatusGreen.Text = "Status: Zatvorena";
                        ((Button)sender).Text = "Otvori";
                        tbGreen.Clear();
                        greenBoxNumOfFiles = 0;
                        tbGreen.Enabled = true;
                        tbGreen.Focus();
                        cbCloseGreen.Checked = false;
                        cbCloseGreen.Enabled = false;
                        return;
                    }
                    
                }

                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbGreen.Text,QRIDs);
                    diag.ShowDialog();
                } 
                else
                {
                    MessageBox.Show("Uneli ste filenumbere za sve kodove iz ove kutije!");
                }
                

            }
            else if (greenBoxOpened == false)
            {
                ColorButtons(0);
                // Box was closed, open is possible only if box code is entered.
                if (tbGreen.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbGreen, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbGreen.Focus();

                }
                else if (!boxCodeRegex.IsMatch(tbGreen.Text) || (tbGreen.Text.Length != 15 && tbGreen.Text.Length != 16))
                {
                    MessageBox.Show("Neuspešno otvorena kutija, nije ispravan format koda.");
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
                        ((Button)sender).Text = "Unesi filenumber";
                        lNumFilesGreen.Text = "Broj fajlova u kutiji: " + greenBoxNumOfFiles;
                        tbGreen.Enabled = false;
                        cbCloseGreen.Enabled = true;
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
            tbQr.Focus();
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
                ColorButtons(2);
                // Box was opened, remove box code and close it.
                // Check if codes for RW report are inserted into database.
                // List for QR Codes that dont have Filenumber added.
                List<string> QRIDs = new List<string>();
                int newCodes = CheckNumberOfCodes(tbRed.Text, redBoxNumOfFiles,ref QRIDs);

                if (cbCloseRed.Checked)
                {
                    if (newCodes > 0)
                    {
                        MessageBox.Show("Potrebno je uneti " + newCodes.ToString() + " filenumber za kodove koje ste skenirali!");
                        return;
                    }
                    else
                    {
                        CloseBoxValidator validator = new CloseBoxValidator(GetFileNumbersForBox(tbRed.Text));
                        if (validator.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        redBoxOpened = false;
                        lStatusRed.Text = "Status: Zatvorena";
                        ((Button)sender).Text = "Otvori";
                        lNumFilesRed.Text = string.Empty;
                        redBoxNumOfFiles = 0;
                        tbRed.Clear();
                        tbRed.Enabled = true;
                        cbCloseRed.Checked = false;
                        cbCloseRed.Enabled = false;
                        return;
                    }
                }
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbRed.Text, QRIDs);
                    diag.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Uneli ste filenumbere za sve kodove iz ove kutije!");
                }
                
            }
            else if (redBoxOpened == false)
            {
                ColorButtons(0);
                // Box was closed, open is possible only if box code is entered.
                if (tbRed.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbRed, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbRed.Focus();
                }
                else if (!boxCodeRegex.IsMatch(tbRed.Text) || (tbRed.Text.Length != 15 && tbRed.Text.Length != 16))
                {
                    MessageBox.Show("Neuspešno otvorena kutija, nije ispravan format koda.");
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
                        ((Button)sender).Text = "Unesi filenumber";
                        lNumFilesRed.Text = "Broj fajlova u kutiji: " + redBoxNumOfFiles;
                        tbRed.Enabled = false;
                        cbCloseRed.Enabled = true;
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
            tbQr.Focus();
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
                ColorButtons(3);
                // Box was opened, remove box code and close it.
                // Check if codes for RW report are inserted into database.
                // List for QR Codes that dont have Filenumber added.
                List<string> QRIDs = new List<string>();
                int newCodes = CheckNumberOfCodes(tbYellow.Text, yellowBoxNumOfFiles,ref QRIDs);

                if (cbCloseYellow.Checked)
                {

                    if (newCodes > 0)
                    {
                        MessageBox.Show("Potrebno je uneti " + newCodes.ToString() + " filenumber za kodove koje ste skenirali!");
                        return;
                    }
                    else
                    {
                        CloseBoxValidator validator = new CloseBoxValidator(GetFileNumbersForBox(tbYellow.Text));
                        if (validator.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        yellowBoxOpened = false;
                        lStatusYellow.Text = "Status: Zatvorena";
                        ((Button)sender).Text = "Otvori";
                        tbYellow.Clear();
                        yellowBoxNumOfFiles = 0;
                        lNumFilesYellow.Text = string.Empty;
                        tbYellow.Enabled = true;
                        cbCloseYellow.Checked = false;
                        cbCloseYellow.Enabled = false;
                        return;
                    }
                    
                }
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbYellow.Text, QRIDs);
                    diag.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Uneli ste filenumbere za sve kodove iz ove kutije!");
                }
                
            }
            else if (yellowBoxOpened == false)
            {
                ColorButtons(0);
                // Box was closed, open is possible only if box code is entered.
                if (tbYellow.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbYellow, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbYellow.Focus();
                }
                else if (!boxCodeRegex.IsMatch(tbYellow.Text) || (tbYellow.Text.Length != 15 && tbYellow.Text.Length != 16))
                {
                    MessageBox.Show("Neuspešno otvorena kutija, nije ispravan format koda.");
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
                        ((Button)sender).Text = "Unesi filenumber";
                        lNumFilesYellow.Text = "Broj fajlova u kutiji: " + yellowBoxNumOfFiles;
                        tbYellow.Enabled = false;
                        cbCloseYellow.Enabled = true;
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
            tbQr.Focus();
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
                ColorButtons(4);
                // Box was opened, remove box code and close it.
                // Check if codes for RW report are inserted into database.
                // List for QR Codes that dont have Filenumber added.
                List<string> QRIDs = new List<string>();
                int newCodes = CheckNumberOfCodes(tbBlue.Text, blueBoxNumOfFiles,ref QRIDs);

                if (cbCloseBlue.Checked)
                {
                    if (newCodes > 0)
                    {
                        MessageBox.Show("Potrebno je uneti " + newCodes.ToString() + " filenumber za kodove koje ste skenirali!" );
                        return;
                    }
                    else
                    {
                        CloseBoxValidator validator = new CloseBoxValidator(GetFileNumbersForBox(tbBlue.Text));
                        if (validator.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        blueBoxOpened = false;
                        lStatusBlue.Text = "Status: Zatvorena";
                        ((Button)sender).Text = "Otvori";
                        tbBlue.Clear();
                        blueBoxNumOfFiles = 0;
                        lNumFilesBlue.Text = string.Empty;
                        tbBlue.Enabled = true;
                        cbCloseBlue.Checked = false;
                        cbCloseBlue.Enabled = false;
                        return;
                    }
                }
                if (newCodes > 0)
                {
                    CloseBoxDialog diag = new CloseBoxDialog(newCodes, tbBlue.Text, QRIDs);
                    diag.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Uneli ste filenumbere za sve kodove iz ove kutije!");
                }
                
            }
            else if (blueBoxOpened == false)
            {
                ColorButtons(0);
                // Box was closed, open is possible only if box code is entered.
                if (tbBlue.Text.Equals(string.Empty))
                {
                    errorProvider.SetError(tbBlue, "Da biste otvorili kutiju, morate uneti šifru.");
                    tbBlue.Focus();
                }
                else if (!boxCodeRegex.IsMatch(tbBlue.Text) || (tbBlue.Text.Length != 15 && tbBlue.Text.Length != 16))
                {
                    MessageBox.Show("Neuspešno otvorena kutija, nije ispravan format koda.");
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
                        ((Button)sender).Text = "Unesi filenumber";
                        lNumFilesBlue.Text = "Broj fajlova u kutiji: " + blueBoxNumOfFiles;
                        tbBlue.Enabled = false;
                        cbCloseBlue.Enabled = true;
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
            tbQr.Focus();
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
            
            //textBox.Text = textBox.Text.Trim();
            if (Regex.IsMatch(textBox.Text, @"{((.|\n)*)}"))
            {
                while (textBox.Text.EndsWith("\r\n"))
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.LastIndexOf("\r\n"), 1);
                }
                int textLength = textBox.Text.Length;
                if (textLength > 0 && textBox.Text.Last().Equals('}'))
                {
                    // Set control number of files to check if error appears.
                    SetCtrlNumberOfFiles();
                    GetQrCodeAndWrite(textBox.Text);
                    textBox.Clear();
                    textBox.SelectionStart = 0;
                    if (!CheckCtrlNumberOfFiles())
                    {
                        MessageBox.Show("Desila se greska, proverite da li se kodovi skeniraju!");
                    }
                }
                ColorButtons(0);
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

        /// <summary>
        /// Menu item strip delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IzbrišiBazuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Da li ste sigurni da želite da izbrišete podatke iz baze?",string.Empty,MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    DeleteDatabaseData();
                    MessageBox.Show("Uspešno ste počistili bazu.");
                }
                catch(SqlException)
                {
                    MessageBox.Show("Greška prilikom brisanja baze.");
                }
            }
        }

        /// <summary>
        /// Show dialog for adding, updating and deleting doctypes.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Following args.</param>
        private void DoctypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoctypeDialog diag = new DoctypeDialog();
            diag.ShowDialog();
        }
      

        /// <summary>
        /// Menu item strip manage users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void korisnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserForm form = new UserForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Menu strip item delete box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kutijuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteForm form = new DeleteForm(2);
            form.ShowDialog();
        }

        /// <summary>
        /// Menu strip item manage order nums.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brojNalogaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DataUpdater("Broj Naloga", "[QRCode].[dbo].[BankTable]", "[OrderNum]", "[ID]", Helper.GetConnection()).ShowDialog();
        }

        /// <summary>
        /// Menu strip item delete single qr code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pojedinacnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteForm form = new DeleteForm(3);
            form.ShowDialog();
        }

        /// <summary>
        /// Menu strip item delete qr code from input table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void izTabeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteForm form = new DeleteForm(1);
            form.ShowDialog();
        }

        /// <summary>
        /// Main form closing event - clear connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SqlConnection connection = Helper.GetConnection();
            if (connection != null)
            {
                try
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
                catch (Exception exp)
                {

                }
            }
        }

        #endregion


        #endregion

        
        /// <summary>
        /// Colors buttons depending on indicator.
        /// </summary>
        /// <param name="indicator">0 - don't color, 1 - first button and etc.</param>
        private void ColorButtons(int indicator)
        {
            switch (indicator)
            {
                case 0:
                    bCloseGreen.BackColor = Color.White;
                    bCloseRed.BackColor = Color.White; ;
                    bCloseYellow.BackColor = Color.White; 
                    bCloseBlue.BackColor = Color.White;
                    break;
                case 1:
                    bCloseGreen.BackColor = Color.Red;
                    bCloseRed.BackColor = Color.White; ;
                    bCloseYellow.BackColor = Color.White;
                    bCloseBlue.BackColor = Color.White;
                    break;
                case 2:
                    bCloseGreen.BackColor = Color.White;
                    bCloseRed.BackColor = Color.Red; ;
                    bCloseYellow.BackColor = Color.White;
                    bCloseBlue.BackColor = Color.White;
                    break;
                case 3:
                    bCloseGreen.BackColor = Color.White;
                    bCloseRed.BackColor = Color.White; ;
                    bCloseYellow.BackColor = Color.Red;
                    bCloseBlue.BackColor = Color.White;
                    break;
                case 4:
                    bCloseGreen.BackColor = Color.White;
                    bCloseRed.BackColor = Color.White; ;
                    bCloseYellow.BackColor = Color.White;
                    bCloseBlue.BackColor = Color.Red;
                    break;
            }
        }

        /// <summary>
        /// Set number of files to control variables - the values before scaning.
        /// If scaning is successful and the numbers haven't changed some error appears.
        /// </summary>
        private void SetCtrlNumberOfFiles()
        {
            greenBoxNumOfFilesCtrl = greenBoxNumOfFiles;
            redBoxNumOfFilesCtrl = redBoxNumOfFiles;
            yellowBoxNumOfFilesCtrl = yellowBoxNumOfFiles;
            blueBoxNumOfFilesCtrl = blueBoxNumOfFiles;
        }

        /// <summary>
        /// Checks if error appears to solve bug.
        /// </summary>
        /// <returns></returns>
        private bool CheckCtrlNumberOfFiles()
        {
            if ((greenBoxNumOfFiles == greenBoxNumOfFilesCtrl 
                && redBoxNumOfFiles == redBoxNumOfFilesCtrl 
                && yellowBoxNumOfFiles == yellowBoxNumOfFilesCtrl
                && blueBoxNumOfFiles == blueBoxNumOfFilesCtrl) && lNotification.ForeColor.Equals(Color.Green))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Used for CTRL+A on QRCode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbQr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
            }
        }

        private void oJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DataUpdater("Organizaciona jedinica", "[QRCode].[dbo].[RWTABLE]", "[OrganizationalUnit]", "[QRID]", Helper.GetConnection()).ShowDialog();
        }
    }
}
