using Mahat.Server.Models;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Security.Principal;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Core;
using Mahat.Server.DTOs;
using System;
using Mahat.Server.Enums;

namespace Mahat.Server.Repositories
{
    public interface IDBRepository
    {
        public void CheckConnection(string instanceName);
        public List<DB> GetDBInfo(string instanceName);
        public string ExecuteQuery(string instanceName, string request);
        public void RestoreDB(string databaseName, string instanceName, List<BackupInfo> backupInfo);
        public void BackupDB(string instanceName, string databaseName, BackupInfo backupInfo);
        public void AddDB(string instanceName, string databaseName, string collation );
        public void DeleteDB(string instanceName, string databaseName);
        public bool IsDBExists(string instanceName, string databaseName);
        public List<string> GetExistingBackups(string instanceName, string databaseName, BackupTypes backupType);
        public bool IsBackupExist(string instanceName, string databaseName, string filePath);
    }

    public class DBRepository : IDBRepository
    {

        public DBRepository()
        {

        }

        public void CheckConnection(string instanceName)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException($"Failed to connect to instance '{instanceName}'.", ex);
                }
            }
        }

        public List<DB> GetDBInfo(string instanceName)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            List<DB> dbList = new List<DB>();

            string dbInfoQuery = @"
                SELECT name, database_id, state_desc, recovery_model_desc, compatibility_level, collation_name
                FROM sys.databases
                WHERE name NOT IN ('master', 'model', 'tempdb', 'msdb');";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(dbInfoQuery, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DB database = new DB
                                {
                                    DatabaseName = reader["name"] as string ?? "",
                                    DatabaseId = reader["database_id"] as int? ?? 0,
                                    State = reader["state_desc"] as string ?? "",
                                    RecoveryModel = reader["recovery_model_desc"] as string ?? "",
                                    CompatibilityLevel = reader["compatibility_level"] as int? ?? 0,
                                    Collation = reader["collation_name"] as string ?? "",
                                };

                                dbList.Add(database);
                            }
                        }
                    }                       
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to get DBs on instance '{instanceName}'.", ex);
            }

            return dbList;
        }

        public string ExecuteQuery(string instanceName, string request)
        {
            string connectionString = "Server=" + instanceName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            DataTable resultTable = new DataTable();
            int rowsAffected = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(request, con))
                    {
                        con.Open();

                        if (request.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                        {
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {

                                adapter.Fill(resultTable);
                            }
                        }
                        else
                        {

                            rowsAffected = command.ExecuteNonQuery();
                        }

                        var result = new
                        {
                            RowsAffected = rowsAffected,
                            Result = resultTable
                        };

                        return JsonConvert.SerializeObject(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"An error occurred while executing the query on instance '{instanceName}'.", ex);
            }
        }

        public void RestoreDB(string instanceName, string databaseName,  List<BackupInfo> backupInfo)
        {
            string connectionString = $"Server={instanceName};Database=master;Integrated Security=SSPI;TrustServerCertificate=True;";
            string restoreQuery = $@"
                ALTER DATABASE [{databaseName}]
                SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

            int finalIndex = backupInfo.Count - 1;
            int index = 0;

            backupInfo.ForEach((info) => {
                if (info.BackupType == BackupTypes.FULL)
                {
                    restoreQuery += $@"
                        RESTORE DATABASE [{databaseName}] 
                        FROM DISK = '{info.FilePath}\\{info.FileName}.bak'";
                }
                else if (info.BackupType == BackupTypes.DIFFERENTIAL)
                {
                    restoreQuery += $@"
                        RESTORE DATABASE [{databaseName}] 
                        FROM DISK = '{info.FilePath}\\{info.FileName}.bak'";
                }
                else
                {
                    restoreQuery += $@"
                        RESTORE LOG [{databaseName}] 
                        FROM DISK = '{info.FilePath}\\{info.FileName}.trn';";
                }
                if (index == finalIndex)
                {
                    restoreQuery += " WITH RECOVERY;";
                }
                else
                {
                    restoreQuery += " WITH NORECOVERY;";
                }
                index++;
                });
                
            restoreQuery += $@"
                ALTER DATABASE [{databaseName}]
                SET MULTI_USER;";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(restoreQuery, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to restore DB '{databaseName}' on instance '{instanceName}'.", ex);
            }
        }

        public void BackupDB(string instanceName, string databaseName, BackupInfo backupInfo)
        {

            string connectionString = "Server=" + instanceName + ";Database=" + databaseName + ";Integrated Security=SSPI;TrustServerCertificate=True;";
            string backupQuery = "";

            if (backupInfo.BackupType == BackupTypes.FULL){
                backupQuery = $@"
                    BACKUP DATABASE {databaseName} 
                    TO DISK = '{backupInfo.FilePath}\\{backupInfo.FileName}.bak' 
                    WITH INIT;";
            }

            else if (backupInfo.BackupType == BackupTypes.DIFFERENTIAL){
                backupQuery = $@"
                    BACKUP DATABASE {databaseName} 
                    TO DISK = '{backupInfo.FilePath}\\{backupInfo.FileName}.bak' 
                    WITH DIFFERENTIAL;";
            }

            else
            {
                backupQuery = $@"
                    BACKUP LOG {databaseName} 
                    TO DISK = '{backupInfo.FilePath}\\{backupInfo.FileName}.trn';";
            }
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(backupQuery, con))
                    {
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        catch (SqlException ex)
                        {
                            throw new InvalidOperationException($"Failed to back up DB '{databaseName}' on instance '{instanceName}'.", ex);
                        }
                    }
                }
        }

        public void AddDB(string instanceName, string databaseName, string collation)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string query = $@"CREATE DATABASE {databaseName} COLLATE {collation};";

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

        public void DeleteDB(string instanceName, string databaseName)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string query = $@"DROP DATABASE {databaseName};";

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
                        throw new InvalidOperationException($"Failed to delete database '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }

        public bool IsDBExists(string instanceName, string databaseName)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string query = $@"SELECT database_id FROM sys.databases WHERE name = '{databaseName}';";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        var result = command.ExecuteScalar();

                        return result != null;
                    }
                    catch (SqlException ex)
                    {
                        throw new InvalidOperationException($"Failed to check existence of database '{databaseName}' on instance '{instanceName}'.", ex);
                    }
                }
            }
        }

        public List<string> GetExistingBackups(string instanceName, string databaseName, BackupTypes backupType)
        {
            string connectionString = $"Server={instanceName};Integrated Security=SSPI;TrustServerCertificate=True;";
            string type = backupType == BackupTypes.FULL ? "D" : backupType == BackupTypes.DIFFERENTIAL ? "I" : "L";

            string query = $@"
                    SELECT DISTINCT bmf.physical_device_name AS backup_file
                    FROM msdb.dbo.backupmediafamily bmf
                    JOIN msdb.dbo.backupset bs ON bs.media_set_id = bmf.media_set_id
                    WHERE bs.database_name = '{databaseName}' AND bs.type = '{type}'
                    ORDER BY backup_file;";

            List<string> backupFiles = new List<string>();
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
                                string backupFile = reader["backup_file"] as string ?? "";
                                backupFile = backupFile.Replace("\\\\", "\\");
                                backupFiles.Add(backupFile);
                            }

                            return backupFiles;
                        }
                    }
                }
            }
      
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to get existing backups of DB '{databaseName}' with type '{backupType}' on instance '{instanceName}'.", ex);
            }
        }

        public bool IsBackupExist(string instanceName, string databaseName, string filePath)
        {
            string connectionString = $"Server={instanceName};Database={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            
            string query = $@"exec xp_fileexist '{filePath}';";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        var result = cmd.ExecuteScalar();

                        return Convert.ToBoolean(result);
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new InvalidOperationException($"Failed to check if backup {filePath}  of db {databaseName} om instance {instanceName} exist", ex);
            }
        }
    }
}
