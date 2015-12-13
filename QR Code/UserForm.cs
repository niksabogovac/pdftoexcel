using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    /// <summary>
    /// Class used for controling users.
    /// </summary>
    public partial class UserForm : Form
    {
        /// <summary>
        /// Adapter used for data grid view component.
        /// </summary>
        private SqlDataAdapter dataAdapter;

        public UserForm()
        {
            InitializeComponent();
            bindingSource1 = new BindingSource();
            dataAdapter = new SqlDataAdapter();
        }

        /// <summary>
        /// Fills datagridview from data from database.
        /// </summary>
        private void GetData()
        {
            string selectCommand = "SELECT * FROM [QRCode].[dbo].[Users]";
            try
            {
                SqlConnection conn = new SqlConnection(Helper.ConnectionString);
                dataAdapter = new SqlDataAdapter(selectCommand, conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;
            }
            catch (Exception)
            {
                MessageBox.Show("NESTO NE VALJA");
            }
        }

        /// <summary>
        /// When row data is updated update database.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Following args.</param>
        private void DataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                bindingSource1.EndEdit();
                dataAdapter.Update((DataTable)bindingSource1.DataSource);
                Helper.UpdateDocTypesFromDatabase();
            }
            catch (SqlException)
            {
                MessageBox.Show("Nije moguće dodati korisnika, proverite da li je već dodat.");
            }
        }

        /// <summary>
        /// Attaches datasource to datagridview when dialog is shown.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Following args.</param>
        private void DoctypeDialog_Shown(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;
            GetData();
        }

    }
}
