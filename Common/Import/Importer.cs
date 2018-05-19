using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;

namespace QR_Code.Import
{
    /// <summary>
    /// Class responsible for importing data from excel.
    /// Used for changing order nums and organizational units.
    /// </summary>
    internal class Importer
    {
        public IEnumerable<Tuple<string, string>> ImportValues(string filepath, int sheetNum = 0)
        {
            IList<Tuple<string, string>> columnValuePair = new List<Tuple<string, string>>();

            Worksheet sheet = LoadWorkSheet(filepath, sheetNum);
            foreach(Row row in sheet.Cells.Rows.Values)
            {
                yield return Tuple.Create(row.GetCell(0).StringValue, row.GetCell(1).StringValue);
            }
        }

        /// <summary>
        /// Method that loads sheet from xls document on given location and sheet number.
        /// </summary>
        /// <param name="filePath">Location of xls document.</param>
        /// <param name="sheetNum">Number of sheet.</param>
        /// <returns></returns>
        private Worksheet LoadWorkSheet(string filePath, int sheetNum) => Workbook.Load(filePath).Worksheets[sheetNum];
    }
}
