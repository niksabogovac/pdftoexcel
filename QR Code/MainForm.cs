using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        

        #region Event handlers
        /// <summary>
        /// Triggered when QR Code is scanned or manually entered.
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
                // Get QR Code.
                string code = tbQr.Text;
                // ID of scanned code.
                string id = null;

                // Separate clien infos and remove ".
                string[] stringSeparators = new string[] { "," };
                string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                foreach (string clientInfo in tokens)
                {
                    Regex.Replace(clientInfo, "\"", string.Empty);
                    string[] tmpSeparator = new string[] { ":" };
                    string[] tmpTokens = clientInfo.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpTokens[0].Equals("id"))
                    {
                        // Get id.
                        id = tmpTokens[1];
                        break;
                    }
                }


                
            }

        }

        #endregion
        #endregion
    }
}
