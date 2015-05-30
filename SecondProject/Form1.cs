using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ExcelLibrary;
using ExcelLibrary.SpreadSheet;

namespace SecondProject
{
    public partial class Form1 : Form
    {
        private static string filePath = "";
        private Conversion conversion;
        public Form1()
        {
            InitializeComponent();
            this.CenterToParent();

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excell Files (*.xls)|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName.ToString();
            }

        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string archNumStr = txtArchNum.Text;
            int archNum = Int32.Parse(archNumStr);
            conversion = new Conversion(filePath,archNum);
            conversion.OpenInputAndOutputFiles();
            conversion.MainSort();
            conversion.Write();
        }
    }
}
