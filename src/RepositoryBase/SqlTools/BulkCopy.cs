using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dlp.Buy4.RepositoryBase.SqlTools
{
    class BulkCopy : IDisposable
    {
        private SqlTransaction _transaction;
        private const int BULK_COPY_DEFAULT_TIMEOUT = 600;

        public BulkCopy(SqlTransaction transaction)
        {
            this._transaction = transaction;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inserts bulk data from the passed DataTable to database.
        /// </summary>
        /// <param name="dataTable">Data Table to be bulk inserted</param>
        public void BulkInsert(DataTable dataTable, SqlBulkCopyOptions copyOptions = SqlBulkCopyOptions.Default)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_transaction.Connection, copyOptions, _transaction))
            {
                bulkCopy.DestinationTableName = dataTable.TableName;

                bulkCopy.BulkCopyTimeout = BULK_COPY_DEFAULT_TIMEOUT;

                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
