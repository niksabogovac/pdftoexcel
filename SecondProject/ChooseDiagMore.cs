using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecondProject
{
    public partial class ChooseDiagMore : Form
    {
        public int year;
        public int categoryNum;
        public int fileType;
        public int location;
        public int deadline;
        public int categoryName;
        public int fileNum;
        public DialogResult result = DialogResult.None;
        public ChooseDiagMore()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                year = Int32.Parse(tbYear.Text) - 1;
                categoryNum = Int32.Parse(tbCategoryNum.Text) - 1;
                fileType = Int32.Parse(tbFileType.Text) - 1;
                location = Int32.Parse(tbLocation.Text) - 1;
                deadline = Int32.Parse(tbDeadline.Text) - 1;
                categoryName = Int32.Parse(tbCategoryName.Text) - 1;
                fileNum = Int32.Parse(tbFileNumber.Text) - 1;
                result = DialogResult.OK;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Uneti podaci po kolonama nisu brojevi, proverite!");
            }
            finally
            {
                this.Dispose();
            }
            
        }
    }
}
