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

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="name">Name of the logged user.</param>
        /// <param name="jmbg">Unique parameter of every user.</param>
        public MainForm(string name, string jmbg)
        {
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
            switch(Helper.DoctypeBoxCode[doctype])
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
        /// <returns>Indicator of success.</returns>
        private bool OpenOrCreateBox(string boxCode, BoxTypeEnum boxType)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT [TYPE] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader =  command.ExecuteReader();

            if (reader.HasRows)
            {

            }
            return true;
            conn.Close();
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
                lStatusGreen.Text = "Status: Zatvorena";
                ((Button)sender).Text = "Otvori";
                tbGreen.Clear();
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
                    errorProvider.SetError(tbGreen, string.Empty);
                    greenBoxOpened = true;
                    lStatusGreen.Text = "Status: Otvorena";
                    ((Button)sender).Text = "Zatvori";
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
                    errorProvider.SetError(tbRed, string.Empty);
                    redBoxOpened = true;
                    lStatusRed.Text = "Status: Otvorena";
                    ((Button)sender).Text = "Zatvori";
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
                    errorProvider.SetError(tbYellow, string.Empty);
                    yellowBoxOpened = true;
                    lStatusYellow.Text = "Status: Otvorena";
                    ((Button)sender).Text = "Zatvori";
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
                    errorProvider.SetError(tbBlue, string.Empty);
                    blueBoxOpened = true;
                    lStatusBlue.Text = "Status: Otvorena";
                    ((Button)sender).Text = "Zatvori";
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
