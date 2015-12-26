using ExcelLibrary.SpreadSheet;
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
    /// Class used for reading and changing order numbers.
    /// </summary>
    public partial class OrderNumDialog : Form
    {
        /// <summary>
        /// Path of input file.
        /// </summary>
        private string filePath = string.Empty;
        /// <summary>
        /// Workbook for input .xls file.
        /// </summary>
        private Workbook book;
        /// <summary>
        /// Worksheet for input .xls file.
        /// </summary>
        private Worksheet sheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNumDialog"/> class.
        /// </summary>
        public OrderNumDialog()
        {
            InitializeComponent();
        }

        private void bChooseClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName.ToString();
            }
            else
            {
                MessageBox.Show("Nije moguće otvoriti ulaznu tabelu, proverite da li je već otvorena!");
            }
        }

        private void bConfirmClick(object sender, EventArgs e)
        {
            // Try to open input table.
            book = new Workbook();
            try
            {
                book = Workbook.Load(filePath);
            }
            catch
            {
                MessageBox.Show("Uneti fajl ne moze da se otvori.\nProverite da li ste uneli ispravno ime.\nProverite da li je vec otvoren i ukoliko jeste zatvorite ga i pokusajte ponovo.");
                Environment.Exit(-1);
            }

            // Open first sheet in input xls document.
            try
            {
                sheet = book.Worksheets[0];
            }
            catch
            {
                MessageBox.Show("Uneti fajl ne moze da se otvori.\nUlazni fajl mora da sadrzi sve u prvom sheetu!");
                Environment.Exit(-1);
            }

            SqlConnection conn;
            SqlCommand command;

            try
            {
                conn = new SqlConnection(Helper.ConnectionString);
                conn.Open();
                command = new SqlCommand();
            }
            catch
            {
                MessageBox.Show("Nije moguće otvoriti konekciju prema bazi.");
                return;
            }
            try
            {
                // Iterate through  table and generate sql UPDATE STATEMENTS.
                for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
                {
                    Row row = new Row();
                    // Get current row.
                    row = sheet.Cells.GetRow(rowIndex);
                    // Take QRID from it.
                    string id = row.GetCell(0).StringValue;
                    // Take the new OrderNum from it.
                    string orderNum = row.GetCell(1).StringValue;

                    string commandText = "UPDATE [QRCode].[dbo].[BankTable] SET [OrderNum] = @orderNum WHERE [ID] = @id";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@orderNum", orderNum);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Uspješno izvršene izmjene.");

            }
            catch
            {
                MessageBox.Show("Nije moguće izvući podatke iz ulazne tabele, ili su pogrešno uneseni.");
                return;
            }

            conn.Close();
        }
    }
}
