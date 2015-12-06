using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CounterApp
{
    public partial class Brojac : Form
    {
        private string filePath = null;
        public Brojac()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Counter c= new Counter(filePath);
            c.LoadTableData();
            c.Count();
            c.Write();
        }
    }
}
