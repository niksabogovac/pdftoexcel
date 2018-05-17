using QR_Code.Import;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QR_Code
{
    public partial class OrganizationalUnit : Form
    {
        private string filePath;

        private const string sqlUpdateCommand = "UPDATE [QRCode].[dbo].[RWTABLE] SET [OrganizationalUnit] = @orgUnit WHERE [QRID] = @id";

        public OrganizationalUnit()
        {
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
            DbUpdater dbUpdater = new DbUpdater(Helper.GetConnection());

            try
            {
                dbUpdater.StartTransacation();
                foreach (var value in importer.ImportValues(filePath))
                {
                    if (dbUpdater.ExecuteUpdate(
                        sqlUpdateCommand,
                        new List<Tuple<string, object>>()
                        {
                        Tuple.Create("@id", (object)value.Item1),
                        Tuple.Create("@orgUnit", (object)value.Item2),
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
                    File.WriteAllText("ImportOrganizationalUnit.txt", outputLog);
                    dbUpdater.RollBackChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neuspešno izvršene izmene. Pogledajte log.");
                File.WriteAllText("ImportOrganizationalUnit.txt", string.Join("\n",ex, builder.ToString()));
                dbUpdater.RollBackChanges();
            }
        }
    }
}
