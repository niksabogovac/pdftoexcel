using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    public partial class DeleteForm : Form
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
        /// 1 - QRCODE.
        /// 2 - BOX.
        /// 3 - QRCODE SINGLE.
        /// </summary>
        private int type;
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteForm"/> class.
        /// </summary>
        /// <param name="type">Whether code or box is deleted</param>
        public DeleteForm(int type)
        {
            InitializeComponent();
            this.type = type;
            switch(type)
            {
                case 1:
                    label1.Text += "QR kod fajl";
                    bFile.Visible = true;
                    textBox1.Visible = false;
                    break;
                case 2:
                    label1.Text += "kutiju";
                    bFile.Visible = false;
                    textBox1.Visible = true;
                    break;
                case 3:
                    label1.Text += "QR kod";
                    bFile.Visible = false;
                    textBox1.Visible = true;
                    break;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( (!textBox1.Text.Equals(string.Empty) && (type == 2 || type == 3)) || (type == 1 && filePath != string.Empty))
            {
                switch(type)
                {
                    #region Case 1 QR From table
                    case 1:

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
                        SqlCommand cmd;

                        try
                        {
                            conn = new SqlConnection(Helper.ConnectionString);
                            conn.Open();
                            cmd = new SqlCommand();
                        }
                        catch
                        {
                            MessageBox.Show("Nije moguće otvoriti konekciju prema bazi.");
                            return;
                        }

                        try
                        {
                            // Iterate through  table and generate sql  STATEMENTS.
                            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
                            {

                                Row row = new Row();
                                // Get current row.
                                row = sheet.Cells.GetRow(rowIndex);
                                // Take QRCode from it.
                                string QRCode = row.GetCell(0).StringValue;
                                string id = handleCode(QRCode);

                                SqlTransaction sqlTran = conn.BeginTransaction();
                                try
                                {
                                    string cmdText = "SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                    cmd = new SqlCommand(cmdText, conn);
                                    cmd.Transaction = sqlTran;
                                    cmd.Parameters.AddWithValue("@id", id);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    string boxCode = null; 
                                       
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        boxCode = (string)reader["BoxCode"];
                                        reader.Close();
                                        reader.Dispose();
                                        reader = null;
                                        SqlCommand command;
                                        // Execute command delete
                                        cmdText = "DELETE FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                        command = new SqlCommand(cmdText, conn);
                                        command.Transaction = sqlTran;
                                        command.Parameters.AddWithValue("@id", id);
                                        command.ExecuteNonQuery();
                                        command.Parameters.Clear();
                                        
                                        // DELETE QRCodes from RW tables.
                                        command.CommandText = "DELETE FROM [QRCode].[dbo].[RWTable] WHERE [QRID] = @id";
                                        command.Parameters.AddWithValue("@id", id);
                                        command.ExecuteNonQuery();
                                        command.Parameters.Clear();

                                        // Execute update for number of files in box
                                        cmdText = "UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = (SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode) - 1  WHERE [Code] = @boxCode";
                                        command = new SqlCommand(cmdText, conn);
                                        command.Transaction = sqlTran;
                                        command.Parameters.AddWithValue("@boxCode", boxCode);
                                        command.ExecuteNonQuery();
                                        sqlTran.Commit();
                                        sqlTran.Dispose();
                                        sqlTran = null;
                                    }
                                    if ((reader != null) && !(reader.IsClosed))
                                    {
                                        reader.Close();
                                        reader.Dispose();
                                        reader = null;
                                    }

                                    if (sqlTran != null)
                                    {
                                        sqlTran.Commit();
                                        sqlTran.Dispose();
                                        sqlTran = null;
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        string nesto = ex.StackTrace;
                                        // Attempt to roll back the transaction.
                                        sqlTran.Rollback();
                                    }
                                    catch (Exception exRollback)
                                    {
                                        MessageBox.Show("Nije uspešno probajte ponovo!");
                                    }
                                }
                                
                            }
                            MessageBox.Show("Uspešno izvršena brisanja.");
                        }
                        catch (Exception h)
                        {
                            Console.WriteLine(h.StackTrace);
                            MessageBox.Show("Nije moguće izvući podatke iz ulazne tabele, ili su pogrešno uneseni.");
                            return;
                        }

                        break;
                    #endregion

                    #region Case 2 Box
                    case 2:
                    {
                        using (SqlConnection connection1 = new SqlConnection(Helper.ConnectionString))
                        {
                            connection1.Open();

                            // Start a local transaction.
                            SqlTransaction sqlTran = connection1.BeginTransaction();

                            // Enlist a command in the current transaction.
                            SqlCommand command = connection1.CreateCommand();
                            command.Transaction = sqlTran;

                            try
                            {

                                command.CommandText = "SELECT * FROM [QRCode].[dbo].[Box] WHERE [Code] = @code";
                                command.Parameters.AddWithValue("@code", textBox1.Text);
                                SqlDataReader reader = command.ExecuteReader();
                                if (reader.HasRows)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("Kutija ne postoji u bazi.");
                                    return;
                                }
                                reader.Close();
                                command.Parameters.Clear();

                                // Execute two separate commands.
                                command.CommandText =
                                 "DELETE FROM [QRCode].[dbo].[BankTable] WHERE [BoxCode] = @code";
                                command.Parameters.AddWithValue("@code", textBox1.Text);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                                command.CommandText =
                                 "DELETE FROM [QRCode].[dbo].[RWTable] WHERE [BoxCode] = @code";
                                command.Parameters.AddWithValue("@code", textBox1.Text);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();



                                command.CommandText =
                                 "DELETE FROM [QRCode].[dbo].[Box] WHERE [Code] = @code";
                                command.Parameters.AddWithValue("@code", textBox1.Text);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                // Commit the transaction.
                                sqlTran.Commit();

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    // Attempt to roll back the transaction.
                                    sqlTran.Rollback();
                                }
                                catch (Exception exRollback)
                                {
                                    MessageBox.Show("Nije uspešno probajte ponovo!");
                                }
                            }
                            MessageBox.Show("Uspešno izbrisana kutija : " + textBox1.Text);
                            textBox1.Text = string.Empty;
                        }
                    }
                    break;
                    #endregion

                    #region Case 3 QR single
                    case 3:
                    {
                        using (SqlConnection connection1 = new SqlConnection(Helper.ConnectionString))
                        {
                            connection1.Open();

                            // Start a local transaction.
                            SqlTransaction sqlTran = connection1.BeginTransaction();

                            // Enlist a command in the current transaction.
                            SqlCommand command1 = connection1.CreateCommand();
                            command1.Transaction = sqlTran;
                            try
                            {
                                string id = handleCode(textBox1.Text);
                                string cmdText = "SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                command1 = new SqlCommand(cmdText, connection1);
                                command1.Parameters.AddWithValue("@id", id);
                                command1.Transaction = sqlTran;
                                SqlDataReader reader = command1.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    string boxCode = (string)reader["BoxCode"];
                                    reader.Close();
                                    reader.Dispose();
                                    reader = null;
                                    SqlCommand command;
                                    // Execute command delete
                                    cmdText = "DELETE FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                    command = new SqlCommand(cmdText, connection1);
                                    command.Transaction = sqlTran;
                                    command.Parameters.AddWithValue("@id", id);
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();

                                    // DELETE QRCodes from RW tables.
                                    command.CommandText = "DELETE FROM [QRCode].[dbo].[RWTable] WHERE [QRID] = @id";
                                    command.Parameters.AddWithValue("@id", id);
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();

                                    // Execute update for number of files in box
                                    cmdText = "UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = (SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode) - 1  WHERE [Code] = @boxCode";
                                    command = new SqlCommand(cmdText, connection1);
                                    command.Transaction = sqlTran;
                                    command.Parameters.AddWithValue("@boxCode", boxCode);
                                    command.ExecuteNonQuery();
                                    sqlTran.Commit();
                                    sqlTran.Dispose();
                                    sqlTran = null;
                                }
                                else
                                {
                                    MessageBox.Show("Kod sa ID: " + id+ " ne postoji u bazi.");
                                    return;
                                }
                                command1.Parameters.Clear();


                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    // Attempt to roll back the transaction.
                                    sqlTran.Rollback();
                                }
                                catch (Exception exRollback)
                                {
                                    MessageBox.Show("Nije uspešno probajte ponovo!");
                                }
                            }
                            MessageBox.Show("Uspešno izbrisan QR kod: " + textBox1.Text);
                            textBox1.Text = string.Empty;

                        }

                    }
                    break;
                    #endregion
                }          
            }
        }

        private void bFile_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Splits QRCode returning ID from it.
        /// </summary>
        /// <param name="QRCode">Input QRCode</param>
        /// <returns>Output ID.</returns>
        private string handleCode(string QRCode)
        {
            string id = null;
            string code = QRCode;
            code = Regex.Replace(code, @"\\000021", string.Empty);
            code = Regex.Replace(code, "{", string.Empty);
            code = Regex.Replace(code, "}", string.Empty);
            code = Regex.Replace(code, " ", string.Empty);
            // Separate client infos and remove ".
            string[] stringSeparators = new string[] { "," };
            string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string clientInfo in tokens)
            {
                string tmp = Regex.Replace(clientInfo, "\"", string.Empty);
                string[] tmpSeparator = new string[] { ":" };
                string[] tmpTokens = tmp.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (tmpTokens.Length > 1)
                {

                    if (tmpTokens[0].Equals("id"))
                    {
                        // Get id.
                        id = tmpTokens[1];
                    }
                    
                }

            }
            return id;
        }
    }
}
