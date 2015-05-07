using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;
using System.Text.RegularExpressions;

namespace Specifikacija4
{
   public class Program
    {
        #region Variable definitions
        public static string path = "";
        public static string outPath = "";

        public static Workbook book;
        public static Worksheet sheet;
        public static Worksheet outputsheet;
        public static Workbook outputBook;

        public static string Page = "";
        public static string Account = "";
        public static string Name = "";
        public static string ContainerCode = "";
        public static string Year = "";
        public static string Description = "";
        public static string DescriptionMain = "";
        public static string Contents = "";
        public static string FileFolderCode = "";

        public static Regex regFileFolderCode = new Regex(@"[0-9]{12}");
        public static Regex regDescriptionMain = new Regex(@"([A-Za-z]+\t?)+");
        public static Regex regDescriptionMainNumeric = new Regex(@"([0-9]+-[0-9]+-[0-9]{1,5},?)+");
        public static Regex regYear = new Regex(@"20[0-9]{2}");
        public static Regex regNumber = new Regex(@"\d+");

        public static int curRow;
        public static int curCol;

        public static int[] years = { 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 };

        // Can be used for error checing
        public static int rowNumber;
        public static System.IO.StreamWriter logFile = new System.IO.StreamWriter(Application.StartupPath + @"\log.txt");

        #endregion




       static void Main(string[] args)
        {

            #region Preparing input and output files
            path += Application.StartupPath;
            outPath += Application.StartupPath + @"\outputSpecifikacija4.xls";
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
            outputsheet.Cells[curRow, curCol++] = new Cell("Description1");
            outputsheet.Cells[curRow, curCol++] = new Cell("Description");
            outputsheet.Cells[curRow, curCol++] = new Cell("Contents");
            outputsheet.Cells[curRow++, curCol] = new Cell("FileFolderCode");
            curCol = 0;

            #endregion

            #region Main loop
            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
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
            Console.WriteLine("Uspesno kreirana izlazna datoteka pod imenom outputSpecifikacija4!");
            Console.ReadLine();
            #endregion

        }

       public static void manageFileFolderCode(Row row, int rowIndex)
        {
            FileFolderCode = row.GetCell(0).StringValue;

            // New box reset all values
            Description = "";
            DescriptionMain = "";
            Contents = "";
            Year = "";

            DescriptionMain = row.GetCell(2).StringValue;
            ContainerCode = row.GetCell(3).StringValue;
            Year = row.GetCell(4).StringValue;


            // Whole idea not working, check row 161 
            // DescriptionMain - the one in the same row as FileFoled Code can be numerical
            // Instead of that - switch to get info from every cell by itself
            // Errors can appear

            /*
            // Create string from input row
            string rowStr = "";
            joinRow(ref rowStr, row);

            // Dispose the matched FileFolder Code from input rowStr
            rowStr = regFileFolderCode.Replace(rowStr, "");

            if (regDescriptionMain.IsMatch(rowStr))
            {
                Match m = regDescriptionMain.Match(rowStr);
                string tmp = "";
                while(m.Success)
                {
                    tmp += m.Value + "#£_";
                    m = m.NextMatch();
                }
                DescriptionMain = tmp;
                // Dispose the matched Main Description from input rowStr
                rowStr = regDescriptionMain.Replace(rowStr, "");
            }

            if (regDescriptionMainNumeric.IsMatch(rowStr)) ;
            {
                Match m = regDescriptionMainNumeric.Match(rowStr);
                string tmp = "";
                while (m.Success)
                {
                    tmp += m.Value + "#£_";
                    m = m.NextMatch();
                }
                DescriptionMain = tmp;
                // Dispose the matched Main Description from input rowStr
                rowStr = regDescriptionMainNumeric.Replace(rowStr, "");
            }
            if (regYear.IsMatch(rowStr))
            {
                Match m = regYear.Match(rowStr);
                while (m.Success)
                {
                    if (isYear(m.Value))
                    {
                        Year = m.Value + "#£_";
                        m = m.NextMatch();
                    }
                }
                // Dispose the matched Year or Years from input rowStr
                rowStr = regYear.Replace(rowStr, "");
            }
            if(regNumber.IsMatch(rowStr))
            {
                Match m = regNumber.Match(rowStr);
                ContainerCode = m.Value;

                rowStr = regNumber.Replace(rowStr, "");
                rowStr = Regex.Replace(rowStr, @"#£_", "");
                rowStr = Regex.Replace(rowStr, @"\s+", "");

                if (!rowStr.Equals(""))
                {
                    // Collected all the data
                    // Something else in line - write it to log
                    logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + "je nakon obrade i popunjavanja svih podataka ostavio: " + rowStr + " .\n\n");
                }
            }
            */
            Row nextRow = sheet.Cells.GetRow(rowIndex + 1);
            string nextRowStr = "";
            joinRow(ref nextRowStr, nextRow);

            // Next row is another File folder, current row doesn't have description or contents
            if (regFileFolderCode.IsMatch(nextRow.GetCell(0).StringValue) || nextRowStr.Equals("") || isFooterOrHeader(nextRowStr))
            {
                printToSheet();
                return;
            } else if (nextRowStr.Contains("Description:"))
            {
                manageDescription(nextRowStr, nextRow, ++rowIndex);
            } else if (nextRowStr.Contains("Contents:"))
            {
                manageContents(++rowIndex);
            }
        }

        public static void manageContents(int rowIndex)
        {
            Row row = sheet.Cells.GetRow(rowIndex);
            string rowStr = "";
            joinRow(ref rowStr, row);

            //string tmp = "";
            string[] stringSeparators = new string[] { "Contents:" };
            string[] tokens = rowStr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            //tmp = tokens[0];
            //Contents = Regex.Replace(tmp, "-", "");
            Contents = tokens[0];
            printToSheet();
        }

        public static void manageDescription(string currentRowStr, Row currentRow, int rowIndex)
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
                        //tmp = tokens[0];
                        //Description = Regex.Replace(tmp, "-", "");
                        Description = tokens[0];
                    }
                    else
                    {
                        Description = "";
                        break;
                    }
                    row = sheet.Cells.GetRow(++rowIndex);
                    rowStr = "";
                    joinRow(ref rowStr, row);
                }
                else
                {
                    // Used for knowing if description was in more than 1 line
                    Description += rowStr + "&&&&\n";

                    row = sheet.Cells.GetRow(++rowIndex);
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


        public static void manageAccAndName(string rowStr, int rowIndex)
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

        public static void managePage(string rowStr)
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
                logFile.WriteLine("Red u ulaznoj datoteci sa brojem  " + rowNumber + ": " + rowStr + " ne sadrzi broj stranice ili rec Page.\n\n");
            }
        }

        public static void joinRow(ref string rowStr, Row row)
        {
            for (int i = row.FirstColIndex; i <= row.LastColIndex; i++)
            {
                rowStr += row.GetCell(i).StringValue;
            }
            rowStr = Regex.Replace(rowStr, @"\s+", @"#£_");
        }

        public static bool isYear(string s)
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

        public static void printToSheet()
        {
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Page, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Account, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Name, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(ContainerCode, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Year, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Description, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(Contents, @"[#£_]+", " "));
            outputsheet.Cells[curRow, curCol++] = new Cell(Regex.Replace(DescriptionMain, @"[#£_]+", " "));
            outputsheet.Cells[curRow++, curCol++] = new Cell(Regex.Replace(FileFolderCode, @"[#£_]+", " "));
            // Return to first item in row
            curCol = 0;
        }
        public static bool isFooterOrHeader(string rowStr)
        {
            bool ret = false;
            if (rowStr.Contains("Destroyed") || rowStr.Contains("Summary") || rowStr.Contains("Outsourcing"))
                ret = true;
            return ret;
        }
    }
}
