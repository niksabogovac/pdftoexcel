using BusinessLogic;
using Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Gui
{
    public partial class FileNumDialog : Form
    {
        #region Private members

        /// <summary>
        /// Number of codes to add fileNumber.
        /// </summary>
        private int _codeNums;

        /// <summary>
        /// List of codes to be add fileNumber.
        /// </summary>
        private List<string> _idCodes;

        /// <summary>
        /// List of fileNumers that are currently being added.
        /// </summary>
        private List<string> _currentFileNums = new List<string>();

        /// <summary>
        /// Regex for file numbers.
        /// </summary>
        private Regex regFileNum = new Regex(QRRegex.FileNumber);

        /// <summary>
        /// Organizational unit for associated with current codes (and file numbers).
        /// </summary>
        private string organizationalUnit;

        /// <summary>
        /// Lenegth of organizational unit.
        /// </summary>
        private const short orgUnitLength = 5;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNumDialog"/> class.
        /// </summary>
        public FileNumDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNumDialog"/> class.
        /// </summary>
        /// <param name="idCodes">List of scaned codes that don't have a file number associated.</param>
        public FileNumDialog(List<string> idCodes)
        {
            InitializeComponent();

            _codeNums = DataParser.CalculateNumberOfCodes(idCodes.Count);
            _idCodes = idCodes;
            lTextFileNumber.Text = "Preostali broj kodova za unos: " + _codeNums;
        }

        #endregion


        /// <summary>
        /// Method that is invoked when new file number code is scanned.
        /// </summary>
        /// <param name="sender">Invoking object.</param>
        /// <param name="e">Following args.</param>
        private void tbFileNumCodeTextChanged(object sender, EventArgs e)
        {
            if (regFileNum.IsMatch(tbFileNumCode.Text) && tbFileNumCode.Text.Length == QRRegex.FileNumberLength)
            {
                if (!DatabaseManager.CheckPreviousFileNumberCodes(tbFileNumCode.Text) && !_currentFileNums.Contains(tbFileNumCode.Text))
                {
                    _currentFileNums.Add(tbFileNumCode.Text);
                    lTextFileNumber.Text = "Preostali broj kodova za unos: " + --_codeNums;
                    tbFileNumCode.Clear();
                    if (_codeNums > 0)
                    {
                        tbFileNumCode.Focus();
                    }
                    else
                    {
                        if (!DatabaseManager.AddFileNumbersToPartialCode(_currentFileNums, _idCodes, organizationalUnit))
                        {
                            MessageBox.Show("Desila se greška, pokušajte ponovo!");
                        }
                        Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Method that is invoked when text box for entering file numbers gains focus.
        /// Checks if organizational unit is added.
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
        /// Method that is invoked when text box for entering organizational units is changed.
        /// </summary>
        /// <param name="sender">Invoking object.</param>
        /// <param name="e">Following args.</param>
        private void tbOrgUnitTextChanged(object sender, EventArgs e)
        {
            organizationalUnit = tbOrgUnit.Text;
        }
    }
}
