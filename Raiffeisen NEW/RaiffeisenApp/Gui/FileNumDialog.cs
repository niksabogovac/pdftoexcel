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
    public partial class FileNumDialog : Form
    {
        /// <summary>
        /// Reference to databaseManager.
        /// </summary>
        private DatabaseManager _databaseManager;
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

        private Regex reg = new Regex(@"(RSRFBA)[0-9]{2}\-[0-9]{6}");

        public FileNumDialog()
        {
            InitializeComponent();
        }

        public FileNumDialog(List<string> idCodes, DatabaseManager databaseManager)
        {
            InitializeComponent();
            _databaseManager = databaseManager;

            _codeNums = _databaseManager.CalculateNumberOfCodes(idCodes.Count);
            _idCodes = idCodes;
            lText.Text = "Preostali broj kodova za unos: " + _codeNums;
        }

        private void tbCodeTextChanged(object sender, EventArgs e)
        {
            if (reg.IsMatch(tbCode.Text) && tbCode.Text.Length == 15)
            {
                if (!_databaseManager.CheckPreviousFileNumberCodes(tbCode.Text) && !_currentFileNums.Contains(tbCode.Text))
                {
                    _currentFileNums.Add(tbCode.Text);
                    lText.Text = "Preostali broj kodova za unos: " + --_codeNums;
                    tbCode.Clear();
                    if (_codeNums > 0)
                    {
                        tbCode.Focus();
                    }
                    else
                    {
                        if (!_databaseManager.AddFileNumbersToPartialCode(_currentFileNums, _idCodes))
                        {
                            MessageBox.Show("Desila se greška, pokušajte ponovo!");
                        }
                        Dispose();
                    }
                }
            }
        }
    }
}
