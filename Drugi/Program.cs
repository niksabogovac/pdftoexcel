using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelLibrary;

using QiHe.CodeLib;
using ExcelLibrary.SpreadSheet;

using System.Text.RegularExpressions;


namespace Drugi
{
    class Program
    {
        #region Variable definitions
        public static string path = "";
        public static string outPath = "";

        public static Workbook book;
        public static Worksheet sheet;
        public static Worksheet outputsheet;
        public static Workbook outputBook;

        public static Regex regContCode = new Regex(@"[0-9]+");
        public static Regex regDate = new Regex(@"[0-9]+\.[0-9]+\.?-[0-9]+\.[0-9]+\.?");
        public static Regex regDateExtended = new Regex(@"[0-9]+-[0-9]+-[0-9]+\/[0-9]+\.[0-9]+\.-[0-9]+\.[0-9]+\.");
        public static Regex regDateEnum = new Regex(@"[0-9]+\.[0-9]+(\.,)?");
        public static Regex regYear = new Regex(@"20[0-9]{2}");

        public static string Page = "";
        public static string Account = "";
        public static string Name = "";
        public static string ContainerCode = "";
        public static string Year = "";
        public static string Description = "";
        public static string Contents = "";
        public static string Details = "";
        public static string DetailsOthers = "";
        public static string Remarks = "";
        public static string[] lines;
        public static int[] years = { 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 };

        // Used for making difference between Remarks and DetailsOthers
        public static string oldRemarks = "";
        public const string POLOVINA = "POLOVINA";


        public static int curRow;
        public static int curCol;

        // Can be used for error checing
        public static int rowNumber;
        public static System.IO.StreamWriter logFile = new System.IO.StreamWriter(Application.StartupPath + @"\log.txt");




        #endregion

        static void Main(string[] args)
        {
            #region Preparing input and output files

            if (System.IO.File.Exists("Contents.txt"))
            {
                lines = ReadFromFile.ParseFile("Contents.txt");
            }

            //print array of strings
            //TODO implement it as regexp readed from external file
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            // Create input and output path
            path += Application.StartupPath;
            outPath += Application.StartupPath + @"\outputSpecifikacija5.xls";
            Console.WriteLine("Unesite ime ulaznog xls fajla: ");
            string filename = "";
            filename = Console.ReadLine();
            path += @"\" + filename + ".xls";


            // Open WorkBook with input path
            book = new Workbook();
            try
            {
                book = Workbook.Load(path);
            }
            catch
            {
                Console.WriteLine("Uneti fajl ne moze da se otvori.\nProverite da li ste uneli ispravno ime.\nProverite da li je vec otvoren i ukoliko jeste zatvorite ga i pokusajte ponovo.");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            // Open first sheet in input xls document 
            try
            {
                sheet = book.Worksheets[0];
            }
            catch
            {
                Console.WriteLine("Uneti fajl ne moze da se otvori.\nUlazni fajl mora da sadrzi sve u prvom sheetu!");
                Console.ReadLine();
                Environment.Exit(-1);
            }


            // Create default output sheet and workbook
            outputsheet = new Worksheet("Output");
            outputBook = new Workbook();

            // Set it to beginning of the document
            curRow = 0;
            curCol = 0;
            rowNumber = 0;

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

            #endregion

            #region Main loop
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
            #endregion

            #region Write to output file
            try
            {
                outputBook.Worksheets.Add(outputsheet);
                outputBook.Save(outPath);
            }
            catch
            {
                Console.WriteLine("Nije moguce upisati u izlazni fajl.\nOn se kreira u datoteci gde je i ulazni.\nZatvorite ga i pokusajte ponovo.");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            Console.WriteLine("Uspesno kreirana izlazna datoteka pod imenom outputSpecifikacija5!");
            Console.ReadLine();
            #endregion
        }




        private static void manageYear(string rowStr)
        {
            try
            {
                string tmp = "";
                string[] stringSeparators = new string[] { "Year" };
                string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                tmp = tokens[1];
                Year = Regex.Replace(tmp, "-", "");
            }
            catch { }
        }

        private static void managePage(string rowStr)
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

        private static void manageAccAndName(string rowStr, int rowIndex)
        {
            string[] stringSeparators;
            string[] tokens;
            if (rowStr.Contains("Name:"))
            {
                stringSeparators = new string[] { "Name:" };
                tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                    Name = tokens[1];
                else if (tokens.Length == 1)
                    Name = "[Hipo]";

                stringSeparators = new string[] { "Account:" };
                tokens = tokens[0].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Account = tokens[0];

            }
            else
            {
                stringSeparators = new string[] { "Account:" };
                tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Account = tokens[0];

                Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                string nextRowStr = "";
                joinRow(ref nextRowStr, nextRow);

                if (nextRowStr.Contains("Name:"))
                {
                    stringSeparators = new string[] { "Name:" };
                    tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    Name = tokens[0];
                }
            }



        }

        private static void manageContCode(Row row)
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

        private static void manageDesc(string rowStr)
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

        private static void manageCont(int rowIndex, string rowStr)
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
            Details = "";
            Remarks = "";
        }

        private static void manageDetails(string rowStr, int rowIndex)
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

        private static bool isYear(string s)
        {
            if (s.Length > 0)
            {
                for (int i=0; i<years.Length; i++)
                {
                    if (String.Equals(s, years[i].ToString(), StringComparison.Ordinal)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void manageDetailsOthers(string rowStr, int rowIndex)
        {
            //!regYear.IsMatch(rowStr)
            if (!rowStr.Equals(POLOVINA) && !isYear(rowStr) && !rowStr.Equals(oldRemarks))
            {
                DetailsOthers = rowStr;
                Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
                string nextRowStr = "";
                joinRow(ref nextRowStr, nextRow);

                // Next row is Remarks if it's not any of the rules

                printToSheet();
                DetailsOthers = "";
            }
            else
            {
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + rowStr + " nije mogao da bude obradjen pa je preskocen.\n\n");
            }
        }

        private static void joinRow(ref string rowStr, Row row)
        {
            for (int i = row.FirstColIndex; i <= row.LastColIndex; i++)
            {
                rowStr += row.GetCell(i).StringValue;
            }
            rowStr = Regex.Replace(rowStr, @"\s+", @"#£_");
        }

        private static void printToSheet()
        {
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Page,@"[#£_]+", " "));
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

        private static bool isRule(string rowStr)
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

        private static bool isFooterOrHeader(string rowStr)
        {
            return rowStr.Contains("Destroyed")
                || rowStr.Contains("ACCOUNT")
                || rowStr.Contains("Summary")
                || rowStr.Contains("Specifikacija")
                || rowStr.Contains("Container")
                || rowStr.Contains("Code");
        }
    }
}
