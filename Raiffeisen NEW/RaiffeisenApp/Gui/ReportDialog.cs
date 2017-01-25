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
    public partial class ReportDialog : Form
    {
        /// <summary>
        /// Regex for box codes.
        /// </summary>
        private Regex boxCodeRegex = new Regex(@"(RSRFBA)[0-9]{2}C-[0-9]{5}");

        private readonly int BOX_CODE_LENGTH = 15;

        public ReportDialog()
        {
            InitializeComponent();
        }

        private void button1Click(object sender, EventArgs e)
        {
            
            if (rbBoxCode.Checked && !rbOrderNum.Checked)
            {
                if (boxCodeRegex.IsMatch(tbValue.Text) && tbValue.Text.Length == BOX_CODE_LENGTH)
                {
                    if (ReportManager.GenerateReport(tbValue.Text, null))
                    {
                        MessageBox.Show("Uspešno generisan izveštaj!");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešno generisan izveštaj!");
                    }
                }
                else
                {
                    MessageBox.Show("Nije unet dobar kod kutije!");
                }
            }
            else if (!rbBoxCode.Checked && rbOrderNum.Checked)
            {
                int tmp;
                if (Int32.TryParse(tbValue.Text,out tmp))
                {
                    if (ReportManager.GenerateReport(null,tbValue.Text))
                    {
                        MessageBox.Show("Uspešno generisan izveštaj!");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešno generisan izveštaj!");
                    }
                }
                else
                {
                    MessageBox.Show("Nije unet dobar file number!");
                }
            }
            else
            {
                MessageBox.Show("Izaberite tip izveštaja!");
            }
        }
    }
}
