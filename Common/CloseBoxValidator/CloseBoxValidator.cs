using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Common
{
    public partial class CloseBoxValidator : Form
    {
        /// <summary>
        /// Remaining codes to be validated.
        /// </summary>
        private ISet<string> remainingCodes;

        /// <summary>
        /// Codes that are validated.
        /// </summary>
        private ISet<string> validatedCodes;

        /// <summary>
        /// Regex for file numbers.
        /// </summary>
        private Regex regFileNum = new Regex(QRRegex.FileNumber);

        public CloseBoxValidator(ISet<string> codesToBeValidated)
        {
            this.remainingCodes = codesToBeValidated;
            validatedCodes = new HashSet<string>();
            InitializeComponent();
            AppendRemainingCodesText(remainingCodes.Count);
        }



        private void TbFileNumCodeTextChanged(object sender, System.EventArgs e)
        {
            if (ValidateFileNumber(tbFileNumCode.Text))
            {
                string validCode = tbFileNumCode.Text;
                // Already validated.
                if (validatedCodes.Contains(validCode))
                {
                    ShowErrorMessage($"File Number {validCode} je već kontrolisan.", Color.Red);
                    tbFileNumCode.Text = string.Empty;
                    return;
                }
                
                // Regular validation.
                if (remainingCodes.Contains(validCode))
                {
                    remainingCodes.Remove(validCode);
                    validatedCodes.Add(validCode);
                    AppendRemainingCodesText(remainingCodes.Count);
                    ShowErrorMessage(string.Empty, Color.Black);
                    if (remainingCodes.Count == 0)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    
                }
                // Code is not validated and should not be validated.
                else
                {
                    ShowErrorMessage($"File Number {validCode} ne pripada ovoj kutiji.", Color.Red);
                    tbFileNumCode.Text = string.Empty;
                }
            
            }
        }


        private bool ValidateFileNumber(string text) => regFileNum.IsMatch(text) && text.Length == QRRegex.FileNumberLength;


        private void AppendRemainingCodesText(int numRemainingCodes)
        {
            lTextFileNumber.Text = $"Broj preostalih kodova:  {numRemainingCodes}.";
            tbFileNumCode.Text = string.Empty;
        }

        private void CloseBoxValidatorShown(object sender, System.EventArgs e)
        {
            if (remainingCodes.Count == 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ShowErrorMessage(string text, Color color)
        {
            lErrorMessage.Text = text;
            lErrorMessage.ForeColor = color;
        }
    }
}
