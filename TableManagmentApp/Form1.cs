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

namespace TableManagmentApp
{
    /// <summary>
    /// Form for converting from RWtable to detailed table.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Path of input file.
        /// </summary>
        private string filePath = string.Empty;

        /// <summary>
        /// Path for output file.
        /// </summary>
        string outputPath = string.Empty;
        /// <summary>
        /// Workbook for input .xls file.
        /// </summary>
        private Workbook book;
        /// <summary>
        /// Worksheet for input .xls file.
        /// </summary>
        private Worksheet sheet;

        /// <summary>
        /// Workbook for output .xls file.
        /// </summary>
        private Workbook outputBook;

        /// <summary>
        /// Worksheet for output .xls file.
        /// </summary>
        private Worksheet outputSheet;

        private System.IO.StreamWriter log;

        /// <summary>
        /// Initializes a new instance <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button for open file chooser.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Following args.</param>
        private void button1_Click(object sender, EventArgs e)
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
        /// Button for confirm.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Following args.</param>>
        private void button2_Click(object sender, EventArgs e)
        {
            if (filePath != string.Empty && tbSeparator.Text != string.Empty)
            {
                // Try to open input table.
                book = new Workbook();
                try
                {
                    book = Workbook.Load(filePath);
                    log = new System.IO.StreamWriter(Application.StartupPath + "log.txt");
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



                string[] separator = new string[] {tbSeparator.Text};
                List<string> columnsForSeparation = new List<string>();// = new List<string>(colInput.Split(tmpSeparator, StringSplitOptions.RemoveEmptyEntries));
                for (int i = 1; i <= 21; i++)
                {
                    columnsForSeparation.Add(i.ToString());
                }

                try
                {
                    // List of column names.
                    List<string> columnNames = new List<string>();
                    // List of records in row.
                    List<string> rowValues;

                    // Current row in output  documents.
                    int curRow = 0;
                    // Current column in output document.
                    int curCol = 0;
                    for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
                    {
                        // Indicator if row fails and has to be skipped.
                        bool breakRow = false;
                        rowValues = new List<string>();
                        // Get current row.
                        Row row = new Row();
                        row = sheet.Cells.GetRow(rowIndex);
                        // Get all column names.
                        if (rowIndex == sheet.Cells.FirstRowIndex)
                        {
                            for(int colIndex = row.FirstColIndex; colIndex <= row.LastColIndex;colIndex++)
                            {
                                columnNames.Add(row.GetCell(colIndex).StringValue);
                            }
                            OpenOutputFiles();
                            WriteData(ref curRow, ref curCol, columnNames,new List<int>());
                        }
                        else
                        {
                            // Add all values including non-splited.
                            for (int colIndex = row.FirstColIndex; colIndex <= row.LastColIndex; colIndex++)
                            {
                                rowValues.Add(row.GetCell(colIndex).StringValue);
                            }

                            // Cells including non splittable and splittable data.
                            Dictionary<int, object> cells = new Dictionary<int, object>();
                            // Number of rows after splitting.
                            int splitCnt = -1;
                            // Collect data for splitting.
                            for (int i = 0; i < rowValues.Count;i++)
                            {
                                // Should it be splited?
                                if (columnsForSeparation.Contains((i+1).ToString()))
                                {
                                    string[] tokens = null;
                                    if (rowValues[i].Contains(separator[0]))
                                    {
                                        tokens = rowValues[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    else
                                    {
                                        tokens = new string[] { rowValues[i] };
                                    }
                                    List<string> tmp = new List<string>(tokens);

                                    // List of indexes if there are data to be deleted.
                                    List<int> indexes = new List<int>();

                                    for (int k = 0; k < tmp.Count;k++ )
                                    {
                                        if (tmp[k].EndsWith("\n"))
                                        {
                                            indexes.Add(k);
                                        }
                                    }
                                    foreach(int index in indexes)
                                    {
                                        tmp.RemoveAt(index);
                                    }

                                    // First set number of rows.
                                    if (splitCnt < tmp.Count)
                                    {
                                        splitCnt = tmp.Count;
                                    }
                                    /*
                                    // Differs. Columns don't contain same number of data to split.
                                    else if (splitCnt != tmp.Count && splitCnt != 1 && tmp.Count != 1)
                                    {
                                       log.WriteLine("Greška! Red " + (rowIndex + 1).ToString() + "ne sadrži isti broj podataka za razdvajanje u navedenim kolonama.");
                                       breakRow = true;
                                       break;
                                    }
                                    */
                                    if (tmp.Count == 1)
                                    {
                                        cells[i] = tmp[0];
                                    }
                                    else
                                    {
                                        cells[i] = tmp;
                                    }
                                    
                                }
                                // Shouldn't be splited.
                                else
                                {
                                    cells[i] = rowValues[i];
                                }
                            }

                            if (breakRow)
                            {
                                continue;
                            }


                            // List of data to be written.
                            List<string> rowData = new List<string>();
                            // Counter for iterating trough unsplitted data.
                            int tmpCnt = 0;

                            while (tmpCnt < splitCnt)
                            {
                                // Get all values.
                                int i = 0;
                                List<int> indexes = new List<int>();
                                foreach (KeyValuePair<int, object> cell in cells)
                                {
                                    if (cell.Value is List<string>)
                                    {
                                        List<string> tmpList = (List<string>)cell.Value;
                                        rowData.Add(tmpList[tmpCnt]);
                                        indexes.Add(i);
                                    }
                                    else if (cell.Value is string)
                                    {
                                        rowData.Add(cell.Value.ToString());
                                    }
                                    i++;
                                }
                                // Write them.
                                WriteData(ref curRow, ref curCol, rowData, indexes);
                                // Clear data.
                                rowData.Clear();
                                indexes.Clear();
                                // Increment counter;
                                tmpCnt++;
                            }

                        }
                       
                        
                    }
                    SaveData();


                }
                catch(Exception gf)
                {
                    MessageBox.Show(gf.Message);
                }
            }
            else
            {
                MessageBox.Show("GRESKA!");
            }
        }

        /// <summary>
        /// Opens output book and sheet.
        /// </summary>
        private void OpenOutputFiles()
        {
            outputPath = Application.StartupPath + @"\izlaz.xls";
            outputSheet = new Worksheet("Sheet1");
            outputBook = new Workbook();


        }

        /// <summary>
        /// Writes one row of data (including header).
        /// </summary>
        /// <param name="curRow">Current row.</param>
        /// <param name="curCol">Current column.</param>
        /// <param name="names">Data from one row (splited).</param>
        private void WriteData(ref int curRow, ref int curCol, List<string> data, List<int> indexes)
        {
            var rgx = new Regex("[0-9]+\\.(?<actualstr>.*)");
            List<int> a;
            for (int i = 0; i < data.Count; ++i)
            {
                string tmpData = data[i].Trim();

                if (indexes.Contains(i))
                {
                    var match = rgx.Match(tmpData);
                    if (match.Success)
                    {
                        tmpData = match.Groups["actualstr"].Value;
                    }
                }
                if (tmpData.StartsWith("\""))
                {
                    tmpData = tmpData.Substring(1);
                }
                if (tmpData.EndsWith("\""))
                {
                    tmpData = tmpData.Substring(0, tmpData.Length - 1);
                }
                outputSheet.Cells[curRow, curCol++] = new Cell(tmpData);
            }
            curRow++;
            curCol = 0;
        }

        /// <summary>
        /// Saves data to output table.
        /// </summary>
        private void SaveData()
        {
            outputBook.Worksheets.Add(outputSheet);
            outputBook.Save(outputPath);
            MessageBox.Show("Uspešno kreirana izlazna tabela.");
        }
    }
}
