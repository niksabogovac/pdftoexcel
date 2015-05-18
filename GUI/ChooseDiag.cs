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
    public partial class ChooseDiag : Form
    {
        public int DescriptionMainColumn = 0;
        public int ContainerCodeColumn = 0;
        public int YearColumn = 0;
        public ChooseDiag()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DescriptionMainColumn = Int32.Parse(tbDescription.Text.ToString());
            ContainerCodeColumn = Int32.Parse(tbContainerCode.Text.ToString());
            YearColumn = Int32.Parse(tbYear.Text.ToString());
        }
    }
}
