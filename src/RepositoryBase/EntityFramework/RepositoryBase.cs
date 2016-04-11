using Dlp.Buy4.RepositoryBase.SqlTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dlp.Buy4.RepositoryBase.EntityFramework
{
    public class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        internal DbContext _dbContext;
        internal DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public IList<T> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T GetById(params object[] keyValues)
        {
            return GetByIdAsync(keyValues).Result;
        }

        public async Task<T> GetByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public T Add(T entity)
        {
            return AddAsync(entity).Result;
        }

        public async Task<T> AddAsync(T entity)
        {
            T entityAdded = _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entityAdded;
        }

        public void Remove(T entity)
        {
            RemoveAsync(entity).Wait();
        }

        public async Task RemoveAsync(T entity)
        {
            T entityAdded = _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            UpdateAsync(entity).RunSynchronously();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public void BulkInsert(ICollection<T> entityCollection)
        {
            // Inicia o escopo de uma transação
            using (SqlTransaction transaction = ((SqlConnection)_dbContext.Database.Connection).BeginTransaction())
            {
                using (var bulkCopy = new BulkCopy(transaction))
                {
                    using (DataTable dataTable = GetDataTable(entityCollection))
                    {
                        // Performa o bulk insert
                        bulkCopy.BulkInsert(dataTable, SqlBulkCopyOptions.CheckConstraints);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a DataTable from a entity collection.
        /// </summary>
        private DataTable GetDataTable(ICollection<T> entityCollection)
        {
            string tableName = GetTableName<T>();
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            ICollection<object[]> rows = new List<object[]>();

            ICollection<PropertyInfo> properties = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                columns.Add(GetColumnName(propertyInfo), GetColumnType(propertyInfo));
            }

            foreach (T entity in entityCollection)
            {
                rows.Add(properties.Select(property => property.GetValue(entity, null)).ToArray());
            }

            return DataTableHelper.Create(tableName, columns, rows);
        }

        /// <summary>
        /// Obtém o tipo da coluna para a propriedade da entidade.
        /// </summary>
        /// <param name="propertyInfo">Propriedade da entidade.</param>
        /// <returns>Tipo da coluna</returns>
        private Type GetColumnType(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            return propertyType;
        }

        /// <summary>
        /// Obtém o nome da coluna para a propriedade da entidade.
        /// </summary>
        /// <param name="propertyInfo">Propriedade da entidade.</param>
        /// <returns>Nome da coluna</returns>
        private string GetColumnName(PropertyInfo propertyInfo)
        {
            ColumnAttribute columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();

            if (columnAttribute != null)
            {
                return columnAttribute.Name;
            }

            return propertyInfo.Name;
        }

        /// <summary>
        /// Obtém o nome da tabela definida para a entidade.
        /// </summary>
        private string GetTableName<T>()
        {
            TableAttribute tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();

            if (tableAttribute != null)
            {
                if (string.IsNullOrWhiteSpace(tableAttribute.Schema) == false)
                {
                    return "[" + tableAttribute.Schema + "]." + tableAttribute.Name;
                }
                return tableAttribute.Name;
            }

            return typeof(T).Name;
        }
    }
}
