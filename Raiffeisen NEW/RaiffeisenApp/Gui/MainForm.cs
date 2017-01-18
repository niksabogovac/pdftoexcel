using BusinessLogic;
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

namespace Gui
{
   
    public partial class Kutija5 : Form
    {
        #region Private members

        private DatabaseManager _databaseManager = new DatabaseManager();

        /// <summary>
        /// Indicator if box is opened.
        /// </summary>
        private bool boxOpened = false;

        /// <summary>
        /// Regex for box codes.
        /// </summary>
        private Regex boxCodeRegex = new Regex(@"(RSRFBA)[0-9]{2}C-[0-9]{5}");

        /// <summary>
        /// Length of box code.
        /// </summary>
        private readonly int BOX_CODE_LENGTH = 15;

        /// <summary>
        /// Regex for 1d codes.
        /// </summary>
        private Regex simpleCodeRegex = new Regex(@"RQ[0-9]{8}");

        /// <summary>
        /// Regex for qr codes.
        /// </summary>
        private Regex qrCodeRegex = new Regex("{\"1\":\".*\",\"2\":\"RQ[0-9]{8}\",\"4\":\".*\",\"10\":\"RQ[0-9]{8}\"}");

        #endregion

        #region Constructors

        public Kutija5()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void SetError(string message)
        {
            lReport.Text = message;
            lReport.ForeColor = Color.Red;
        }

        private void SetMessage(string message)
        {
            lReport.Text = message;
            lReport.ForeColor = Color.Green;
        }

        private void UpdateNumberOfFiles(string boxCode)
        {
            lNumberOfFiles.Text = "Broj fajlova u kutiji: " + _databaseManager.GetNumberOfFileFromBox(boxCode).ToString();
        }

        /// <summary>
        /// Tries to insert partial code to database.
        /// </summary>
        /// <param name="code">ID of file.</param>
        /// <param name="orderNum">Current order number.</param>
        /// <param name="boxCode">Code of box that the file is put in.</param>
        private void TryInsertPartialCode(string code, string orderNum, string boxCode)
        {
            int errorNum;
            // Try to insert to database.
            if (_databaseManager.InsertNewPartialCode(code, orderNum, boxCode, out errorNum))
            {
                UpdateNumberOfFiles(tbBoxCode.Text);
                tbCode.Clear();
                SetMessage("Uspešno ste učitali kod!");
            }
            else
            {
                // Primary key violation.
                if (errorNum == 2627)
                {
                    SetError("Već je učitan kod!");
                }
                else
                {
                    SetError("Kod je neuspešno učitan!");
                }
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Triggered when something is entered in text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCodeEnter(object sender, EventArgs e)
        {
            if (tbOrderNum.Text.ToString() == string.Empty)
            {
                SetError("Morate uneti broj naloga!");
                tbOrderNum.Focus();
            }
            else if (!boxOpened)
            {
                SetError("Morate otvoriti kutiju!");
                tbBoxCode.Focus();
            }
            else
            {
                lReport.Text = string.Empty;
                tbCode.Text = string.Empty;
            }

            
        }

        private void bOpenCloseBoxClick(object sender, EventArgs e)
        {
            if (boxOpened)
            {
                List<string> codeWithoutFileNums = _databaseManager.GetCodeWithoutFileNumberByBox(tbBoxCode.Text);

                if (codeWithoutFileNums != null && codeWithoutFileNums.Count > 0)
                {
                    FileNumDialog dialog = new FileNumDialog(codeWithoutFileNums,_databaseManager);
                    dialog.ShowDialog();
                }

                if (cbCloseBox.Checked)
                {
                    boxOpened = false;
                    cbCloseBox.Enabled = false;
                    cbCloseBox.Checked = false;
                    bOpenCloseBox.Text = "Otvori kutiju";
                    tbBoxCode.Text = string.Empty;
                    tbBoxCode.Enabled = true;
                    SetMessage(string.Empty);
                    lNumberOfFiles.Text = "Broj fajlova u kutiji: ";
                }
            }
            else
            {
                string boxCode = tbBoxCode.Text.Trim(' ');
                // Check if box code is correct.
                if (!boxCodeRegex.IsMatch(boxCode) || tbBoxCode.Text.Length != BOX_CODE_LENGTH)
                {
                    SetError("Nije unet dobar broj kutije!");
                }
                else
                {
                    int boxType;
                    // Check if box already exists.
                    if (!_databaseManager.CheckIfType5BoxExists(boxCode, out boxType))
                    {
                        if (!_databaseManager.InsertNewBox(boxCode))
                        {
                            SetError("Ne može se dodati nova kutija!");
                        }
                        else
                        {
                            SetMessage("Uspešno dodata nova kutija!");
                            boxOpened = true;
                            cbCloseBox.Enabled = true;
                            cbCloseBox.Checked = false;
                            bOpenCloseBox.Text = "Unesi filenumber";
                            tbBoxCode.Enabled = false;
                        }
                    }
                    // Box exists check type
                    else if (boxType != (int)Enums.BoxTypeEnum.TIP5)
                    {
                        SetError("Otvorena je kutija pogrešnog tipa!");
                    }
                    else
                    {
                        UpdateNumberOfFiles(boxCode);
                        SetMessage("Uspešno otvorena kutija!");
                        boxOpened = true;
                        cbCloseBox.Enabled = true;
                        cbCloseBox.Checked = false;
                        bOpenCloseBox.Text = "Unesi filenumber";
                        tbBoxCode.Enabled = false;
                    }
                }
            }
        }

        private void tbCodeTextChanged(object sender, EventArgs e)
        {
            if (simpleCodeRegex.IsMatch(tbCode.Text) && tbCode.Text.Length < 11)
            {
                TryInsertPartialCode(tbBoxCode.Text, tbOrderNum.Text, tbBoxCode.Text);
            }
            else if (qrCodeRegex.IsMatch(tbCode.Text))
            {
                MatchCollection ids = simpleCodeRegex.Matches(tbCode.Text);
                if (ids.Count != 2)
                {
                    SetError("Format QRCodea nije odgovarajući!");
                }
                else if (ids[0].ToString() == ids[1].ToString())
                {
                    TryInsertPartialCode(ids[0].ToString(), tbOrderNum.Text, tbBoxCode.Text);
                }
                else
                {
                    SetError("Format QRCodea nije odgovarajući!");
                }

            }
        }

        private void izvestajiToolStripMenuItemClick(object sender, EventArgs e)
        {
            ReportDialog dialog = new ReportDialog(_databaseManager);
            dialog.ShowDialog();
        }

        #endregion

    }
}
