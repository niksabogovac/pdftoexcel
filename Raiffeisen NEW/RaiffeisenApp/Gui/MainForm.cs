using BusinessLogic;
using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Gui
{

    public partial class Kutija5 : Form
    {
        #region Private members

        /// <summary>
        /// Unique identifier of current user.
        /// </summary>
        private string _jmbg;

        /// <summary>
        /// Indicator if box is opened.
        /// </summary>
        private bool boxOpened = false;

        /// <summary>
        /// Regex for box codes.
        /// </summary>
        private Regex boxCodeRegex = new Regex(@"(RSRFBA)[0-9]{2}C-[0-9]{5,6}");

        /// <summary>
        /// Length of box code.
        /// </summary>
        private readonly int BOX_CODE_LENGTH = 16;

        /// <summary>
        /// Regex for 1d codes.
        /// </summary>
        private Regex simpleCodeRegex = new Regex(@"RQ[0-9]{8}[0-9]?");

        /// <summary>
        /// Regex for qr codes.
        /// </summary>
        private Regex qrCodeRegex = new Regex("{\"1\":\".*\",\"2\":\".*\",\"4\":\".*\",\"10\":\".*\"}");

        #endregion

        #region Constructors


        public Kutija5(string jmbg)
        {
            InitializeComponent();
            _jmbg = jmbg;
        }

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
            lNumberOfFiles.Text = "Broj fajlova u kutiji: " + DatabaseManager.GetNumberOfFileFromBox(boxCode).ToString();
        }

        /// <summary>
        /// Tries to insert partial code to database.
        /// </summary>
        /// <param name="code">ID of file.</param>
        /// <param name="orderNum">Current order number.</param>
        /// <param name="boxCode">Code of box that the file is put in.</param>
        private void TryInsertPartialCode(string code, string orderNum, string boxCode)
        {
            string errorNum;
            // Try to insert to database.
            if (DatabaseManager.InsertNewPartialCode(code, orderNum, boxCode,DateTime.Now, out errorNum))
            {
                UpdateNumberOfFiles(tbBoxCode.Text);
                tbCode.Clear();
                SetMessage("Uspešno ste učitali kod!");
            }
            else
            {
                // Primary key violation.
                if (errorNum == "2627")
                {
                    string errorMsg = "Već je učitan kod!\n";
                    // If a code has already been scaned, try to get its box code.
                    string existingBoxCode;
                    if (DatabaseManager.TryGetBoxCodeForID(code, out existingBoxCode))
                    {
                        errorMsg += $"Nalazi se u kutiji:\n {existingBoxCode}.";
                    }
                    else
                    {
                        errorMsg += "Nije moguće odrediti u kojoj kutiji.";
                    }

                    SetError(errorMsg);
                }
                else
                {
                    SetError("Kod je neuspešno učitan!Kod greške: " + errorNum);
                }
                tbCode.Clear();
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
                List<string> codeWithoutFileNums = DatabaseManager.GetCodeWithoutFileNumberByBox(tbBoxCode.Text);

                if (codeWithoutFileNums != null && codeWithoutFileNums.Count > 0)
                {
                    FileNumDialog dialog = new FileNumDialog(codeWithoutFileNums);
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
                // New rule is that the box code can be 15 or 16 characters long.
                if (!boxCodeRegex.IsMatch(boxCode) || (tbBoxCode.Text.Length != BOX_CODE_LENGTH && tbBoxCode.Text.Length != BOX_CODE_LENGTH -1))
                {
                    SetError("Nije unet dobar broj kutije!");
                }
                else
                {
                    int boxType;
                    // Check if box already exists.
                    if (!DatabaseManager.CheckIfType5BoxExists(boxCode, out boxType))
                    {
                        if (!DatabaseManager.InsertNewBox(boxCode))
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
            if (simpleCodeRegex.IsMatch(tbCode.Text) && tbCode.Text.Length <= 11 )
            {
                TryInsertPartialCode(tbCode.Text, tbOrderNum.Text, tbBoxCode.Text);
            }
			// Try parse QRCode.
            else if (DataParser.TryParseJson(tbCode.Text))
            {
                TryInsertPartialCode(DataParser.GetIdFromQrCode(tbCode.Text), tbOrderNum.Text, tbBoxCode.Text);
            }
        }

        private void reportToolStripClicked(object sender, EventArgs e)
        {
            ReportDialog dialog = new ReportDialog();
            dialog.ShowDialog();
        }

        private void importToolStripClicked(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Excel Files (*.xls)|*.xls";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (ReportManager.ImportData(diag.FileName,_jmbg))
                {
                    MessageBox.Show("Importovanje podataka završeno!");
                }
                else
                {
                    MessageBox.Show("Neuspešno importovanje podataka! Proverite da li je fajl otvoren!");
                }
                
            }
        }

        private void tsbOrgUnitClick(object sender, EventArgs e)
        {
            new DataUpdater("Organizaciona jedinica", "[QRCode].[dbo].[PartCodes]", "[OrganizationalUnit]", "[ID]", DatabaseManager.SqlConnection).ShowDialog();
        }

        #endregion


    }
}
