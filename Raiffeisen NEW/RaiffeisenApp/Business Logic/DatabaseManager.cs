using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogic.Enums;

namespace BusinessLogic
{
    public static class DatabaseManager
    {
        #region Private members

        // Single instance of database connection.
        private static SqlConnection _sqlConnection = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets instance of sql connection to database.
        /// </summary>
        public static SqlConnection SqlConnection
        {
            get
            {
                if (_sqlConnection == null)
                {
                    _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["QrCode"].ConnectionString);
                    _sqlConnection.Open();
                }
                return _sqlConnection;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates values of nubmer of files in box when new values are scaned.
        /// </summary>
        /// <param name="boxCode">Code of box to be updated.</param>
        private static void UpdateBoxTable(string boxCode)
        {
            using (SqlCommand command = new SqlCommand("UPDATE [QRCode].[dbo].[Box] SET [NumberOfFiles] = (SELECT [NumberOfFiles] FROM [QRCode].[dbo].[Box] WHERE [Code] = @boxCode) + 1  WHERE [Code] = @boxCode", SqlConnection))
            {
                command.Parameters.AddWithValue("@boxCode", boxCode);
                command.ExecuteNonQuery();
            }

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds new partially scaned code to database.
        /// </summary>
        /// <param name="id">Id of new code, only simple format needed, for example RQ00024.</param>
        /// <param name="orderNum">Order number of code.</param>
        /// <param name="boxCode">Code of box where code is stored.</param>
        /// <param name="exceptionCode">In case of excepetion its code is set here.</param>
        /// <returns>Indicator of success.</returns>
        public static bool InsertNewPartialCode(string id, string orderNum, string boxCode,DateTime date, out int exceptionCode)
        {
            exceptionCode = -1;
            try
            {
                // Add new entry for partial code.
                using (SqlCommand command = new SqlCommand("INSERT INTO [QrCode].[dbo].[PartCodes] ([ID],[OrderNum],[BoxCode],[Date]) VALUES (@id,@orderNum,@boxCode,@date)", SqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@orderNum", orderNum);
                    command.Parameters.AddWithValue("@boxCode", boxCode);
                    command.Parameters.AddWithValue("@date", date);
                    command.ExecuteNonQuery();
                }
                // Update box table.
                UpdateBoxTable(boxCode);

                return true;
            }
            catch (SqlException e)
            {
                exceptionCode = e.Number;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds new box with "Tip5" into database.
        /// </summary>
        /// <param name="boxCode">Code of new box.</param>
        /// <returns>Indicator of success.</returns>
        public static bool InsertNewBox(string boxCode)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO [QrCode].[dbo].[Box] ([Code],[Type],[NumberOfFiles]) VALUES (@code,@type,@numFiles)", SqlConnection))
                {
                    command.Parameters.AddWithValue("@code", boxCode);
                    command.Parameters.AddWithValue("@type", (int)BoxTypeEnum.TIP5);
                    command.Parameters.AddWithValue("@numFiles", 0);
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if box with given code already exists in database AND is type 5 box.
        /// </summary>
        /// <param name="boxCode">Code to check.</param>
        /// <param name="boxType">Type of box if box exists.</param>
        /// <returns>Indicator of success.</returns>
        public static bool CheckIfType5BoxExists(string boxCode, out int boxType)
        {
            boxType = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [QrCode].[dbo].[Box] WHERE [Code] = @boxCode", SqlConnection))
                {
                    command.Parameters.AddWithValue("@boxCode", boxCode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if there are any boxes and if so, check box type.
                        if (!reader.HasRows)
                        {
                            return false;
                        }
                        reader.Read();
                        boxType = (int)reader["Type"];
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks number of files in box.
        /// </summary>
        /// <param name="boxCode">Code of box.</param>
        /// <returns>Number of files.</returns>
        public static int GetNumberOfFileFromBox(string boxCode)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT NumberOfFiles FROM [QRCode].[dbo].[Box] WHERE Code = @code", SqlConnection))
                {
                    command.Parameters.AddWithValue("@code", boxCode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return -1;
                        }
                        reader.Read();
                        return (int)reader["NumberOfFiles"];
                    }
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }

        public static List<string> GetCodeWithoutFileNumberByBox(string boxCode)
        {
            List<string> ret = new List<string>(500);

            try
            {
                using (SqlCommand command = new SqlCommand("SELECT ID FROM PartCodes WHERE FileNumber IS NULL AND BoxCode = @boxCode ", SqlConnection))
                {
                    command.Parameters.AddWithValue("@boxCode", boxCode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                ret.Add((string)reader["ID"]);
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                ret = null;
            }

            return ret;
        }

        /// <summary>
        /// Checks if fileNumber code is previously added.
        /// </summary>
        /// <param name="fileNum">Code to be added.</param>
        /// <returns>Indicator if code exists. TRUE - exists or error, FALSE - doesn't</returns>
        public static bool CheckPreviousFileNumberCodes(string fileNum)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT ID FROM PartCodes WHERE FileNumber = @fileNum", SqlConnection))
                {
                    command.CommandTimeout = 3600;
                    command.Parameters.AddWithValue("fileNum", fileNum);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Code already used
                        if (reader.HasRows)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds fileNumber to corresponding ids. Number of fileNumbers and ids MUST MATCH.
        /// </summary>
        /// <param name="fileNums">List of fileNumbers.</param>
        /// <param name="ids">List of ids.</param>
        /// <returns></returns>
        public static bool AddFileNumbersToPartialCode(List<string> fileNums, List<string> ids)
        {
            bool ret = false;
            SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE PartCodes SET FileNumber = @fileNum WHERE ID = @id", SqlConnection,sqlTransaction))
                {
                    int k = 0;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        command.Parameters.AddWithValue("@id", ids[i]);
                        if (i != 0 && i % 20 == 0)
                        {
                            k++;
                        }

                        command.Parameters.AddWithValue("@fileNum", fileNums[k]);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    sqlTransaction.Commit();
                    ret = true;
                }
            }
            catch(Exception)
            {
                sqlTransaction.Rollback();
                ret = false;
            }

            return ret;
        }
 
        #endregion
    }
}
