using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI
{

    class Specifikacija5
    {
        #region Variable definitions
        private  string path = "";
        private  string outPath = "";

        private  Workbook book;
        private  Worksheet sheet;
        private  Worksheet outputsheet;
        private  Workbook outputBook;

        public  Regex regContCode = new Regex(@"[0-9]+");
        public  Regex regDate = new Regex(@"[0-9]+\.[0-9]+\.?-[0-9]+\.[0-9]+\.?");
        public  Regex regDateExtended = new Regex(@"[0-9]+-[0-9]+-[0-9]+\/[0-9]+\.[0-9]+\.-[0-9]+\.[0-9]+\.");
        public  Regex regDateEnum = new Regex(@"[0-9]+\.[0-9]+(\.,)?");
        public  Regex regYear = new Regex(@"20[0-9]{2}");

        private  string Page = "";
        private  string Account = "";
        private  string Name = "";
        private  string ContainerCode = "";
        private  string Year = "";
        private  string Description = "";
        private  string Contents = "";
        private  string Details = "";
        private  string DetailsOthers = "";
        private  string Remarks = "";
        private  string[] lines;
        public  int[] years = { 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 };

        // Used for making difference between Remarks and DetailsOthers
        private  string oldRemarks = "";
        private const string POLOVINA = "POLOVINA";


        private  int curRow;
        private  int curCol;

        // Can be used for error checing
        private  int rowNumber;
        private  StreamWriter logFile = new StreamWriter(Application.StartupPath + @"\log.txt");
        #endregion


        public void initialize()
        {
            outPath += Application.StartupPath + @"\outputSpecifikacija5.xls";
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

            // Set it to beginning of the document
            curRow = 0;
            curCol = 0;
            rowNumber = 1;

            // Write default headers
            outputsheet.Cells[curRow, curCol++] = new Cell("Page");
            outputsheet.Cells[curRow, curCol++] = new Cell("Account");
            outputsheet.Cells[curRow, curCol++] = new Cell("Name");
            outputsheet.Cells[curRow, curCol++] = new Cell("ContainerCode");
            outputsheet.Cells[curRow, curCol++] = new Cell("Year");
            outputsheet.Cells[curRow, curCol++] = new Cell("Description");
            outputsheet.Cells[curRow, curCol++] = new Cell("Contents");
            outputsheet.Cells[curRow, curCol++] = new Cell("Details");
            outputsheet.Cells[curRow, curCol++] = new Cell("Details others");
            outputsheet.Cells[curRow++, curCol] = new Cell("Napomena");
            curCol = 0;

        }
        public void convert()
        {
            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                // Read row by row and check if any of rules are fulfilled
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);
                rowNumber++;

                string rowStr = "";
                joinRow(ref rowStr, row);

                if (rowStr.Contains("Page"))
                {
                    managePage(rowStr);
                    continue;
                }

                /*if (rowStr.Contains("Year"))
                {
                    manageYear(ref Year, rowStr);
                    continue;
                }*/
                if (rowStr.Contains("Account:") || rowStr.Contains("Account#£_:"))
                {
                    manageAccAndName(rowStr, rowIndex);
                    continue;
                }

                if (rowStr.Contains("Description:") || rowStr.Contains("Description#£_:"))
                {
                    manageDesc(rowStr);
                    continue;
                }


                if (regContCode.IsMatch(row.GetCell(0).StringValue))
                {
                    manageContCode(row);
                    continue;
                }

                if (rowStr.Contains("Contents") || rowStr.StartsWith("-"))
                {
                    manageCont(rowIndex, rowStr);
                    continue;
                }

                if (regDate.IsMatch(rowStr) || regDateExtended.IsMatch(rowStr) || regDateEnum.IsMatch(rowStr))
                {
                    manageDetails(rowStr, rowIndex);
                    continue;
                }

                // Old Remarks are sometimes mixed with Details Others
                if (!(isFooterOrHeader(rowStr)))
                {
                    manageDetailsOthers(rowStr, rowIndex);
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
            MessageBox.Show("Uspesno kreirana izlazna datoteka pod imenom outputSpecifikacija5!");
            logFile.Close();
        }
        private  void managePage(string rowStr)
        {
            try
            {
                string tmp = "";
                string[] stringSeparators = new string[] { "Page" };
                string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                    tmp = tokens[1];
                else if (tokens.Length == 1)
                {
                    // Is number?
                    if (Regex.IsMatch(tokens[0], @"^\d+$"))
                        tmp = tokens[0];
                }

                Page = Regex.Replace(tmp, "-", "");
            }
            catch (Exception e)
            {
                rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
            }
        }

        private  void manageAccAndName(string rowStr, int rowIndex)
        {
            string[] stringSeparators;
            string[] tokens;
            if (rowStr.Contains("Name"))
            {
                stringSeparators = new string[] { "Name" };
                try
                {
                    tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 2)
                        Name = Regex.Replace(tokens[1], ":", "");
                    else if (tokens.Length == 1)
                        Name = "[Hipo]";

                    stringSeparators = new string[] { "Account" };
                    tokens = tokens[0].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    Account = Regex.Replace(tokens[0], ":", "");
                }
                catch (Exception e)
                {
                    rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                    logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
                }

            }
            else
            {
                try
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
                catch (Exception e)
                {
                    rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                    logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
                }
            }



        }
        private  void manageContCode(Row row)
        {
            if (Regex.IsMatch(row.GetCell(0).StringValue, @"^\d+$"))
            {
                ContainerCode = row.GetCell(0).StringValue;
                // Description is not bound to Name and Acc but for Container Code, replacement made last night 
                // Same for Contents
                Description = "";
                Contents = "";
                // Does it contain Year?
                // Start searching for Cell 1 because in Cell 0 is Container Code
                for (int i = 1; i <= row.LastColIndex; i++)
                {
                    if (regYear.IsMatch(row.GetCell(i).StringValue))
                    {
                        Year = row.GetCell(i).StringValue;
                    }
                }
            }

        }

        private  void manageDesc(string rowStr)
        {

            try
            {
                string tmp = "";
                string[] stringSeparators = new string[] { "Description:" };
                string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 1)
                {
                    tmp = tokens[0];
                    Description = Regex.Replace(tmp, "-", "");
                }
                else
                    Description = "";
            }
            catch (Exception e)
            {
                rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
            }

        }

        private  void manageCont(int rowIndex, string rowStr)
        {
            if (rowStr.StartsWith("-"))
            {
                Contents = rowStr.Substring(1);
                Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                string nextRowStr = "";
                joinRow(ref nextRowStr, nextRow);
                if (nextRowStr.StartsWith("-") || regContCode.IsMatch(nextRow.GetCell(0).StringValue))
                {
                    printToSheet();
                }
            }
            else
            {

                try
                {
                    string tmp = "";
                    string[] stringSeparators = new string[] { "Contents:" };
                    string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tmp = tokens[0];
                    Contents = Regex.Replace(tmp, "-", "");

                    Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                    string nextRowStr = "";
                    joinRow(ref nextRowStr, nextRow);
                    if (nextRowStr.StartsWith("-") || regContCode.IsMatch(nextRow.GetCell(0).StringValue))
                    {
                        printToSheet();
                    }
                }
                catch (Exception e)
                {
                    rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                    logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
                }
            }
            Details = "";
            Remarks = "";
        }

        private  void manageDetails(string rowStr, int rowIndex)
        {
            Details = rowStr;
            Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
            string nextRowStr = "";
            joinRow(ref nextRowStr, nextRow);

            // Next row is Remarks if it's not any of the rules
            if (!(isRule(nextRowStr)))
            {
                Remarks = nextRowStr;
                printToSheet();
                oldRemarks = Remarks;
                Remarks = "";
                Details = "";
            }
            else
            {
                printToSheet();
                Details = "";
            }
        }

        private  bool isYear(string s)
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

        private  void manageDetailsOthers(string rowStr, int rowIndex)
        {
            //!regYear.IsMatch(rowStr)
            if (!rowStr.Equals(POLOVINA) && !isYear(rowStr) && !rowStr.Equals(oldRemarks))
            {
                // If details others contains more than one line it should be merged into one cell
                // Multi lines start with #[ and end with #] 
                // Everything between is considered as one deail
                if (rowStr.Contains("#["))
                {
                    DetailsOthers = "";
                    string[] stringSeparators = new string[] { "#[" };
                    string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    DetailsOthers += tokens[0] + "[#£_]";

                    Row row = new Row();
                    row = sheet.Cells.GetRow(++rowIndex);
                    rowNumber++;
                    rowStr = "";
                    joinRow(ref rowStr, row);

                    while (!rowStr.Contains("#]"))
                    {
                        DetailsOthers += rowStr + "[#£_]";

                        row = new Row();
                        row = sheet.Cells.GetRow(++rowIndex);
                        rowNumber++;
                        rowStr = "";
                        joinRow(ref rowStr, row);

                    }
                    stringSeparators = new string[] { "#]" };
                    tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    DetailsOthers += tokens[0] + "[#£_]";

                    printToSheet();
                    DetailsOthers = "";
                }
                else
                {
                    DetailsOthers = rowStr;
                    Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                    string nextRowStr = "";
                    joinRow(ref nextRowStr, nextRow);

                    // Next row is Remarks if it's not any of the rules

                    printToSheet();
                    DetailsOthers = "";
                }
            }
            else
            {
                // Remove #£_ from Log file
                rowStr = Regex.Replace(rowStr, @"[#£_]+", " ");
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + Regex.Replace(rowStr, @"[#£_]+", " ") + " nije mogao da bude obradjen pa je preskocen.\n\n");
            }
        }

        private  void joinRow(ref string rowStr, Row row)
        {
            for (int i = row.FirstColIndex; i <= row.LastColIndex; i++)
            {
                rowStr += row.GetCell(i).StringValue;
            }
            rowStr = Regex.Replace(rowStr, @"\s+", @"#£_");
        }

        private  void printToSheet()
        {
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Page, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Account, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Name, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(ContainerCode, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Year, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Description, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Contents, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Details, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(DetailsOthers, @"[#£_]+", " "));
            outputsheet.Cells[curRow++, curCol++] = new Cell(Regex.Replace(Remarks, @"[#£_]+", " "));
            // Return to first item in row
            curCol = 0;
        }

        private  bool isRule(string rowStr)
        {
            return rowStr.Contains("Account") || rowStr.Contains("Name:") || rowStr.Contains("Description")
                || regContCode.IsMatch(rowStr)
                || rowStr.Contains("Contents")
                || regDate.IsMatch(rowStr)
                || regDateExtended.IsMatch(rowStr)
                || regDateEnum.IsMatch(rowStr)
                || rowStr.StartsWith("-")
                || rowStr.Contains("Page")
                || isFooterOrHeader(rowStr);
        }

        private  bool isFooterOrHeader(string rowStr)
        {
            return rowStr.Contains("Destroyed")
                || rowStr.Contains("ACCOUNT")
                || rowStr.Contains("Summary")
                || rowStr.Contains("Specifikacija")
                || rowStr.Contains("ontainer")
                || rowStr.Contains("odeYear")
                || rowStr.Equals("");
        }

        public void setPath(String path)
        {
            this.path = path;
        }
    }
}
