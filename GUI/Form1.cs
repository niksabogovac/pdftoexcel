using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        private static string filePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName.ToString();
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (cbSpec.SelectedIndex == 0)
            {
                Specifikacija4 spec = new Specifikacija4();
                spec.setPath(filePath);
                spec.initialize();
                spec.convert();
                spec.write();
            } else
            {
                Specifikacija5 spec = new Specifikacija5();
                spec.setPath(filePath);
                spec.initialize();
                spec.convert();
                spec.write();
            }
        }
    }
}
