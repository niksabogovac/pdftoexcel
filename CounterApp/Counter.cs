using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CounterApp
{
    public class Counter
    {
        string filepath;
        private string outPath;

        private Workbook book;
        private Worksheet sheet;


        private int count1 = 0, count2 = 0;

        private List<string> text1 = new List<string>();
        private List<string> text2 = new List<string>();

        public Counter(string f)
        {
            filepath = f;
        }

        public void LoadTableData()
        {
            outPath += Application.StartupPath;
            book = new Workbook();
            try
            {
                book = Workbook.Load(filepath);
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


        }

        public void Count()
        {
            char[] separator = {','};
            //string[] tokens = expression.Split(separator);
            Regex reg1 = new Regex("1623[0-9]{6}");
            Regex reg2 = new Regex("1624[0-9]{6}");


            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);

                string rowStr = "";
                JoinRow(ref rowStr, row);
                if (reg1.IsMatch(rowStr))
                {
                    MatchCollection collection = reg1.Matches(rowStr);
                    foreach (Match item in collection)
                    {
                        text1.Add(item.ToString());
                        count1++;
                    }
                }
                if (reg2.IsMatch(rowStr))
                {
                    MatchCollection collection = reg2.Matches(rowStr);
                    foreach (Match item in collection)
                    {
                        text2.Add(item.ToString());
                        count2++;
                    }
                }
            }
        }

        public void Write()
        {
            System.IO.File.WriteAllLines(outPath + "1623.txt",text1.ToArray());
            System.IO.File.WriteAllLines(outPath + "1624.txt", text2.ToArray());
            MessageBox.Show("Uspesno kreirana izlazna datoteke!");
        }

        private void JoinRow(ref string rowStr, Row row)
        {
            for (int i = row.FirstColIndex; i <= row.LastColIndex; i++)
            {
                rowStr += row.GetCell(i).StringValue;
            }
            rowStr = Regex.Replace(rowStr, @"\s+", @"#£_");
        }
    }
}
