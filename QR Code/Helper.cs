using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QR_Code
{
    /// <summary>
    /// Represents support structures.
    /// </summary>
    public class Helper
    {
        #region Constants

        private const string CONFIGURATION_NAME = "RaifeisenConfiguration.xml";
        #endregion

        #region Private members
        /// <summary>
        /// Represents a collection of names of client infos and their historical names.
        /// Not valid so not finished.
        /// </summary>
        private static Dictionary<string, string> clientInfoNamesHistoricalNames = new Dictionary<string, string>()
        {
            { "Potvrde_DinarskiTekuciRacun" ,"PotTR"},
            { "OstalaDokumenta_DinarskiTekuciRacun","OstDevAR"},
            { "OsnovnaDokumenta_DinarskiAvistaRacun","ODDinAR" },
            { "Krediti - ugovaranje","ODKR"},
            { "OsnovnaDokumenta_DevizniOroceniRacun", "ODDevOR" },
            { "OsnovnaDokumenta_DinarskiOroceniRacun" ,"ODDinOR"},
            { "OsnovnaDokumenta_DinarskiTekuciRacun" ,"ODTR" },
            { "OsnovniPodaciODepozitu_DevizniOroceniRacun" , "OPDDevOR"},
            { "OsnovniPodaciODepozitu_DinarskiOroceniRacun" ,"OPDDinOR"},
            { "Pozajmice_DinarskiTekuciRacun" , "PozTR"  },
            { "Kreditne kartice - ugovaranje" , "UruKK"},

        };

        /// <summary>
        /// Represents a dictionary of doctypes and appropriate box types.
        /// </summary>
        private static Dictionary<string, BoxTypeEnum> doctypeBoxCode = new Dictionary<string, BoxTypeEnum>();

        private static Lazy<string> connectionString = new Lazy<string>(() => InitializeXmlConfig());
        #endregion

        #region Properties

        /// <summary>
        /// Gets the support historical doctype dictionary.
        /// </summary>
        public static Dictionary<string, string> ClientInfoNamesHistoricalNames
        {
            get
            {
                return clientInfoNamesHistoricalNames;
            }
        }

        /// <summary>
        /// Gets the support <see cref="doctypeBoxCode"/> dictionary.
        /// </summary>
        public static Dictionary<string, BoxTypeEnum> DoctypeBoxCode
        {
            get
            {
                return doctypeBoxCode;
            }
        }

        //public static string ConnectionString = @"Data Source=MILAN;Initial Catalog=QRCode;Integrated Security=True";
        //public static string ConnectionString = @"Data Source=KNJG\SQLEXPRESS;Initial Catalog=QRCode;Integrated Security=True";

        public static string ConnectionString
        {
            get
            {
                return connectionString.Value;
            }
        }

        //private static string ConnectionString = @"Data Source=89.216.58.242\SQLEXPRESS;Initial Catalog=QRCode;Integrated Security=true";
        #endregion

        /// <summary>
        /// Singleton instance of db connection.
        /// </summary>
        public static SqlConnection connection;

        #region Public static methods

        /// <summary>
        /// Gets db connection.
        /// </summary>
        /// <returns>Singleton instance of db connection.</returns>
        public static SqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
            }
            return connection;

        }

        /// <summary>
        /// Checks if current token is ID or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's id.</returns>
        public static bool CheckId(string token)
        {
            if (token.Equals("id") || token.Equals("2"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is doctype or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's doctype.</returns>
        public static bool CheckDocType(string token)
        {
            if (token.Equals("doctype") || token.Equals("3"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is mbr or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's mbr.</returns>
        public static bool CheckMbr(string token)
        {
            if (token.Equals("mbr") || token.Equals("4"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is partija or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's partija.</returns>
        public static bool CheckPartija(string token)
        {
            if (token.Equals("partija") || token.Equals("5"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is zahtev or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's zahtev.</returns>
        public static bool CheckZahtev(string token)
        {
            if (token.Equals("zahtev") || token.Equals("6"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is id_kartice or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's id_kartice.</returns>
        public static bool CheckIdKartice(string token)
        {
            if (token.Equals("id_kartice") || token.Equals("7"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is mbrid or it's numeric pair.
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's mbrid.</returns>
        public static bool CheckMbrId(string token)
        {
            if (token.Equals("mbrid") || token.Equals("9"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if current token is paket or it's numeric pair (when they add one).
        /// </summary>
        /// <param name="token">Tokens to be checked.</param>
        /// <returns>Indicator if it's paket.</returns>
        public static bool CheckPaket(string token)
        {
            if (token.Equals("paket"))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Helper"/> class.
        /// </summary>
        public Helper()
        {

        }
        #endregion

        #region Public methods

        /// <summary>
        /// Updates value of doctypes from database when new doctype is added.
        /// </summary>
        public static void UpdateDocTypesFromDatabase()
        {
            SqlConnection conn = Helper.GetConnection();
            //conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[DocTypes]", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (doctypeBoxCode.ContainsKey((string)reader[0]))
                        {

                        }
                        else
                        {
                            string value = (string)reader[1];
                            value.ToUpper();
                            if (value.Equals("RACUNI"))
                            {
                                doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.RACUNI);
                            }
                            else if (value.Equals("POZAJMICE"))
                            {
                                doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.POZAJMICE);
                            }
                            else if (value.Equals("KREDITI"))
                            {

                                doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.KREDITI);
                            }
                            else if (value.Equals("OROCENJA"))
                            {
                                doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.OROCENJA);
                            }
                        }
                    }
                }
            }
            
        }




        #endregion

        #region Private methods

        private static string InitializeXmlConfig()
        {
            XElement configXml = XElement.Load(CONFIGURATION_NAME);
            string connString = string.Empty;
            foreach(var item in configXml.Elements())
            {
                if (item.FirstAttribute.Value.Equals(@"Connection String"))
                {
                    connString = item.Value;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
