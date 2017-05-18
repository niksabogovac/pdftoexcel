using ExcelLibrary.SpreadSheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class ReportManager
    {
        private static readonly int FOLL_ID_COL = 1;
        private static readonly int CLIENT_ID_COL = 2;
        private static readonly int CLIENT_NAME_COL = 3;
        private static readonly int ACCOUNT_ID_COL = 4;
        private static readonly int CASE_TYPE_COL = 5;
        private static readonly int DOCUMENT_OBJ_ID_COL = 6;

        /// <summary>
        /// Generates report from current database snapshot. Uses only IDs that have fileNumbers assigned. 
        /// Also it can generate reports by box code OR order numbers.
        /// <param name="boxCode">Box code provided.</param>
        /// <param name="orderNum">Order number provided.</param>
        /// </summary>
        public static bool GenerateReport(string boxCode, string orderNum)
        {
            string outputPath = "izvestaj.xls";
            Worksheet outputSheet = new Worksheet("Sheet1");
            Workbook outputBook = new Workbook();


            int curRow = 0;
            int curCol = 0;

            outputSheet.Cells[curRow, curCol++] = new Cell("ID");
            outputSheet.Cells[curRow, curCol++] = new Cell("Broj naloga");
            outputSheet.Cells[curRow, curCol++] = new Cell("Kutija");
            outputSheet.Cells[curRow++, curCol] = new Cell("File number");

            curCol = 0;

            string commandText = string.Empty;

            if (boxCode == null && orderNum != null)
            {
                commandText = "SELECT * FROM PartCodes WHERE FileNumber IS NOT NULL AND OrderNum = @orderNum";
            }
            else if (boxCode != null & orderNum == null)
            {
                commandText = "SELECT * FROM PartCodes WHERE FileNumber IS NOT NULL AND BoxCode = @boxCode";
            }

            using (SqlCommand command = new SqlCommand(commandText, DatabaseManager.SqlConnection))
            {
                if (boxCode == null && orderNum != null)
                {
                    command.Parameters.AddWithValue("@orderNum", orderNum);
                }
                else if (boxCode != null & orderNum == null)
                {
                    command.Parameters.AddWithValue("@boxCode", boxCode);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["ID"]);
                            outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["OrderNum"]);
                            outputSheet.Cells[curRow, curCol++] = new Cell((string)reader["BoxCode"]);
                            outputSheet.Cells[curRow++, curCol] = new Cell((string)reader["FileNumber"]);
                            curCol = 0;
                        }
                    }
                    try
                    {
                        outputBook.Worksheets.Add(outputSheet);
                        outputBook.Save(outputPath);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

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
            catch(Exception)
            {
                return false;
            }
            

            // Skip headers.
            for(int rowIndex = sheet.Cells.FirstRowIndex + 1; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
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
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO BankTable VALUES (@id,@orderNum, @boxCode, @date,@mbr,@qrCode)", DatabaseManager.SqlConnection,sqlTransaction))
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
                catch(SqlException e)
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

    }
}
