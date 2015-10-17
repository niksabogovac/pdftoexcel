using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    public partial class MainForm : Form
    {
        private string Jmbg;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string _name, string _jmbg)
        {
            InitializeComponent();
            lWorker.Text += _name;
            Jmbg = _jmbg;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fMain_Load(object sender, EventArgs e)
        {

        }
    }
}
