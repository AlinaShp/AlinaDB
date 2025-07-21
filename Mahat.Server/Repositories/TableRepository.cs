using Microsoft.Data.SqlClient;
using System.Data;
using Mahat.Server.DTOs;
using Mahat.Server.Models;

namespace Mahat.Server.Repositories
{
    public interface ITableRepository
    {
        public List<TableDto> GetAllTablesData(string instanceName, string databaseName);
        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName);
        public void AddTable(string instanceName, string databaseName, Table newTable);
        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData);
    }

    public class TableRepository : ITableRepository
    {

        public TableRepository()
        {

        }

        public List<TableDto> GetAllTablesData(string instanceName, string databaseName)
        {
            string connectionString = $"Server={instanceName};Database={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            List<TableDto> tablesList = new List<TableDto>();
            string query = @"
                SELECT tc.TABLE_NAME AS TableName, kcu.COLUMN_NAME AS PrimaryKey
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS kcu
                    ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
                WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
                ORDER BY tc.TABLE_NAME;";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TableDto table = new TableDto
                                {
                                    TableName = reader["TableName"] as string ?? "",
                                    PrimaryKey = reader["PrimaryKey"] as string ?? ""
                                };
                                tablesList.Add(table);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new InvalidOperationException($"Failed to get tables in DB '{databaseName}' on instance '{instanceName}'.", ex);
            }

            return tablesList;
        }


        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName)
        {
            string connectionString = $"Server={instanceName};Database={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();
            string query = $"SELECT * FROM [{tableName}]";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> rowData = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowData[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                                tableDataList.Add(rowData);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to get data of table '{tableName}' in DB '{databaseName}' on instance '{instanceName}'.", ex);
            }

            return tableDataList;
        }
        
        public void AddTable(string instanceName, string databaseName, Table newTable)
        {
            string connectionString = $"Server={instanceName};Database={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string colsDefinition = "";
            for (int i = 0; i < newTable.Cols.Length; i++)
            {
                TableCol col = newTable.Cols[i];
                string colDefinition = $"{col.ColName} {col.DataType} ";

                if (!col.IsNullable)
                {
                    colDefinition += "NOT NULL ";
                }
                if (col.IsIdentity)
                {
                    colDefinition += "IDENTITY(1,1) ";
                }
                if (col.IsPrimaryKey)
                {
                    colDefinition += "PRIMARY KEY ";
                }
                if (col.IsUnique)
                {
                    colDefinition += "UNIQUE ";
                }
                if (col.IsForeignKey)
                {
                    ForeignKeyTableCol foreignKeyTableCol = col as ForeignKeyTableCol;

                    colDefinition += $"FOREIGN KEY REFERENCES {foreignKeyTableCol.ForeignKeyTableName}({foreignKeyTableCol.ForeignKeyColName}) ";
                }
                colsDefinition += colDefinition;

                if (i != newTable.Cols.Length - 1)
                {
                    colsDefinition += ", ";
                }
            }
            string query = $"CREATE TABLE [{newTable.TableName}] ({colsDefinition});";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new InvalidOperationException($"Failed to create table  {newTable.TableName} in database '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }

        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData)
        {
            string connectionString = $"Server={instanceName};Database={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string columns = string.Join("], [", rowData.Keys);
            string values = string.Join(", ", rowData.Values.Select(v => $"'{v}'"));
            string query = $@"
                INSERT INTO [{tableName}] ([{columns}]) 
                VALUES ({values});";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new InvalidOperationException($"Failed to insert row into table '{tableName}' in database '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }
    }
}