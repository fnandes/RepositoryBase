using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dlp.Buy4.RepositoryBase.SqlTools
{
    /// <summary> Help methods formanipulate datatables </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// Receives the table name and the table columns and creates a empty DataTable.
        /// </summary>
        /// <param name="tableName">string table name</param>
        /// <param name="columns">string array with table columns</param>
        public static DataTable Create(string tableName, Dictionary<string, Type> columns)
        {
            DataTable dataTable = new DataTable(tableName);

            foreach (KeyValuePair<string, Type> column in columns)
            {
                dataTable.Columns.Add(column.Key, column.Value);
            }

            return dataTable;
        }

        /// <summary>
        /// Receives the table name, the table columns and the table rows and creates a filled DataTable.
        /// </summary>
        /// <param name="tableName">string table name</param>
        /// <param name="columns">string array with table columns</param>
        /// <param name="rows">object array collection with table rows</param>
        public static DataTable Create(string tableName, Dictionary<string, Type> columns, ICollection<object[]> rows)
        {
            DataTable dataTable = Create(tableName, columns);
            foreach (object[] row in rows)
            {
                AddRow(ref dataTable, row);
            }

            return dataTable;
        }

        /// <summary>
        /// Receives a DataTable by reference and a row and adds it to the DataTable.
        /// </summary>
        /// <param name="dataTable">Data Table</param>
        /// <param name="row">object array with row to be added</param>
        public static void AddRow(ref DataTable dataTable, object[] row)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow.ItemArray = row;
            dataTable.Rows.Add(dataRow);
        }
    }
}
