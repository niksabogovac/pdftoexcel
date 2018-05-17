using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QR_Code.Import
{
    public class DbUpdater
    {
        private readonly SqlConnection connection;
        private SqlTransaction transaction;

        public DbUpdater(SqlConnection connection)
        {
            this.connection = connection;
        }

        public int ExecuteUpdate(string commandText, IEnumerable<Tuple<string, object>> parameters)
        {
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Transaction = transaction;
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                }
                 return command.ExecuteNonQuery();
            }
        }

        public void StartTransacation()
        {
            transaction = connection.BeginTransaction();
        }

        public void CommitChanges() => transaction.Commit();

        public void RollBackChanges() => transaction.Rollback();
    }
}
