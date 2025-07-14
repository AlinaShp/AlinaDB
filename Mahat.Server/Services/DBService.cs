using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;
using Mahat.Server.DTOs;
using Mahat.Server.Models;
using Mahat.Server.Repositories;
using Microsoft.Data.SqlClient;

namespace Mahat.Server.Services
{
    public interface IDBService
    {
        public List<DB> GetDBInfo(string instanceName, WindowsIdentity user);
        public List<TableDto> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user);
        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName, WindowsIdentity user);
        public string ExecuteQuery(string instanceName, string request, WindowsIdentity user);
        public DataTable RestoreDB(string instanceName, string databaseName, WindowsIdentity user);
        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user);
        public void AddDB(string instanceName, string databaseName, string collection, WindowsIdentity user);
        public void AddTable(string instanceName, string databaseName, Table newTable, WindowsIdentity user);
        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData, WindowsIdentity user);
    }

    public class DBService(IDBRepository dBRepository) : IDBService
    {

        public readonly IDBRepository _dbRepository = dBRepository;

        public List<DB> GetDBInfo(string instanceName, WindowsIdentity user)
        {

            List<DB> dbList = new List<DB>();

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                dbList = _dbRepository.GetDBInfo(instanceName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

            if (dbList.Count > 0)
            {

                return dbList;
            }

            else
            {

                throw new KeyNotFoundException($"No databases on instance '{instanceName}'.");
            }
        }

        public List<TableDto> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user)
        {
            List<TableDto> tablesList = new List<TableDto>();

#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                tablesList = _dbRepository.GetAllTablesData(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility

            if (tablesList.Count > 0)
            {

                return tablesList;
            }
            else
            {

                throw new KeyNotFoundException($"No tables found in database '{databaseName}' on instance '{instanceName}'.");
            }
        }

        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName, WindowsIdentity user)
        {
            List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                tableDataList = _dbRepository.GetTableData(instanceName, databaseName, tableName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility


            if (tableDataList.Count > 0)
            {

                return tableDataList;
            }
            else
            {

                throw new KeyNotFoundException($"No data found in table '{tableName}' in DB '{databaseName}' on instance '{instanceName}'.");
            }
        }

        public string ExecuteQuery(string instanceName, string request, WindowsIdentity user)
        {

            string result = string.Empty;

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                result = _dbRepository.ExecuteQuery(instanceName, request);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

            return result;
        }

        public DataTable RestoreDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            DataTable resultTable = new DataTable();

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                resultTable = _dbRepository.RestoreDB(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

            return resultTable;
        }

        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user)
        {

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.BackupDB(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void AddDB(string instanceName, string databaseName, string collection, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.AddDB(instanceName, databaseName, collection);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void AddTable(string instanceName, string databaseName, Table newTable, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.AddTable(instanceName, databaseName, newTable);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.InsertRow(instanceName, databaseName, tableName, rowData);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}