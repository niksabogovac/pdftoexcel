using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;
using System.Text.RegularExpressions;

namespace SecondProject
{
    class Conversion 
    {

        #region Variable definition

        /// <summary>
        /// Columns of input file
        /// </summary>
        private Int32 year;
        private string fileNumber;
        private string categoryNumber;
        private string categoryName;
        private string fileType;
        private string deadline;
        private string location;

        /// <summary>
        /// Support fields used for reading input  files
        /// </summary>
        private Workbook book;
        private Worksheet sheet;
        private Worksheet outputSheet1;
        private Workbook outputBook1;
        private Worksheet outputSheet2;
        private Workbook outputBook2;

        /// <summary>
        /// Output fields for first output file
        /// </summary>
        private Int32 yearOutput;
        private Int32 archiveNumber;
        private string categoryNumberOutput;
        private string categoryNameOutput;
        private string fileTypeOutput;
        private string deadlineOutput;
        private string fileAmount;
        private string locationOutput;

        /// <summary>
        /// Output files no.2 uses archiveNumber of first output file, explained later
        /// </summary>
        private string fileNumberOutput;

        /// <summary>
        /// Indicator of both outputFiles
        /// </summary>
        private int curRowOutput1;
        private int curColOutput1;
        private int curRowOutput2;
        private int curColOutput2;

        public string InputFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Paths for output files
        /// </summary>
        private string outputFilePath1;
        private string outputFilePath2;

        /// <summary>
        /// Log files is used for writing lines of input rows that could not be converted
        /// </summary>
 
        private StreamWriter logFile;

        /// <summary>
        /// Internal representation of all rows
        /// </summary>
        private List<MyRow> rows;


        /// <summary>
        /// Recognize a range od years
        /// </summary>
        private static Regex yearReg = new Regex(@"[0-9]{4}");
        private static Regex yearRangeReg = new Regex(@"[0-9]{4}-[0-9]{4}");
        
        #endregion

        private void Initialize()
        {
            year = -1;
            yearOutput = -1;
            fileNumber = "";
            categoryNumber = "";
            categoryName = "";
            fileType = "";
            deadline = "";
            location = "";


            yearOutput = -1;
            archiveNumber = -1;
            categoryNumberOutput = "";
            categoryNameOutput = "";
            fileTypeOutput = "";
            deadlineOutput = "";
            fileAmount = "";
            locationOutput = "";

            curColOutput1 = 0;
            curRowOutput1 = 0;
            curColOutput2 = 0;
            curRowOutput2 = 0;

            outputBook1 = new Workbook();
            outputSheet1 = new Worksheet("Izlaz");

            outputBook2 = new Workbook();
            outputSheet2 = new Worksheet("Izlaz");

            rows = new List<MyRow>();
        }
            
        public Conversion(string _intputFilePath, int _archNum)
        {
            Initialize();
            InputFilePath = _intputFilePath;
            archiveNumber = _archNum;
        }

        /// <summary>
        /// Open intput files using property InputFileNumber.
        /// Creates output files in the same directory as the running exe and writes headers to it.
        /// </summary>
        public void OpenInputAndOutputFiles()
        {
            book = new Workbook();
            try
            {
                book = Workbook.Load(InputFilePath);
            }
            catch(Exception)
            {
                MessageBox.Show("Ulazni fajl ne moze da se otvori.\n"
                              + "Proverite da li je vec prethodno otvoren.");
                return;
            }
            
            try
            {
                sheet = book.Worksheets[0];
            }
            catch(Exception)
            {
                MessageBox.Show("Uneti fajl ne moze da se otvori.\n"
                               +"Ulazni fajl mora da sadrzi sve u prvom sheetu!");
                return;
            }
            
            try
            {
                outputFilePath1 = Application.StartupPath + @"\Izlaz1.xls";
                outputFilePath2 = Application.StartupPath + @"\Izlaz2.xls";
                logFile = new StreamWriter(Application.StartupPath + @"\log.txt");
            } catch (Exception)
            {
                MessageBox.Show("Izlazni fajlovi ne mogu da budu otvoreni, proverite mesto na kojem ste pokrenuli program.");
                return;
            }

            


            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Arhivski broj");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Godina");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Broj kategorije");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Naziv kategorije");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Rok cuvanja");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Vrsta dokumenta");
            outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("Kolicina");
            outputSheet1.Cells[curRowOutput1++, curColOutput1++] = new Cell("Lokacija");
            curColOutput1 = 0;
            outputSheet2.Cells[curRowOutput2, curColOutput2++] = new Cell("Arhivski broj");
            outputSheet2.Cells[curRowOutput2++, curColOutput2++] = new Cell("Broj dokumenta");
            curColOutput2 = 0;

        }

        public void Write()
        {
            try
            {
                outputBook1.Worksheets.Add(outputSheet1);
                outputBook1.Save(outputFilePath1);
            }
            catch
            {
                MessageBox.Show("Nije moguce upisati u prvi izlazni fajl.\n"
                               +"On se kreira u datoteci gde je i ulazni.\n"
                               +"Zatvorite ga i pokusajte ponovo.");
                return;
            }

            try
            {
                outputBook2.Worksheets.Add(outputSheet2);
                outputBook2.Save(outputFilePath2);
            }
            catch
            {
                MessageBox.Show("Nije moguce upisati u drugi izlazni fajl.\n"
                               + "On se kreira u datoteci gde je i ulazni.\n"
                               + "Zatvorite ga i pokusajte ponovo.");
                return;
            }
            MessageBox.Show("Uspesno kreirane izlazne datoteke pod imenom Izlaz1 i Izlaz2!");
            logFile.Close();
        }

        public void MainSort()
        {
            
            for (int rowIndex = sheet.Cells.FirstRowIndex + 1; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);

               
                int _year = 0;
                string tmpYearString = "";
                tmpYearString = row.GetCell(0).StringValue;
                _year = Int32.Parse(tmpYearString);

                string _categoryNumber = "";
                string _categoryName = "";
                string tmpCategory = row.GetCell(4).StringValue;
                char[] stringSeparator = new char[] {' ' };
                string[] tokens = tmpCategory.Split(stringSeparator, 2);
                _categoryNumber = tokens[0];
                _categoryName = tokens[1];

                string _fileType = row.GetCell(2).StringValue;
                string _location = row.GetCell(9).StringValue;
                string _fileNumber = row.GetCell(11).StringValue;

                MyRow tmpRow = new MyRow(_year,_categoryNumber,_fileType,_location,_categoryName,"",_fileNumber);
                rows.Add(tmpRow);
            }

            rows.Sort();
            foreach(MyRow row in rows)
            {
                
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell((archiveNumber++).ToString());
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.Year.ToString());
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.CategoryNumber);
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.CategoryName);
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.Deadline);
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.FileType);
                outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(row.FileNumber);
                outputSheet1.Cells[curRowOutput1++, curColOutput1++] = new Cell(row.Location);
                curColOutput1 = 0;
            }
            MessageBox.Show("");
            #region 
            /*List<Int32> yearList = new List<Int32>();
            List<string> locationList = new List<string>();
            List<string> categoryNumberList = new List<string>();

            // Get all rows into a list and later sort them out
            for (int rowIndex = sheet.Cells.FirstRowIndex + 1; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);
                rows.Add(row);

                int tmpYear = 0;
                string tmpYearString = "";
                tmpYearString = row.GetCell(0).StringValue;

                if (yearRangeReg.IsMatch(tmpYearString))
                {
                    string[] stringSeparators = new string[] { "-" };
                    string[] tokens = tmpYearString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tmpYear = Int32.Parse(tokens[0]);
                    yearList.Add(tmpYear);

                } else if (yearReg.IsMatch(tmpYearString))
                {
                    tmpYear = Int32.Parse(tmpYearString);
                    if (!yearList.Contains(tmpYear))
                        yearList.Add(tmpYear);
                }

            }
            yearList.Sort();

            List<Row> rowsSortedByYear = new List<Row>();

            foreach (uint  currentYear in yearList)
            {
                foreach(Row row in rows)
                {
                    int tmpYear = 0;
                    string tmpYearString = "";
                    tmpYearString = row.GetCell(0).StringValue;
                    if (yearRangeReg.IsMatch(tmpYearString))
                    {
                        string[] stringSeparators = new string[] { "-" };
                        string[] tokens = tmpYearString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        tmpYear = Int32.Parse(tokens[0]);
                    }
                    else if (yearReg.IsMatch(tmpYearString))
                    {
                        tmpYear = Int32.Parse(tmpYearString);
                    }
                    if (tmpYear == currentYear)
                    {
                        rowsSortedByYear.Add(row);
                    }
                }
            }

            */
            #endregion

        }

        private class MyRow : Row, IComparable
        {
            public Int32 Year
            {
                get;
                set;
            }
            public string CategoryNumber
            {
                get;
                set;
            }
            public string FileType
            {
                get;
                set;
            }
            public string Location
            {
                get;
                set;
            }
            public string CategoryName
            {
                get;
                set;
            }
            public string Deadline
            {
                get;
                set;
            }
            public string FileNumber
            {
                get;
                set;
            }

            public MyRow(int _year, string _categoryNumber,string _fileType, string _location, string _categoryName,string _deadline, string _fileNumber)
            {
                this.Year = _year;
                this.CategoryNumber = _categoryNumber;
                this.FileType = _fileType;
                this.Location = _location;
                this.CategoryName = _categoryName;
                this.Deadline = _deadline;
                this.FileNumber = _fileType;
            }

            virtual public int CompareTo(object obj)
            {
                

                if (obj is MyRow)
                {
                    var compareObj = (MyRow)obj;
                    if (this.Year.CompareTo(compareObj.Year) == 0)
                    {
                        if (this.Location.CompareTo(compareObj.Location) == 0)
                        {
                            if (this.CategoryNumber.CompareTo(compareObj.CategoryNumber) == 0)
                            {
                                return this.FileType.CompareTo(compareObj.FileType);
                            }
                            return this.CategoryNumber.CompareTo(compareObj.CategoryNumber);
                        }
                        return this.Location.CompareTo(compareObj.Location);
                    }
                    return this.Year.CompareTo(compareObj.Year);
                }
                else
                {
                    throw new ArgumentException("Object is not a MyObject ");
                }
            }
        }


    }
}
