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
        /// Support fields used for reading input  files
        /// </summary>
        private Workbook book;
        private Worksheet sheet;
        private Worksheet outputSheet1;
        private Workbook outputBook1;
        private Worksheet outputSheet2;
        private Workbook outputBook2;

 
        /// <summary>
        /// This is get trough textBox
        /// </summary>
        private Int32 archiveNumber;


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
        private static Regex yearRangeReg = new Regex(@"[0-9]{4}.? *- *[0-9]{4}");
        private static Regex yearManyReg = new Regex(@"([0-9]{4} ?. ?)+");
        private static Regex trajnoReg = new Regex(@" *trajno *");
        #endregion

        private void Initialize()
        {
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

        public bool MainSort()
        {
            ChooseDiagMore diag = new ChooseDiagMore();
            diag.ShowDialog();
            // Indicator if there were any error while parsing input xls file.
            int errors = 0;
            if (diag.result != DialogResult.OK)
            {
                return false;
            }
            for (int rowIndex = sheet.Cells.FirstRowIndex + 1; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {

                try
                {
                    Row row = new Row();
                    row = sheet.Cells.GetRow(rowIndex);
                    int _yearForSort = 0;
                    string _year = row.GetCell(diag.year).StringValue;
                    _year = _year.Trim();
                    if (yearRangeReg.IsMatch(_year))
                    {
                        _year = yearRangeReg.Match(_year).ToString();
                        char[] stringSeparator = new char[] { '-' };
                        Regex.Replace(_year, " ", "");
                        Regex.Replace(_year, ".", "");
                        string[] tokens = _year.Split(stringSeparator);
                        try
                        {
                            _yearForSort = Int32.Parse(tokens[tokens.Length - 1]);
                        }
                        catch (Exception e)
                        {
                            errors++;
                            logFile.WriteLine("Red " + (rowIndex + 1).ToString() + " nije mogao da bude obradjen.");
                        }
                    }
                    else if (yearManyReg.IsMatch(_year))
                    {
                        _year = yearManyReg.Match(_year).ToString();
                        char[] stringSeparator = new char[] { '.' };
                        Regex.Replace(_year, " ", "");
                        string[] tokens = _year.Split(stringSeparator);
                        if (!tokens[tokens.Length - 2].Equals(""))
                            try
                            {
                                _yearForSort = Int32.Parse(tokens[tokens.Length - 2]);
                            }
                            catch (Exception e)
                            {
                                errors++;
                                logFile.WriteLine("Red " + (rowIndex + 1).ToString() + " nije mogao da bude obradjen.");
                            }
                        else
                            _yearForSort = Int32.Parse(tokens[0]);
                    }
                    else if (yearReg.IsMatch(_year))
                    {
                        _yearForSort = Int32.Parse(_year);
                        _year += ".";
                    }
                    else
                    {
                        errors++;
                        logFile.WriteLine("Red " + (rowIndex + 1).ToString() + " nije mogao da bude obradjen.\nProgram nije mogao da prepozna godinu u tom redu.");
                    }
                    string _categoryNumber = row.GetCell(diag.categoryNum).StringValue;
                    string _fileType = row.GetCell(diag.fileType).StringValue;
                    string _location = row.GetCell(diag.location).StringValue;
                    int _deadline = 0;
                    string _deadlineTmp = row.GetCell(diag.deadline).StringValue;
                    _deadlineTmp.TrimEnd();
                    _deadlineTmp.TrimStart();
                    _deadlineTmp.Trim();
                    if (trajnoReg.IsMatch(_deadlineTmp))
                    {
                        _deadline = 999999;
                    }
                    else _deadline = Int32.Parse(_deadlineTmp);

                    string _categoryName = row.GetCell(diag.categoryName).StringValue;
                    string _fileNumber = row.GetCell(diag.fileNum).StringValue;

                    MyRow tmpRow = new MyRow(_yearForSort, _year, _categoryNumber, _fileType, _location, _categoryName, _deadline, _fileNumber);
                    rows.Add(tmpRow);
                } 
                catch (Exception e)
                {
                    errors++;
                    logFile.WriteLine("Postoji greska u redu: " + (rowIndex + 1).ToString());
                    continue;
                }
                
            }

            rows.Sort();

            Form1.getInstance().progressBar.Minimum = 1;
            Form1.getInstance().progressBar.Maximum = rows.Count;

            List<MyRow> deletedRows = new List<MyRow>();
            
            for (int i = 0; i < rows.Count; i++ )
            {
                if (deletedRows.Contains(rows[i]))
                    continue;
               try
               {
                   Form1.getInstance().progressBar.Value++;
               }
               catch (Exception e)
               {
               }
               int fileAmount = 1;
               for (int j = 0; j < rows.Count; j++)
               {
                    if (i != j)
                    {
                        if (rows[i].Equals(rows[j]))
                        {
                            outputSheet2.Cells[curRowOutput2, curColOutput2++] = new Cell(archiveNumber.ToString());
                            outputSheet2.Cells[curRowOutput2++, curColOutput2++] = new Cell(rows[j].FileNumber);
                            curColOutput2 = 0;

                            fileAmount++;
                            deletedRows.Add(rows[j]);
                            //rows.RemoveAt(j);
                            Form1.getInstance().progressBar.Maximum--;
                        }
                    }
               }
               outputSheet2.Cells[curRowOutput2, curColOutput2++] = new Cell(archiveNumber.ToString());
               outputSheet2.Cells[curRowOutput2++, curColOutput2++] = new Cell(rows[i].FileNumber);
               curColOutput2 = 0;

               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell((archiveNumber++).ToString());
               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(rows[i].Year);
               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(rows[i].CategoryNumber);
               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(rows[i].CategoryName);
               int _deadLine = rows[i].Deadline;
               if (_deadLine == 999999)
                    outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell("trajno");
               else
                   outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(_deadLine.ToString());
               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(rows[i].FileType);
               outputSheet1.Cells[curRowOutput1, curColOutput1++] = new Cell(fileAmount.ToString());
               outputSheet1.Cells[curRowOutput1++, curColOutput1++] = new Cell(rows[i].Location);
               curColOutput1 = 0;

               deletedRows.Add(rows[i]);
               //rows.RemoveAt(i);
               Form1.getInstance().progressBar.Maximum--;
            }
            if (errors > 0)
            {
                MessageBox.Show("Postojale su greske tokom obrade ulaznog fajla, proverite log!");
            }
            return true;      
        }

        private class MyRow : Row, IComparable
        {
            public int YearForSort
            {
                get;
                set;
            }
            public string Year
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
            public int Deadline
            {
                get;
                set;
            }
            public string FileNumber
            {
                get;
                set;
            }

            public MyRow(int _yearForSort, string _year, string _categoryNumber,string _fileType, string _location, string _categoryName,int _deadline, string _fileNumber)
            {
                this.YearForSort = _yearForSort;
                this.Year = _year;
                this.CategoryNumber = _categoryNumber;
                this.FileType = _fileType;
                this.Location = _location;
                this.CategoryName = _categoryName;
                this.Deadline = _deadline;
                this.FileNumber = _fileNumber;
            }

            virtual  public int CompareTo(object obj)
            {
                

                /*if (obj is MyRow)
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
                }*/
                if (obj is MyRow)
                {
                    var compareObj = (MyRow)obj;
                    if (this.YearForSort.CompareTo(compareObj.YearForSort)== 0)
                    {
                        /*if (this.CategoryNumber.CompareTo(compareObj.CategoryNumber) == 0)
                        {
                                return this.FileType.CompareTo(compareObj.FileType);
                        }
                        else return this.CategoryNumber.CompareTo(compareObj.CategoryNumber);*/
                        return - this.Deadline.CompareTo(compareObj.Deadline);
                    } else  return this.YearForSort.CompareTo(compareObj.YearForSort);
                }
                else
                {
                    throw new ArgumentException("Object is not a MyObject ");
                }
            }

            public override bool Equals(object obj)
            {
                bool ret = false;
                if (obj is MyRow)
                {
                    MyRow row = (MyRow)obj;
                    if ((this.Year == row.Year) 
                        && (this.Location.Equals(row.Location))
                        && (this.CategoryNumber.Equals(row.CategoryNumber))
                        && (this.FileType.Equals(row.FileType))
                        && (this.Deadline.Equals(row.Deadline))
                        )
                    {
                        ret = true;
                    }
                    return ret;
                }
                else return ret;
            }


        }


    }
}
