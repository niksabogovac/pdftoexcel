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
    public partial class DeleteForm : Form
    {
        /// <summary>
        /// 1 - QRCODE.
        /// 2 - BOX.
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
                    label1.Text += "QR kod ID";
                    break;
                case 2:
                    label1.Text += "kutiju";
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(string.Empty))
            {
                switch(type)
                { 
                    case 1:
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

                                command.CommandText = "SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                command.Parameters.AddWithValue("@id",textBox1.Text);
                                string boxCode;
                                SqlDataReader reader = command.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    boxCode = (string)reader[0];
                                }
                                else
                                {
                                    MessageBox.Show("QR kod ne postoji u bazi.");
                                    return;
                                }
                                reader.Close();
                                command.Parameters.Clear();

                                // Execute two separate commands.
                                command.CommandText =
                                 "DELETE FROM [QRCode].[dbo].[BankTable] WHERE [ID] = @id";
                                command.Parameters.AddWithValue("@id", textBox1.Text);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                                command.CommandText =
                                 "DELETE FROM [QRCode].[dbo].[RWTable] WHERE [QRID] = @qrid";
                                command.Parameters.AddWithValue("@qrid", textBox1.Text);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                     

                                command.CommandText =
                                 "UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = (SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode) - 1  WHERE [Code] = @boxCode";
                                command.Parameters.AddWithValue("@boxCode", boxCode);
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
                            MessageBox.Show("Uspešno izbrisan QR kod : " + textBox1.Text);
                            textBox1.Text = string.Empty;
                        }
                    break;
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
                }          
            }
        }
    }
}
