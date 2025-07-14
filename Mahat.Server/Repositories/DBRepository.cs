using Mahat.Server.Models;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Security.Principal;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Core;
using Mahat.Server.DTOs;

namespace Mahat.Server.Repositories
{
    public interface IDBRepository
    {
        public List<DB> GetDBInfo(string instanceName);
        public List<TableDto> GetAllTablesData(string instanceName, string databaseName);
        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName);
        public string ExecuteQuery(string instanceName, string request);
        public DataTable RestoreDB(string instanceName, string databaseName);
        public void BackupDB(string instanceName, string databaseName);
        public void AddDB(string instanceName, string databaseName, string collection);
        public void AddTable(string instanceName, string databaseName, Table newTable);
        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData);
    }

    public class DBRepository : IDBRepository
    {

        public DBRepository()
        {

        }

        public List<DB> GetDBInfo(string instanceName)
        {
            string connectionString = "Server=" + instanceName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT name,database_id, state_desc, recovery_model_desc, compatibility_level, " +
                "collation_name FROM sys.databases WHERE name NOT IN ('master', 'model', 'tempdb', 'msdb');\r\n ", con);
            List<DB> dbList = new List<DB>();

            try
            {
                con.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DB database = new DB();
#pragma warning disable CS8601 // Possible null reference assignment.
                        database.DatabaseName = Convert.ToString(dt.Rows[i]["name"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                        database.DatabaseId = Convert.ToInt32(dt.Rows[i]["database_id"]);
#pragma warning disable CS8601 // Possible null reference assignment.
                        database.State = Convert.ToString(dt.Rows[i]["state_desc"]);
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
                        database.RecoveryModel = Convert.ToString(dt.Rows[i]["recovery_model_desc"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                        database.CompatibilityLevel = Convert.ToInt32(dt.Rows[i]["compatibility_level"]);
#pragma warning disable CS8601 // Possible null reference assignment.
                        database.Collation = Convert.ToString(dt.Rows[i]["collation_name"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                        SqlDataAdapter daTableNames = new SqlDataAdapter("USE " + database.DatabaseName + ";  ", con);
                        DataTable tablesNameDT = new DataTable();
                        daTableNames.Fill(tablesNameDT);
                        for (int j = 0; j < tablesNameDT.Rows.Count; j++)
                        {
                            System.Diagnostics.Debug.WriteLine("USE " + database.DatabaseName + "; SELECT name FROM sys.tables;");

#pragma warning disable CS8604 // Possible null reference argument.
                            database.Tables.Add(Convert.ToString(tablesNameDT.Rows[j]["name"]));
#pragma warning restore CS8604 // Possible null reference argument.
                        }

                        dbList.Add(database);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to get DBs on instance '{instanceName}'.", ex);
            }

            return dbList;
        }

        public List<TableDto> GetAllTablesData(string instanceName, string databaseName)
        {
            string connectionString = "Server=" + instanceName + ";Database=" + databaseName + "; Integrated Security=SSPI;TrustServerCertificate=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("USE " + databaseName + "; SELECT tc.TABLE_NAME AS TableName, kcu.COLUMN_NAME AS PrimaryKey FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS" +
                " tc INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS kcu ON tc.CONSTRAINT_NAME" +
                " = kcu.CONSTRAINT_NAME WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY tc.TABLE_NAME;", con);

            DataTable dt = new DataTable();
            List<TableDto> tablesList = new List<TableDto>();

            try
            {
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableDto table = new TableDto();
#pragma warning disable CS8601 // Possible null reference assignment.
                        table.TableName = Convert.ToString(dt.Rows[i]["TableName"]);
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
                        table.PrimaryKey = Convert.ToString(dt.Rows[i]["PrimaryKey"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                        tablesList.Add(table);
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
            string connectionString = "Server=" + instanceName + ";Database=" + databaseName + "; Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("USE " + databaseName + "; SELECT * FROM " + tableName + ";", con);
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();

            try
            {

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> rowData = new Dictionary<string, object>();
                        foreach (DataColumn column in dt.Columns)
                        {

                            rowData[column.ColumnName] = row[column];
                        }

                        tableDataList.Add(rowData);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to get data of table '{tableName}' in DB '{databaseName}' on instance '{instanceName}'.", ex);
            }

            return tableDataList;
        }

        public string ExecuteQuery(string instanceName, string request)
        {
            string connectionString = "Server=" + instanceName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(request, con);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable resultTable = new DataTable();
            int rowsAffected = 0;

            try
            {
                con.Open();

                // Use ExecuteNonQuery to get the number of rows affected for non-select queries
                if (request.TrimStart().IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0)
                {

                    adapter.Fill(resultTable);
                }
                else
                {
                    // For non-SELECT queries, use ExecuteNonQuery to get the number of rows affected
                    rowsAffected = command.ExecuteNonQuery();
                }

                var result = new
                {
                    RowsAffected = rowsAffected,
                    Result = resultTable
                };

                con.Close();

                return JsonConvert.SerializeObject(result);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"An error occurred while executing the query on instance '{instanceName}'.", ex);
            }
        }

        public DataTable RestoreDB(string instanceName, string databaseName)
        {
            string connectionString = "Server=" + instanceName + ";Database=" + databaseName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("USE master; ALTER DATABASE" + databaseName + "SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE bestplaceever FROM DISK = 'C:\\Backups\\" + databaseName + "\\File.bak' WITH REPLACE; ALTER DATABASE bestplaceever SET MULTI_USER;", con);
            DataTable resultTable = new DataTable();

            try
            {
                con.Open();
                adapter.Fill(resultTable);
                con.Close();

                return resultTable;
            }

            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to restore DB '{databaseName}' on instance '{instanceName}'.", ex);
            }
        }

        public void BackupDB(string instanceName, string databaseName)
        {

            string connectionString = "Server=" + instanceName + ";Database=" + databaseName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            string backupQuery = $"BACKUP DATABASE {databaseName} TO DISK = 'C:\\Backups\\{databaseName}\\{databaseName}.bak' WITH FORMAT, INIT, SKIP;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(backupQuery, con))
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    catch (SqlException ex)
                    {
                        throw new InvalidOperationException($"Failed to back up DB '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }

        public void AddDB(string instanceName, string databaseName, string collection)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string query = $"CREATE DATABASE {databaseName} COLLATE {collection};";

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
                        throw new InvalidOperationException($"Failed to create database '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }

        public void AddTable(string instanceName, string databaseName, Table newTable)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
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
            string query = $"USE {databaseName}; CREATE TABLE [{newTable.TableName}] ({colsDefinition});";

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
            string query = $"USE {databaseName}; INSERT INTO [{tableName}] ([{columns}]) VALUES ({values});";
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
