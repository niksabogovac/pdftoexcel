using QR_Code.Import;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public partial class DataUpdater : Form
    {
        private string filePath;
        private readonly string tableName;
        private readonly string columnToUpdate;
        private readonly string keyColumn;
        private readonly SqlConnection connection;

        public DataUpdater(string title, string tableName, string columnToUpdate, string keyColumn, SqlConnection connection)
        {
            this.Text = title;
            this.tableName = tableName;
            this.columnToUpdate = columnToUpdate;
            this.keyColumn = keyColumn;
            this.connection = connection;
            InitializeComponent();
        }

        private void bChooseClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName.ToString();
            }
        }

        private void bConfirmClick(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            Importer importer = new Importer();
            DbUpdater dbUpdater = new DbUpdater(connection);

            try
            {
                string commandText = $"UPDATE {tableName} SET {columnToUpdate} = @value WHERE {keyColumn} = @id";
                dbUpdater.StartTransacation();
                // Two column are in ID as is one and the value to be updated is second.
                foreach (var value in importer.ImportValues(filePath))
                {
                    if (dbUpdater.ExecuteUpdate(
                        commandText,
                        new List<Tuple<string, object>>()
                        {
                        Tuple.Create("@id", (object)value.Item1),
                        Tuple.Create("@value", (object)value.Item2),
                        }) != 1)
                    {
                        builder.AppendLine($"Neuspešno importovanje za ID {value.Item1} na vrednost {value.Item2}.");
                    }
                }

                string outputLog = builder.ToString();
                if (string.IsNullOrEmpty(outputLog))
                {
                    MessageBox.Show("Uspešno izvršene izmene.");
                    dbUpdater.CommitChanges();
                }
                else
                {
                    MessageBox.Show("Neuspešno izvršene izmene. Pogledajte log.");
                    File.WriteAllText("ImportData.txt", outputLog);
                    dbUpdater.RollBackChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neuspešno izvršene izmene. Pogledajte log.");
                File.WriteAllText("ImportData.txt", string.Join("\n",ex, builder.ToString()));
                dbUpdater.RollBackChanges();
            }
        }
    }
}
