﻿using ExcelLibrary.SpreadSheet;
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
    public partial class ReportDialog : Form
    {
        public ReportDialog()
        {
            InitializeComponent();
            
        }

        private void BReport_Click(object sender, EventArgs e)
        {
            if (tbOrderNumber.Text.Equals(string.Empty))
            {
                MessageBox.Show("Unesite broj naloga.");
                
            }
            else
            {
                switch (cbReport.SelectedIndex)
                {
                    case -1:
                        MessageBox.Show("Izaberite vrstu izveštaja.");
                        break;
                    case 0:
                        GeneralReport();
                        break;
                    case 1:
                        DetailedReport();
                        break;
                    case 2:
                        RWReport();
                        break;
                }
            }
            
        }

        /// <summary>
        /// Triggered when detailed report creation from combo box is selected.
        /// </summary>
        private void DetailedReport()
        {
            string outPath = string.Empty;
            outPath += Application.StartupPath + @"\tabelaZaBankuDetaljna.xls";
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Output");
            Workbook outputBook = new Workbook();


            // Set it to beginning of the document
            int curRow = 0;
            int curCol = 0;

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

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[BankTable] WHERE [OrderNum] = @orderNum", conn);
            command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
            SqlDataReader reader = command.ExecuteReader();

            string doctype = null, id = null, mbr = null, partija = null, zahtev = null, idKartice = null, paket = null;

            while (reader.Read())
            {
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
                outputSheet.Cells[curRow, curCol++] = new Cell(boxCode);

                #region handle Code
                string code = (string)reader["QRCode"];
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
                    if (tmpTokens[0].Equals("id"))
                    {
                        // Get id.
                        id = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("doctype"))
                    {
                        // Get type of document.
                        doctype = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("mbr"))
                    {
                        mbr = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("partija"))
                    {
                        partija = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("zahtev"))
                    {
                        zahtev = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("id_kartice"))
                    {
                        idKartice = tmpTokens[1];
                    }
                    else if (tmpTokens[0].Equals("paket"))
                    {
                        paket = tmpTokens[1];
                    }

                }
                #endregion

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

                curCol = 0;
            }
            conn.Close();
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati izveštaj!");
            }


            curCol = 0;
        }

        /// <summary>
        /// Triggered when report creation from combo box is selected.
        /// </summary>
        private void GeneralReport()
        {
            string outPath = null;
            outPath += Application.StartupPath + @"\tabelaZaBanku.xls";
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Output");
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

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[BankTable] WHERE [OrderNum] = @orderNum", conn);
            command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["ID"]);
                outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["OrderNum"]);
                outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["BoxCode"]);
                DateTime date = (DateTime)reader["Date"];
                outputSheet.Cells[curRow, curCol++] = new Cell(date.ToShortDateString());
                outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["MBR"]);
                outputSheet.Cells[curRow++, curCol++] = new Cell((string)reader["QRCode"]);
                curCol = 0;
            }
            conn.Close();
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati izveštaj!");
            }


            curCol = 0;

        }

        /// <summary>
        /// Triggered when RW report creation from combo box is selected.
        /// </summary>
        private void RWReport()
        {
            string outPath = string.Empty;
            outPath += Application.StartupPath + @"\tabelaZaRW.xls";
            // Create default output sheet and workbook
            Worksheet outputSheet = new Worksheet("Output");
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

            // Get all box codes  for RW report.
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [BoxCode] FROM [QRCode].[dbo].[BankTable] WHERE [OrderNum] = @orderNum", conn);
            command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
            SqlDataReader reader = command.ExecuteReader();

            List<string> boxCodes = new List<string>();

            while(reader.Read())
            {
                string tmp = (string)reader[0];
                if (!boxCodes.Contains(tmp))
                {
                    boxCodes.Add(tmp);
                }
            }
            reader.Close();

           
            foreach (string boxCode in boxCodes)
            {
                List<string> codes = new List<string>();
                // Get codes foreach box code for new report.
                command = new SqlCommand("SELECT [Code] FROM [QRCode].[dbo].[RWTable] WHERE [BoxCode] = @boxCode", conn);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    codes.Add((string)reader[0]);
                }
                reader.Close();

                // Get values for current boxCode and write to table.
                command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[BankTable] WHERE [BoxCode] = @boxCode AND [OrderNum] = @orderNum", conn);
                command.Parameters.AddWithValue("@orderNum", tbOrderNumber.Text);
                command.Parameters.AddWithValue("@boxCode", boxCode);
                reader = command.ExecuteReader();

                string orderNum = null, hbCode = null;
                string doctype = null, id = null, mbr = null, partija = null, zahtev = null, idKartice = null, paket = null;

                // Get for each boxCode infos and add them to excel table.
                int i = 0;
                while(reader.Read())
                {
                    i++;
                    orderNum = (string)reader["OrderNum"];
                    hbCode = (string)reader["BoxCode"];
                    #region handle Code
                    string code = (string)reader["QRCode"];
                    code = Regex.Replace(code, "{", string.Empty);
                    code = Regex.Replace(code, "}", string.Empty);
                    code = Regex.Replace(code, " ", string.Empty);
                    // Separate client infos and remove ".
                    string[] stringSeparators = new string[] { "," };
                    string[] tokens = code.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    string tmpdoctype = i.ToString() + ". "  + @"\\" + ((char)13).ToString();
                    string tmpid = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                    string tmpmbr = i.ToString() + ". "  + @"\\" + ((char)13).ToString();
                    string tmppartija = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                    string tmpzahtev = i.ToString() + ". "  + @"\\" + ((char)13).ToString();
                    string tmpidKartice = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                    string tmppaket = i.ToString() + ". " + @"\\" + ((char)13).ToString();
                    foreach (string clientInfo in tokens)
                    {
                        string tmp = Regex.Replace(clientInfo, "\"", string.Empty);
                        string[] tmpSeparator = new string[] { ":" };
                        string[] tmpTokens = tmp.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries);
                        if (tmpTokens[0].Equals("id"))
                        {
                            // Get id.
                            tmpid =i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString(); 
                        }
                        else if (tmpTokens[0].Equals("doctype"))
                        {
                            // Get type of document.
                            tmpdoctype = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                        }
                        else if (tmpTokens[0].Equals("mbr"))
                        {
                            tmpmbr = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                        }
                        else if (tmpTokens[0].Equals("partija"))
                        {
                            tmppartija = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                        }
                        else if (tmpTokens[0].Equals("zahtev"))
                        {
                            tmpzahtev = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                        }
                        else if (tmpTokens[0].Equals("id_kartice"))
                        {
                            tmpidKartice = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
                        }
                        else if (tmpTokens[0].Equals("paket"))
                        {
                            tmppaket = i.ToString() + ". " + tmpTokens[1] + @"\\" + ((char)13).ToString();
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

                   
                    if ( i % 30 == 0)
                    {

                        #region Write header data
                        // Always read from the beggining of list and remove codes added to cells.
                        outputSheet.Cells[curRow, curCol++] = new Cell(codes[0]);
                        codes.RemoveAt(0);
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
                        outputSheet.Cells[curRow, curCol++] = new Cell(GetNumberOfFilesFromBox(boxCode).ToString());
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
                        i = 0;
                        curCol = 0;
                    }
                }

             
                // Divide has less than 30 still need to write it
                if (i != 0)
                {
                    #region Write header data
                    // Always read from the beggining of list and remove codes added to cells.
                    outputSheet.Cells[curRow, curCol++] = new Cell(codes[0]);
                    codes.RemoveAt(0);
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
                    outputSheet.Cells[curRow, curCol++] = new Cell(GetNumberOfFilesFromBox(boxCode).ToString());
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
                    reader.Close();
                    curCol = 0;
                }
                reader.Close();
            }

            conn.Close();
            try
            {
                outputBook.Worksheets.Add(outputSheet);
                outputBook.Save(outPath);
                MessageBox.Show("Uspešno kreiran izveštaj.");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće kreirati izveštaj!");
            }

        }

        /// <summary>
        /// Reades input box code type from database.
        /// </summary>
        /// <param name="boxCode">Input box code.</param>
        /// <returns>Output box type.</returns>
        private int GetTypeFromBoxCode(string boxCode)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [Type] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                return (int)reader[0];
            }
            else
            {
                return -1;
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

            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-DMTBJFE;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode", conn);
            command.Parameters.AddWithValue("@boxCode", boxCode);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                fileNum = (int)reader["NumberOfFiles"];
            }
            return fileNum;
        }
    }
}