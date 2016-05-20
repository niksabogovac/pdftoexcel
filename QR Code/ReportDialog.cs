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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    public partial class ReportDialog : Form
    {
        public ReportDialog()
        {
            InitializeComponent();
            
        }

        private void BReport_Click(object sender, EventArgs e)
        {
            // Is order number provided.
            if (tbOrderNumber.Text.Equals(string.Empty) && (cbChoose.SelectedIndex == 0 || cbChoose.SelectedIndex == 2))
            {
                MessageBox.Show("Unesite broj naloga.");
            }
            else
            {
                // Check what is selected.
                switch(cbChoose.SelectedIndex)
                {
                    case 0:
                        GenerateReports(null, null);
                        break;
                    case 1:
                        GenerateReports(dateTimeFrom.Value, dateTimeUntil.Value);
                        break;
                    case 2:
                        GenerateReports(null, null);
                        break;
                    default:
                        break;
                }
                
            }
            
        }

        /// <summary>
        /// Triggered when detailed report creation from combo box is selected.
        /// </summary>
        /// <param name="start">Start date if provided.</param>
        /// <param name="end">End date if provided.</param>
        private void DetailedReport(DateTime? start, DateTime? end)
        {
            string outPath = string.Empty;
            if (start == null && end == null)
            {
                outPath += Application.StartupPath + @"\tabelaZaBankuDetaljna " + tbOrderNumber.Text + ".xls";
            } 
            else
            {
                outPath += Application.StartupPath + @"\tabelaZaBankuDetaljna " + start.Value.Date.ToString("dd.MM.yyyy.") + " - " + end.Value.Date.ToString("dd.MM.yyyy.") + ".xls";
            }
            
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Sheet1");
            Workbook outputBook = new Workbook();


            // Set it to beginning of the document
            int curRow = 0;
            int curCol = 0;

            outputSheet.Cells[curRow, curCol++] = new Cell("Broj naloga/primopredaje");
            outputSheet.Cells[curRow, curCol++] = new Cell("Vrsta kutije");
            outputSheet.Cells[curRow, curCol++] = new Cell("Broj fajlova u kutiji");
            outputSheet.Cells[curRow, curCol++] = new Cell("File number");
            outputSheet.Cells[curRow, curCol++] = new Cell("Šifra kutije");
            outputSheet.Cells[curRow, curCol++] = new Cell("Doctype");
            outputSheet.Cells[curRow, curCol++] = new Cell("ID");
            outputSheet.Cells[curRow, curCol++] = new Cell("Mbr");
            outputSheet.Cells[curRow, curCol++] = new Cell("Partija");
            outputSheet.Cells[curRow, curCol++] = new Cell("Zahtev");
            outputSheet.Cells[curRow, curCol++] = new Cell("ID kartice");
            outputSheet.Cells[curRow, curCol++] = new Cell("Mbrid");
            outputSheet.Cells[curRow++, curCol++] = new Cell("Paket");
            curCol = 0;

            SqlConnection conn = Helper.GetConnection();
            SqlCommand command;
            string commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] on [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] WHERE ";
            
            // Used for counting number of files for progress bar.
            string countCommandText = "SELECT count([QRCode].[dbo].[BankTable].[ID]) FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] on [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] WHERE ";

            if (start == null && end == null)
            {
                // Do by order num.
                if (cbChoose.SelectedIndex == 0)
                {
                    commandText += "[OrderNum] = @orderNum";
                    countCommandText += "[OrderNum] = @orderNum";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
                } 
                // Do by box Code.
                else if (cbChoose.SelectedIndex == 2)
                {
                    commandText += "[QRCode].[dbo].[BankTable].[BoxCode] = @boxCode";
                    countCommandText += "[QRCode].[dbo].[BankTable].[BoxCode] = @boxCode";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@boxCode", tbOrderNumber.Text);
                }
                // Error.
                else
                {
                    return;
                }
            }
            else
            {
                if (start.Value.Date.Equals(end.Value.Date))
                {
                    commandText += "CAST([Date] as Date) LIKE CAST(@date as Date)";
                    countCommandText += "CAST([Date] as Date) LIKE CAST(@date as Date)";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("date", start.Value);
                }
                else
                {
                    commandText += "[Date] BETWEEN @startDate AND @stopDate";
                    countCommandText += "[Date] BETWEEN @startDate AND @stopDate";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@startDate", start.Value);
                    command.Parameters.AddWithValue("@stopDate", end.Value);
                }
                
            }

            command.CommandText = countCommandText;
            int totalRows = (int)command.ExecuteScalar();

            progressBar1.Value = 0;
            progressBar1.Maximum = totalRows;
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        progressBar1.Value++;
                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["OrderNum"]);
                        string boxCode = (string)reader["BoxCode"];
                        int boxType = GetTypeFromBoxCode(boxCode);
                        switch (boxType)
                        {
                            case 86:
                                outputSheet.Cells[curRow, curCol++] = new Cell("POZAJMICE");
                                break;
                            case 148:
                                outputSheet.Cells[curRow, curCol++] = new Cell("KREDITI");
                                break;
                            case 82:
                                outputSheet.Cells[curRow, curCol++] = new Cell("RAČUNI");
                                break;
                            case 83:
                                outputSheet.Cells[curRow, curCol++] = new Cell("OROČENJA");
                                break;
                            default:
                                break;

                        }
                        outputSheet.Cells[curRow, curCol++] = new Cell(GetNumberOfFilesFromBox(boxCode).ToString());
                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["Code"]);
                        outputSheet.Cells[curRow, curCol++] = new Cell(boxCode);
                        string code = (string)reader["QRCode"];
                        HandleCodeAndWrite(code, outputSheet, ref curRow, ref curCol);
                    }
                }
            }
            

            
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran detaljan izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati detaljan izveštaj!");
            }
        }

        /// <summary>
        /// Triggered when report creation from combo box is selected.
        /// </summary>
        /// <param name="start">Start date if provided.</param>
        /// <param name="end">End date if provided.</param>
        private void GeneralReport(DateTime? start, DateTime? end)
        {
            string outPath = null;
            if (start == null && end == null)
            {
                outPath += Application.StartupPath + @"\tabelaZaBanku " + tbOrderNumber.Text + ".xls";
            }
            else
            {
                outPath += Application.StartupPath + @"\tabelaZaBanku " + start.Value.Date.ToString("dd.MM.yyyy.") + " - " + end.Value.Date.ToString("dd.MM.yyyy.") + ".xls";
            }
            
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Sheet1");
            Workbook outputBook = new Workbook();

            // Set it to beginning of the document
            int curRow = 0;
            int curCol = 0;

            outputSheet.Cells[curRow, curCol++] = new Cell("Unique ID");
            outputSheet.Cells[curRow, curCol++] = new Cell("Broj naloga/primopredaje");
            outputSheet.Cells[curRow, curCol++] = new Cell("Šifra kutije");
            outputSheet.Cells[curRow, curCol++] = new Cell("Datum očitavanja");
            outputSheet.Cells[curRow, curCol++] = new Cell("Operater");
            outputSheet.Cells[curRow++, curCol++] = new Cell("Sadržaj QR koda");
            curCol = 0;

            SqlConnection conn = Helper.GetConnection();
            SqlCommand command;


            string commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] WHERE ";
            // Used for counting for progress bar.
            string countCommandText = "SELECT count([Id]) FROM [QRCode].[dbo].[BankTable] WHERE ";
            if (start == null && end == null)
            {
                // Do by order num.
                if (cbChoose.SelectedIndex == 0)
                {
                    commandText += "[OrderNum] = @orderNum";
                    countCommandText += "[OrderNum] = @orderNum";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
                }
                // Do by box Code.
                else if (cbChoose.SelectedIndex == 2)
                {
                    commandText += "[BoxCode] = @boxCode";
                    countCommandText += "[BoxCode] = @boxCode";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@boxCode", tbOrderNumber.Text);
                }
                // Error.
                else
                {
                    return;
                }
            }
            else
            {
                if (start.Value.Date.Equals(end.Value.Date))
                {
                    commandText += "CAST([Date] AS Date) LIKE CAST (@date as Date)";
                    countCommandText +=  "CAST([Date] AS Date) LIKE CAST (@date as Date)";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@date", start.Value);
                }
                else
                {
                    commandText += "[Date] BETWEEN @startDate AND @stopDate";
                    countCommandText += "[Date] BETWEEN @startDate AND @stopDate";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@startDate", start.Value);
                    command.Parameters.AddWithValue("@stopDate", end.Value);
                }
                
                
            }
            command.CommandText = countCommandText;
            int totalRows = (int)command.ExecuteScalar();
           
            progressBar1.Value = 0;
            progressBar1.Maximum = totalRows;
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        progressBar1.Value++;

                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["ID"]);
                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["OrderNum"]);
                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["BoxCode"]);
                        DateTime date = (DateTime)reader["Date"];
                        outputSheet.Cells[curRow, curCol++] = new Cell(date.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                        outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["MBR"]);
                        // Don't show messages.
                        string qrCode = (string)reader["QRCode"];
                        qrCode = Regex.Replace(qrCode, @"\\000021", string.Empty);
                        outputSheet.Cells[curRow++, curCol++] = new Cell(qrCode);
                        curCol = 0;
                    }
                }

            }
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran opšti izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati opšti izveštaj!");
            }
        }

        /// <summary>
        /// Triggered when RW report creation from combo box is selected.
        /// </summary>
        /// <param name="start">Start date if provided.</param>
        /// <param name="end">End date if provided.</param>
        private void RWReport(DateTime? start, DateTime? end)
        {
            string outPath = string.Empty;
            if (start == null && end == null)
            {
                outPath += Application.StartupPath + @"\RWTabela " + tbOrderNumber.Text + ".xls";
            }
            else
            {

                outPath += Application.StartupPath + @"\RWTabela " + start.Value.Date.ToString("dd.MM.yyyy.") + " - " + end.Value.Date.ToString("dd.MM.yyyy.") + ".xls";
            }
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Sheet1");
            Workbook outputBook = new Workbook();


            // Set it to beginning of the document
            int curRow = 0;
            int curCol = 0;

            outputSheet.Cells[curRow, curCol++] = new Cell("Novi kod:");
            outputSheet.Cells[curRow, curCol++] = new Cell("Broj naloga/primopredaje");
            outputSheet.Cells[curRow, curCol++] = new Cell("Vrsta kutije");
            outputSheet.Cells[curRow, curCol++] = new Cell("Broj fajlova u kutiji");
            outputSheet.Cells[curRow, curCol++] = new Cell("Šifra kutije");
            outputSheet.Cells[curRow, curCol++] = new Cell("Doctype");
            outputSheet.Cells[curRow, curCol++] = new Cell("ID");
            outputSheet.Cells[curRow, curCol++] = new Cell("Mbr");
            outputSheet.Cells[curRow, curCol++] = new Cell("Partija");
            outputSheet.Cells[curRow, curCol++] = new Cell("Zahtev");
            outputSheet.Cells[curRow, curCol++] = new Cell("ID kartice");
            outputSheet.Cells[curRow++, curCol++] = new Cell("Paket");
            curCol = 0;

            SqlConnection conn = Helper.GetConnection();
            //conn.Open();
            SqlCommand command;
            string commandText = "SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable]";

            if (start == null && end == null)
            {
                // Do by order num.
                if (cbChoose.SelectedIndex == 0)
                {
                    commandText += " WHERE [OrderNum] = @orderNum";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
                } 
                // Do by box Code.
                else if (cbChoose.SelectedIndex == 2)
                {
                    commandText += " WHERE [BoxCode] = @boxCode";
                    command = new SqlCommand(commandText, conn);
                    command.Parameters.AddWithValue("@boxCode", tbOrderNumber.Text);
                }
                // Error.
                else
                {
                    return;
                }
            }
            else
            {
                command = new SqlCommand(commandText, conn);
            }

            List<string> boxCodes = new List<string>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string tmp = (string)reader[0];
                        if (!boxCodes.Contains(tmp))
                        {
                            boxCodes.Add(tmp);
                        }
                    }
                }
            }

           
            progressBar1.Value = 0;
            progressBar1.Maximum = boxCodes.Count;

            foreach (string boxCode in boxCodes)
            {
                progressBar1.Value++;
                if (start == null && end == null)
                {
                    if (cbChoose.SelectedIndex == 0)
                    {
                        commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] ON [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] AND [QRCode].[dbo].[BankTable].[BoxCode] = @boxCode AND [QRCode].[dbo].[BankTable].[OrderNum] = @orderNum INNER JOIN [QRCode].[dbo].[Box] ON [QRCode].[dbo].[BankTable].[BoxCode] = [QRCode].[dbo].[Box].Code ORDER BY [QRCode].[dbo].[RWTable].[Code]";
                        command = new SqlCommand(commandText, conn);
                        command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
                        command.Parameters.AddWithValue("@boxCode", boxCode);
                    } 
                    else if (cbChoose.SelectedIndex == 2)
                    {
                        commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] ON [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] AND [QRCode].[dbo].[BankTable].[BoxCode] = @boxCode INNER JOIN [QRCode].[dbo].[Box] ON [QRCode].[dbo].[BankTable].[BoxCode] = [QRCode].[dbo].[Box].Code ORDER BY [QRCode].[dbo].[RWTable].[Code]";
                        command = new SqlCommand(commandText, conn);
                        command.Parameters.AddWithValue("@boxCode", boxCode);
                    }
                    
                }
                else
                {
                    if (start.Value.Date.Equals(end.Value.Date))
                    {
                        commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] ON [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] AND [QRCode].[dbo].[BankTable].[BoxCode] = @boxCode AND CAST([QRCode].[dbo].[BankTable].[Date] as Date) LIKE CAST(@date as DATE) INNER JOIN [QRCode].[dbo].[Box] ON [QRCode].[dbo].[BankTable].[BoxCode] = [QRCode].[dbo].[Box].Code ORDER BY [QRCode].[dbo].[RWTable].[Code]";
                        command = new SqlCommand(commandText, conn);
                        command.Parameters.AddWithValue("@boxCode", boxCode);
                        command.Parameters.AddWithValue("@date", start.Value);
                    }
                    else
                    {
                        commandText = "SELECT * FROM [QRCode].[dbo].[BankTable] INNER JOIN [QRCode].[dbo].[RWTable] ON [QRCode].[dbo].[BankTable].[ID] = [QRCode].[dbo].[RWTable].[QRID] AND [QRCode].[dbo].[BankTable].[BoxCode] = @boxCode AND [QRCode].[dbo].[BankTable].[Date] BETWEEN @startDate AND @stopDate INNER JOIN [QRCode].[dbo].[Box] ON [QRCode].[dbo].[BankTable].[BoxCode] = [QRCode].[dbo].[Box].Code ORDER BY [QRCode].[dbo].[RWTable].[Code]";
                        command = new SqlCommand(commandText, conn);
                        command.Parameters.AddWithValue("@boxCode", boxCode);
                        command.Parameters.AddWithValue("@startDate", start.Value);
                        command.Parameters.AddWithValue("@stopDate", end.Value);
                    }
                    
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int i = 0;
                        // hbCode - current boxcode
                        string orderNum = null, hbCode = null;
                        string doctype = null, id = "", mbr = null, partija = null, zahtev = null, idKartice = null,paket = null;
                        // current filenumber
                        string tmpCode = null;
                        // old filenumber
                        string oldcode = null;
                        int numfiles = 0;
                        int counter = 0;
                        while (reader.Read())
                        {
                            i++;
                            counter++;
                            orderNum = (string)reader["OrderNum"];
                            hbCode = (string)reader["BoxCode"];
                            oldcode = tmpCode;
                            tmpCode = (string)reader[7];

                            if ((tmpCode == oldcode && i % 21 == 0) || (tmpCode != oldcode && oldcode != null && i > 1))
                            {

                                #region Write header data
                                // Always read from the beggining of list and remove codes added to cells.
                                outputSheet.Cells[curRow, curCol++] = new Cell(oldcode);
                                outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["OrderNum"]);
                                string bCode = (string)reader["BoxCode"];
                                int boxType = GetTypeFromBoxCode(bCode);
                                switch (boxType)
                                {
                                    case 86:
                                        outputSheet.Cells[curRow, curCol++] = new Cell("POZAJMICE");
                                        break;
                                    case 148:
                                        outputSheet.Cells[curRow, curCol++] = new Cell("KREDITI");
                                        break;
                                    case 82:
                                        outputSheet.Cells[curRow, curCol++] = new Cell("RAČUNI");
                                        break;
                                    case 83:
                                        outputSheet.Cells[curRow, curCol++] = new Cell("OROČENJA");
                                        break;
                                    default:
                                        break;

                                }
                                #endregion
                                outputSheet.Cells[curRow, curCol++] = new Cell((int)reader["NumberOfFiles"]);
                                outputSheet.Cells[curRow, curCol++] = new Cell(bCode);

                                #region Write to cells
                                if (doctype != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(doctype);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }

                                if (id != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(id);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }

                                if (mbr != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(mbr);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }

                                if (partija != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(partija);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }

                                if (zahtev != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(zahtev);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }
                                if (idKartice != null)
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(idKartice);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                                }
                                if (paket != null)
                                {
                                    outputSheet.Cells[curRow++, curCol++] = new Cell(paket);
                                }
                                else
                                {
                                    outputSheet.Cells[curRow++, curCol++] = new Cell(string.Empty);
                                }
                                #endregion
                                doctype = null;
                                id = null;
                                mbr = null;
                                partija = null;
                                zahtev = null;
                                idKartice = null;
                                paket = null;
                                i = 1;
                                curCol = 0;
                                oldcode = tmpCode;
                            }
                            numfiles = (int)reader["NumberOfFiles"];
                            #region handle Code
                            string code = (string)reader["QRCode"];
                            code = Regex.Replace(code, @"\\000021", string.Empty);
                            code = Regex.Replace(code, "{", string.Empty);
                            code = Regex.Replace(code, "}", string.Empty);
                            code = Regex.Replace(code, " ", string.Empty);
                            code = Regex.Replace(code, "\r\n", string.Empty);
                            // Separate client infos and remove ".
                            string[] stringSeparators = new string[] { "," };
                            string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            string tmpdoctype = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmpid = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmpmbr = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmppartija = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmpzahtev = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmpidKartice = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            string tmppaket = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                            foreach (string clientInfo in tokens)
                            {
                                string tmp = Regex.Replace(clientInfo, "\"", string.Empty);
                                string[] tmpSeparator = new string[] { ":" };
                                string[] tmpTokens = tmp.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                                if (tmpTokens.Length > 1)
                                {
                                    if (Helper.CheckId(tmpTokens[0]))
                                    {
                                        // Get id.
                                        tmpid = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckDocType(tmpTokens[0]))
                                    {
                                        // Get type of document.
                                        tmpdoctype = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckMbr(tmpTokens[0]))
                                    {
                                        tmpmbr = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckPartija(tmpTokens[0]))
                                    {
                                        tmppartija = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckZahtev(tmpTokens[0]))
                                    {
                                        tmpzahtev = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckIdKartice(tmpTokens[0]))
                                    {
                                        tmpidKartice = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                    else if (Helper.CheckPaket(tmpTokens[0]))
                                    {
                                        tmppaket = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                                    }
                                }

                            }
                            id += tmpid;
                            doctype += tmpdoctype;
                            mbr += tmpmbr;
                            partija += tmppartija;
                            zahtev += tmpzahtev;
                            idKartice += tmpidKartice;
                            paket += tmppaket;
                            #endregion



                        }
                        // Divide has less than 30 still need to write it
                        if (i != 0)
                        {
                            #region Write header data
                            // Always read from the beggining of list and remove codes added to cells.
                            outputSheet.Cells[curRow, curCol++] = new Cell(tmpCode);
                            outputSheet.Cells[curRow, curCol++] = new Cell(orderNum);
                            int boxType = GetTypeFromBoxCode(hbCode);
                            switch (boxType)
                            {
                                case 86:
                                    outputSheet.Cells[curRow, curCol++] = new Cell("POZAJMICE");
                                    break;
                                case 148:
                                    outputSheet.Cells[curRow, curCol++] = new Cell("KREDITI");
                                    break;
                                case 82:
                                    outputSheet.Cells[curRow, curCol++] = new Cell("RAČUNI");
                                    break;
                                case 83:
                                    outputSheet.Cells[curRow, curCol++] = new Cell("OROČENJA");
                                    break;
                                default:
                                    break;

                            }
                            #endregion
                            outputSheet.Cells[curRow, curCol++] = new Cell(numfiles);
                            outputSheet.Cells[curRow, curCol++] = new Cell(hbCode);

                            #region Write to cells
                            if (doctype != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(doctype);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }

                            if (id != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(id);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }

                            if (mbr != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(mbr);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }

                            if (partija != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(partija);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }

                            if (zahtev != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(zahtev);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }
                            if (idKartice != null)
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(idKartice);
                            }
                            else
                            {
                                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
                            }
                            if (paket != null)
                            {
                                outputSheet.Cells[curRow++, curCol++] = new Cell(paket);
                            }
                            else
                            {
                                outputSheet.Cells[curRow++, curCol++] = new Cell(string.Empty);
                            }
                            #endregion
                            curCol = 0;
                        }
                    }
                    
                }
                
            }
            
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran reisswolf izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati reisswolf izveštaj!");
            }

        }

        /// <summary>
        /// Reades input box code type from database.
        /// </summary>
        /// <param name="boxCode">Input box code.</param>
        /// <returns>Output box type.</returns>
        private int GetTypeFromBoxCode(string boxCode)
        {
            // Because this is called from another reader we NEED to open a new connection.
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT [Type] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int ret = (int)reader[0];
                        return ret;
                    }
                    else
                    {
                        return -1;
                    }
                }   
            }    
        }

        /// <summary>
        /// Gets the number of files in box from database.
        /// </summary>
        /// <param name="boxCode">Unique identifier of box.</param>
        /// <returns></returns>
        private int GetNumberOfFilesFromBox(string boxCode)
        {
            int fileNum = -1;

            // Because this is called from another reader we NEED to create a new connection.
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        fileNum = (int)reader["NumberOfFiles"];
                    }
                    return fileNum;
                }
            }
        }

        /// <summary>
        /// Triggered when selected index is changed, changes GUI according to selection.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Following args.</param>
        private void cbChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbChoose.SelectedIndex)
            {
                // Order number is selected.
                case 0:
                    lValue.Visible = true;
                    lValue.Text = "Unesite broj naloga:";
                    tbOrderNumber.Visible = true;
                    tbOrderNumber.Text = string.Empty;
                    dateTimeFrom.Visible = false;
                    dateTimeUntil.Visible = false;
                    break;
                // Date is selected.
                case 1:
                    lValue.Visible = true;
                    lValue.Text = "Izaberite pocetni i kranji datum:";
                    tbOrderNumber.Visible = false;
                    dateTimeFrom.Visible = true;
                    dateTimeUntil.Visible = true;
                    break;
                case 2:
                    lValue.Visible = true;
                    lValue.Text = "Unesite kod kutije:";
                    tbOrderNumber.Visible = true;
                    tbOrderNumber.Text = string.Empty;
                    dateTimeFrom.Visible = false;
                    dateTimeUntil.Visible = false;
                    break;
                default:
                    lValue.Visible = false;
                    dateTimeFrom.Visible = false;
                    dateTimeUntil.Visible = false;
                    tbOrderNumber.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// Generates reports for selected types of reports.
        /// </summary>
        /// <param name="start">Start date if provided.</param>
        /// <param name="end">Stop date if provided.</param>
        private void GenerateReports(DateTime? start = null, DateTime? end = null)
        {
            if (checkBox1.Checked)
            {
                GeneralReport(start, end);
            }
            if (checkBox2.Checked)
            {
                DetailedReport(start, end);
            }
            if (checkBox3.Checked)
            {
                RWReport(start, end);
            }
        }

        /// <summary>
        /// Gets info from input QR ceode and writes it to outputSheet to different cells.
        /// </summary>
        /// <param name="code">Input unparsed QR code.</param>
        /// <param name="outputSheet">Worksheet to write the parsed data.</param>
        /// <param name="curRow">Current row count.</param>
        /// <param name="curCol">Current column count.</param>
        private void HandleCodeAndWrite(string code, Worksheet outputSheet, ref int curRow, ref int curCol)
        {

            string doctype = null, id = null, mbr = null, partija = null, zahtev = null, idKartice = null, mbrid = null, paket = null;

            code = Regex.Replace(code, @"\\000021", string.Empty);
            code = Regex.Replace(code, "{", string.Empty);
            code = Regex.Replace(code, "}", string.Empty);
            code = Regex.Replace(code, " ", string.Empty);
            code = Regex.Replace(code, "\r\n", string.Empty);
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

                    if (Helper.CheckId(tmpTokens[0]))
                    {
                        // Get id.
                        id = tmpTokens[1];
                    }
                    else if (Helper.CheckDocType(tmpTokens[0]))
                    {
                        // Get type of document.
                        doctype = tmpTokens[1];
                    }
                    else if (Helper.CheckMbr(tmpTokens[0]))
                    {
                        mbr = tmpTokens[1];
                    }
                    else if (Helper.CheckPartija(tmpTokens[0]))
                    {
                        partija = tmpTokens[1];
                    }
                    else if (Helper.CheckZahtev(tmpTokens[0]))
                    {
                        zahtev = tmpTokens[1];
                    }
                    else if (Helper.CheckIdKartice(tmpTokens[0]))
                    {
                        idKartice = tmpTokens[1];
                    }
                    else if (Helper.CheckMbrId(tmpTokens[0]))
                    {
                        mbrid = tmpTokens[1];
                    } else if (Helper.CheckPaket(tmpTokens[0]))
                    {
                        paket = tmpTokens[1];
                    }
                }

            }

            if (doctype != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(doctype);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }

            if (id != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(id);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }

            if (mbr != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(mbr);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }

            if (partija != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(partija);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }

            if (zahtev != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(zahtev);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }
            if (idKartice != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(idKartice);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }
            if (mbrid != null)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(mbrid);
            }
            else
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(string.Empty);
            }

            if (paket != null)
            {
                outputSheet.Cells[curRow++, curCol++] = new Cell(paket);
            } 
            else
            {
                outputSheet.Cells[curRow++, curCol++] = new Cell(string.Empty);
            }

            curCol = 0;
        }
    }
}
