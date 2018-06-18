using ExcelLibrary.SpreadSheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class ReportManager
    {
        #region Private members

        private static readonly int FOLL_ID_COL = 1;
        private static readonly int CLIENT_ID_COL = 2;
        private static readonly int CLIENT_NAME_COL = 3;
        private static readonly int ACCOUNT_ID_COL = 4;
        private static readonly int CASE_TYPE_COL = 5;
        private static readonly int DOCUMENT_OBJ_ID_COL = 6;

        /// <summary>
        /// List of needed columns to be read from sql database PartCodes.
        /// </summary>
        private static IList<string> sqlDbColumnNames = new List<string>()
        {
            "ID",
            "OrderNum",
            "TakeoverDate",
            "BoxCode",
            "FileNumber",
            "OrganizationalUnit"
        };

        /// <summary>
        /// List of header names generated in report.
        /// </summary>
        private static IList<string> reportHeaderNames = new List<string>()
        {
            "ID",
            "Broj naloga",
            "Datum primopredaje",
            "Kutija",
            "File number",
            "Organizaciona jedinica"
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Imports data from excel document to database.
        /// </summary>
        /// <param name="filePath">Path of file to import</param>
        /// <param name="jmbg">Unique identifier of currenly logged user.</param>
        public static bool ImportData(string filePath, string jmbg)
        {
            StringBuilder log = new StringBuilder();

            Workbook book;
            Worksheet sheet;
            try
            {
                book = Workbook.Load(filePath);
                sheet = book.Worksheets[0];
            }
            catch (Exception)
            {
                return false;
            }


            // Skip headers.
            for (int rowIndex = sheet.Cells.FirstRowIndex + 1; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = new Row();
                row = sheet.Cells.GetRow(rowIndex);

                JObject jObject = new JObject();


                string id, orderNum, boxCode, fileNum;
                DateTime date;
                using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM PartCodes WHERE ID = @id AND FileNumber IS NOT NULL", DatabaseManager.SqlConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", row.GetCell(FOLL_ID_COL).StringValue);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            id = (string)reader["ID"];
                            orderNum = (string)reader["OrderNum"];
                            boxCode = (string)reader["BoxCode"];
                            fileNum = (string)reader["FileNumber"];
                            date = (DateTime)reader["Date"];

                            jObject["id"] = row.GetCell(FOLL_ID_COL).StringValue;
                            jObject["doctype"] = row.GetCell(CASE_TYPE_COL).StringValue;
                            if (!string.IsNullOrWhiteSpace(row.GetCell(CLIENT_ID_COL).StringValue))
                            {
                                jObject["mbr"] = row.GetCell(CLIENT_ID_COL).StringValue;
                            }

                            if (!string.IsNullOrWhiteSpace(row.GetCell(ACCOUNT_ID_COL).StringValue))
                            {
                                jObject["partija"] = row.GetCell(ACCOUNT_ID_COL).StringValue;
                            }
                            if (!string.IsNullOrWhiteSpace(row.GetCell(CLIENT_NAME_COL).StringValue))
                            {
                                jObject["mbrid"] = row.GetCell(CLIENT_NAME_COL).StringValue;
                            }
                        }
                        else
                        {
                            log.AppendFormat("Red iz tabele: {0} sa ID: {1} nije učitan iz baze jer ne postoji skeniran kod sa tim ID-jem.\n", rowIndex, row.GetCell(FOLL_ID_COL).StringValue);
                            continue;
                        }
                    }
                }

                SqlTransaction sqlTransaction = DatabaseManager.SqlConnection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO BankTable VALUES (@id,@orderNum, @boxCode, @date,@mbr,@qrCode)", DatabaseManager.SqlConnection, sqlTransaction))
                    {
                        insertCommand.Parameters.AddWithValue("@id", id);
                        insertCommand.Parameters.AddWithValue("@orderNum", orderNum);
                        insertCommand.Parameters.AddWithValue("@boxCode", boxCode);
                        insertCommand.Parameters.AddWithValue("@date", date);
                        insertCommand.Parameters.AddWithValue("@mbr", jmbg);
                        insertCommand.Parameters.AddWithValue("@qrCode", jObject.ToString());

                        insertCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO RWTABLE VALUES (@boxCode, @rwCode, @id)", DatabaseManager.SqlConnection, sqlTransaction))
                    {
                        insertCommand.Parameters.AddWithValue("@boxCode", boxCode);
                        insertCommand.Parameters.AddWithValue("@rwCode", fileNum);
                        insertCommand.Parameters.AddWithValue("@id", id);

                        insertCommand.ExecuteNonQuery();
                    }

                    sqlTransaction.Commit();
                }
                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        log.AppendLine(String.Format("Red iz tabele: {0} sa ID: {1} nije upisan u bazu jer postoji fajl sa tim ID-jem.\n", rowIndex, row.GetCell(FOLL_ID_COL).StringValue));
                    }
                    else
                    {
                        log.AppendLine(String.Format("Red iz tabele: {0} sa ID: {1} nije upisan u bazu. Desila se greška. \n", rowIndex, row.GetCell(FOLL_ID_COL).StringValue));
                    }
                    sqlTransaction.Rollback();

                }
            }


            File.WriteAllText("log.txt", log.ToString());
            return true;
        }

        /// <summary>
        /// Generates report from current database snapshot. Uses only IDs that have fileNumbers assigned. 
        /// Also it can generate reports by box code OR order numbers.
        /// <param name="boxCode">Box code provided.</param>
        /// <param name="orderNum">Order number provided.</param>
        /// </summary>
        public static bool GenerateReport(string boxCode, string orderNum)
        {
            string commandText = "SELECT * FROM PartCodes WHERE FileNumber IS NOT NULL AND ";
            string outputPath = string.Empty;
            IList<Tuple<string, object>> parameters = new List<Tuple<string, object>>();

            if (boxCode == null && orderNum != null)
            {
                commandText += "OrderNum = @orderNum";
                outputPath = orderNum + ".xls";
                Tuple<string, object> parameter = Tuple.Create<string, object>("@orderNum", orderNum);
                parameters.Add(parameter);
            }
            else if (boxCode != null & orderNum == null)
            {
                commandText += "BoxCode = @boxCode";
                outputPath = boxCode + ".xls";
                Tuple<string, object> parameter = Tuple.Create<string, object>("@boxCode", boxCode);
                parameters.Add(parameter);
            }

            return ExecuteQuery(outputPath, commandText, parameters);
        }

        public static bool GenerateReport(DateTime start, DateTime stop)
        {
            string commandText = string.Empty;
            string outputPath = string.Empty;
            if (start.Date.Equals(stop.Date))
            {
                commandText = "SELECT * FROM PartCodes WHERE CAST([Date] as Date) LIKE CAST(@start as DATE)";
                outputPath = $"{start.Day}.{start.Month}.{start.Year}.xls";
            }
            else
            {
                commandText = "SELECT * FROM PartCodes WHERE CAST([Date] as Date) BETWEEN CAST(@start as DATE) AND CAST(@stop as DATE)";
                outputPath = $"{start.Day}.{start.Month}.{start.Year}.-{stop.Day}.{stop.Month}.{stop.Year}.xls";
            }
            IList<Tuple<string, object>> parameters = new List<Tuple<string, object>>()
            {
                Tuple.Create<string, object>("@start", start),
                Tuple.Create<string, object>("@stop", stop)
            };

            return ExecuteQuery(outputPath, commandText, parameters);
        }

        /// <summary>
        /// Extension method that checks if DataReader contains given column.
        /// </summary>
        /// <param name="r">Data reader to check.</param>
        /// <param name="columnName">Column name to be checked.</param>
        /// <returns>Indicator of success.</returns>
        public static bool HasColumn(this IDataRecord r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

        }

        #endregion

        #region Private methods

        private static bool ExecuteQuery(string outputPath, string commandText, IEnumerable<Tuple<string, object>> parameters)
        {
            Worksheet outputSheet = new Worksheet("Sheet1");
            Workbook outputBook = new Workbook();


            int curRow = 0;
            int curCol = 0;

            foreach (string columnHeader in reportHeaderNames)
            {
                outputSheet.Cells[curRow, curCol++] = new Cell(columnHeader);
            }

            curRow++;
            curCol = 0;

            try
            {
                using (SqlCommand command = new SqlCommand(commandText, DatabaseManager.SqlConnection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                foreach (string columnName in sqlDbColumnNames)
                                {
                                    outputSheet.Cells[curRow, curCol++] = GenerateStringValue(reader, columnName);
                                }

                                // Move to next row.
                                curRow++;
                                // Reset the column.
                                curCol = 0;
                            }
                        }
                    }

                    outputBook.Worksheets.Add(outputSheet);
                    outputBook.Save(outputPath);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Generates a <see cref="Cell"/> object depending on input data reader and column name.
        /// </summary>
        /// <param name="reader">Object from which the value is read.</param>
        /// <param name="columnName">Column to be read.</param>
        /// <returns><see cref="Cell"/> with empty value if column does not exist. Column value otherwise.</returns>
        private static Cell GenerateStringValue(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null  || !reader.HasColumn(columnName) || reader[columnName].GetType().Equals(typeof(DBNull)))
            {
                return new Cell(string.Empty);
            }

            // Match special case of order num.
            // With OrderNumber, date of scanning needs to be added.
            if (columnName.Equals("OrderNum"))
            {
                DateTime scanDateTime = (DateTime)reader["Date"];
                return new Cell((string)reader[columnName]  + $"/{scanDateTime.Day}.{scanDateTime.Month}.{scanDateTime.Year}.");
            }
            

            if (reader.GetFieldType(reader.GetOrdinal(columnName)) == typeof(string))
            {
                return new Cell((string)reader[columnName]);
            }
            // Takeover time is used here.
            else if (reader.GetFieldType(reader.GetOrdinal(columnName)) == typeof(DateTime))
            {
                DateTime takeoverDate = (DateTime)reader[columnName];
                return new Cell($"{takeoverDate.Day}.{takeoverDate.Month}.{takeoverDate.Year}.");
            }
            else
            {
                return new Cell(string.Empty);
            }
        }

        #endregion
    }
}
