using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;
using System.Text.RegularExpressions;
using System.IO;
namespace GUI
{
    class Specifikacija4
    {
        #region Variable definitions
        private string path;
        private string outPath;

        private Workbook book;
        private Worksheet sheet;
        private Worksheet outputsheet;
        private Workbook outputBook;

        private string Page;
        private string Account;
        private string Name;
        private string ContainerCode;
        private string Year;
        private string Description;
        private string DescriptionMain;
        private string Contents;
        private string FileFolderCode;

        public static Regex regFileFolderCode = new Regex(@"[0-9]{12}");
        public static Regex regDescriptionMain = new Regex(@"([A-Za-z]+\t?)+");
        public static Regex regDescriptionMainNumeric = new Regex(@"([0-9]+-[0-9]+-[0-9]{1,5},?)+");
        public static Regex regYear = new Regex(@"20[0-9]{2}");
        public static Regex regNumber = new Regex(@"\d+");
        public static int[] years = { 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 };


        private int curRow;
        private int curCol;

        private int DescriptionMainColumn;
        private int ContainerCodeColumn;
        private int YearColumn;

        // Can be used for error checking
        private int rowNumber;
        private StreamWriter logFile;
        #endregion


        public Specifikacija4()
        {
            path = "";
            outPath = "";

            Page = "";
            Account = "";
            Name = "";
            ContainerCode = "";
            Year = "";
            Description = "";
            DescriptionMain = "";
            Contents = "";
            FileFolderCode = "";

            curRow = 0;
            curCol = 0;
            rowNumber = 0;

            logFile = new StreamWriter(Application.StartupPath + @"\log.txt");

            DescriptionMainColumn = 0;
            ContainerCodeColumn = 0;
            YearColumn = 0;
        }


        public void initialize()
        {
            outPath += Application.StartupPath + @"\outputSpecifikacija4.xls";
            /*path += Application.StartupPath;
            Console.WriteLine("Unesite ime ulaznog xls fajla: ");
            string filename = "";
            filename = Console.ReadLine();
            path += @"\" + filename + ".xls";*/
            
            // Open WorkBook with input path
            book = new Workbook();
            try
            {
                book = Workbook.Load(path);
            }
            catch
            {
                MessageBox.Show("Uneti fajl ne moze da se otvori.\nProverite da li ste uneli ispravno ime.\nProverite da li je vec otvoren i ukoliko jeste zatvorite ga i pokusajte ponovo.");
                Environment.Exit(-1);
            }

            // Open first sheet in input xls document 
            try
            {
                sheet = book.Worksheets[0];
            }
            catch
            {
                MessageBox.Show("Uneti fajl ne moze da se otvori.\nUlazni fajl mora da sadrzi sve u prvom sheetu!");
                Environment.Exit(-1);
            }

            // Create default output sheet and workbook
            outputsheet = new Worksheet("Output");
            outputBook = new Workbook();

            #region Ask for column specification
            ChooseDiag diag = new ChooseDiag();
            diag.ShowDialog();
            DescriptionMainColumn = diag.DescriptionMainColumn;
            ContainerCodeColumn = diag.ContainerCodeColumn;
            YearColumn = diag.YearColumn;
            DescriptionMainColumn--;
            ContainerCodeColumn--;
            YearColumn--;

            #endregion

            // Write default headers
            outputsheet.Cells[curRow, curCol++] = new Cell("Page");
            outputsheet.Cells[curRow, curCol++] = new Cell("Account");
            outputsheet.Cells[curRow, curCol++] = new Cell("Name");
            outputsheet.Cells[curRow, curCol++] = new Cell("ContainerCode");
            outputsheet.Cells[curRow, curCol++] = new Cell("Year");
            outputsheet.Cells[curRow, curCol++] = new Cell("Description1");
            outputsheet.Cells[curRow, curCol++] = new Cell("Description");
            outputsheet.Cells[curRow, curCol++] = new Cell("Contents");
            outputsheet.Cells[curRow++, curCol] = new Cell("FileFolderCode");
            curCol = 0;

        }

        public void convert()
        {
            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);
                rowNumber++;

                string rowStr = "";
                joinRow(ref rowStr, row);

                if (rowStr.Contains("Page") || rowStr.Contains("PAGE"))
                {
                    managePage(rowStr);
                    continue;
                }

                if (rowStr.Contains("Account:") || rowStr.Contains("Account#£_:"))
                {
                    manageAccAndName(rowStr, rowIndex);
                    continue;
                }

                if (regFileFolderCode.IsMatch(row.GetCell(0).StringValue))
                {
                    manageFileFolderCode(row, rowIndex);
                    continue;
                }
            }
        }

        public void write()
        {
            try
            {
                outputBook.Worksheets.Add(outputsheet);
                outputBook.Save(outPath);
            }
            catch
            {
                MessageBox.Show("Nije moguce upisati u izlazni fajl.\nOn se kreira u datoteci gde je i ulazni.\nZatvorite ga i pokusajte ponovo.");
                Environment.Exit(-1);
            }
            MessageBox.Show("Uspesno kreirana izlazna datoteka pod imenom outputSpecifikacija4!");
            logFile.Close();
        }

        private void manageFileFolderCode(Row row, int rowIndex)
        {
            FileFolderCode = row.GetCell(0).StringValue;

            // New box reset all values
            Description = "";
            DescriptionMain = "";
            Contents = "";
            ContainerCode = "";
            Year = "";

            DescriptionMain = row.GetCell(DescriptionMainColumn).StringValue;
            ContainerCode = row.GetCell(ContainerCodeColumn).StringValue;
            Year = row.GetCell(YearColumn).StringValue;

            Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
            string nextRowStr = "";
            joinRow(ref nextRowStr, nextRow);

            // Next row is another File folder, current row doesn't have description or contents
            if (regFileFolderCode.IsMatch(nextRow.GetCell(0).StringValue) || nextRowStr.Equals("") || isFooterOrHeader(nextRowStr))
            {
                printToSheet();
                return;
            }
            else if (nextRowStr.Contains("Description"))
            {
                manageDescription(nextRowStr, nextRow, ++rowIndex);
                rowNumber++;
            }
            else if (nextRowStr.Contains("Contents"))
            {
                manageContents(++rowIndex);
                rowNumber++;
            }
        }

        private void manageContents(int rowIndex)
        {
            Row row = sheet.Cells.GetRow(rowIndex);
            string rowStr = "";
            joinRow(ref rowStr, row);

            string[] stringSeparators = new string[] { "Contents:" };
            string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            

            // Sometimes Year is written on purpose in Contents row check that
            // Also OCR misses the row and sometimes fills it in Description row - can't fix that
            try
            {
                if (isYear(tokens[0]))
                {
                    Year = tokens[0];
                }
                else
                    Contents = tokens[0];
            } catch (IndexOutOfRangeException e)
            {
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + "  sadrzi prazan Contents pa nije mogao da bude obradjen.\n\n");
            }
            printToSheet();
        }

        private void manageDescription(string currentRowStr, Row currentRow, int rowIndex)
        {
            // Counter for row parsing
            // Needs to be set to 1 because function sends the next row index when it is called but it is not called the first time 

            int i = 1;
            string rowStr = currentRowStr;
            Row row = currentRow;

            bool first = true;

            // while the rows are not another file folder footer/header or contents write it to description
            do
            {
                if (first)
                {
                    first = false;
                    //string tmp = "";

                    string[] stringSeparators = new string[] { "Description:" };
                    string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 1)
                    {
                        Description = tokens[0];
                    }
                    else
                    {
                        Description = "";
                        break;
                    }
                    row = sheet.Cells.GetRow(++rowIndex);
                    rowNumber++;
                    rowStr = "";
                    joinRow(ref rowStr, row);
                }
                else
                {
                    // Used for knowing if description was in more than 1 line
                    Description += rowStr + "&&&&\n";

                    row = sheet.Cells.GetRow(++rowIndex);
                    rowNumber++;
                    rowStr = "";
                    joinRow(ref rowStr, row);
                }
            } while (!(regFileFolderCode.IsMatch(row.GetCell(0).StringValue) || rowStr.Equals("") || isFooterOrHeader(rowStr) || rowStr.Contains("Contents:")));


            // If next is contents take info from that first then write it
            // else just write it
            if (rowStr.Contains("Contents:"))
            {
                manageContents(rowIndex);
            }
            else
                printToSheet();
        }

        private void manageAccAndName(string rowStr, int rowIndex)
        {
            string[] stringSeparators;
            string[] tokens;
            if (rowStr.Contains("Name"))
            {
                stringSeparators = new string[] { "Name" };
                tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                    Name = Regex.Replace(tokens[1], ":", "");
                else if (tokens.Length == 1)
                    Name = "[Hipo]";

                stringSeparators = new string[] { "Account" };
                tokens = tokens[0].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Account = Regex.Replace(tokens[0], ":", "");

            }
            else
            {
                stringSeparators = new string[] { "Account" };
                tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Account = Regex.Replace(tokens[0], ":", "");

                Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                string nextRowStr = "";
                joinRow(ref nextRowStr, nextRow);

                if (nextRowStr.Contains("Name"))
                {
                    stringSeparators = new string[] { "Name" };
                    tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    Name = Regex.Replace(tokens[0], ":", "");
                }
            }



        }

        private void managePage(string rowStr)
        {
            string[] tokens;
            string[] stringSeparators = new string[] { "Page" };
            try
            {
                tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Page = tokens[1];
            }
            catch
            {
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " ne sadrzi broj stranice ili rec Page.\n\n");
            }
        }

        private void joinRow(ref string rowStr, Row row)
        {
            for (int i = row.FirstColIndex; i <= row.LastColIndex; i++)
            {
                rowStr += row.GetCell(i).StringValue;
            }
            rowStr = Regex.Replace(rowStr, @"\s+", @"#£_");
        }

        private bool isYear(string s)
        {
            if (s.Length > 0)
            {
                for (int i = 0; i < years.Length; i++)
                {
                    if (String.Equals(s, years[i].ToString(), StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void printToSheet()
        {
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Page, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Account, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Name, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(ContainerCode, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Year, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Description, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(DescriptionMain, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Contents, @"[#£_]+", " "));
            outputsheet.Cells[curRow++, curCol++] = new Cell(Regex.Replace(FileFolderCode, @"[#£_]+", " "));
            // Return to first item in row
            curCol = 0;
        }
        private bool isFooterOrHeader(string rowStr)
        {
            bool ret = false;
            if (rowStr.Contains("Destroyed") || rowStr.Contains("Summary") || rowStr.Contains("Outsourcing"))
                ret = true;
            return ret;
        }

        public void setPath(String path)
        {
            this.path = path;
        }
    }
}
