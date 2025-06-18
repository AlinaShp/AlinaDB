using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;
using Mahat.Server.Models;
using Mahat.Server.Repositories;
using Microsoft.Data.SqlClient;

namespace Mahat.Server.Services
{
    public interface IDBService
    {
        public List<DB> GetDBInfo(string instanceName, WindowsIdentity user);
        public List<Table> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user);
        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName, WindowsIdentity user);
        public string ExecuteQuery(string instanceName, string request, WindowsIdentity user);
        public DataTable RestoreDB(string instanceName, string databaseName, WindowsIdentity user);
        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user);
        public DataTable ChangeRecoveryModel(string instanceName, string databaseName, string recoveryModel, WindowsIdentity user);
    }

    public class DBService(IDBRepository dBRepository) : IDBService
    {

        public readonly IDBRepository _dbRepository = dBRepository;

        public List<DB> GetDBInfo(string instanceName, WindowsIdentity user)
        {

            List<DB> dbList = new List<DB>();

            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    dbList = _dbRepository.GetDBInfo(instanceName);
                });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
            }

            catch (SqlException ex)
            {

                throw ex;
            }

            if (dbList.Count > 0)
            {

                return dbList;
            }

            else
            {

                throw new KeyNotFoundException("No databases found on the instance: " + instanceName);
            }
        }

        public List<Table> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user)
        {
            List<Table> tablesList = new List<Table>();

            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {

                    tablesList = _dbRepository.GetAllTablesData(instanceName, databaseName);
                });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch (InvalidOperationException ex)
            {

                throw ex;
            }
            catch (SqlException ex)
            {

                throw ex;
            }

            if (tablesList.Count > 0)
            {

                return tablesList;
            }
            else
            {

                throw new KeyNotFoundException("No databases found on the instance: " + instanceName);
            }
        }

        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName, WindowsIdentity user)
        {
            List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();
            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    tableDataList = _dbRepository.GetTableData(instanceName, databaseName, tableName);
                });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            if (tableDataList.Count > 0)
            {

                return tableDataList;
            }
            else
            {

                throw new KeyNotFoundException("No data found in the table: " + tableName);
            }
        }

        public string ExecuteQuery(string instanceName, string request, WindowsIdentity user)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable RestoreDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            DataTable resultTable = new DataTable();

            try
            {
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable ChangeRecoveryModel(string instanceName, string databaseName, string recoveryModel, WindowsIdentity user)
        {
            DataTable resultTable = new DataTable();

            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    resultTable = _dbRepository.ChangeRecoveryModel(instanceName, databaseName, recoveryModel);
                });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

                return resultTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}