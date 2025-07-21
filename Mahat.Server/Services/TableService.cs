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
    public interface ITableService
    {
        public List<TableDto> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user);
        public List<Dictionary<string, object>> GetTableData(string instanceName, string databaseName, string tableName, WindowsIdentity user);

        public void AddTable(string instanceName, string databaseName, Table newTable, WindowsIdentity user);
        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData, WindowsIdentity user);
    }

    public class TableService(ITableRepository tableRepository) : ITableService
    {

        public readonly ITableRepository _tableRepository = tableRepository;

        public List<TableDto> GetAllTablesData(string instanceName, string databaseName, WindowsIdentity user)
        {
            List<TableDto> tablesList = new List<TableDto>();

#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                tablesList = _tableRepository.GetAllTablesData(instanceName, databaseName);
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
                tableDataList = _tableRepository.GetTableData(instanceName, databaseName, tableName);
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

        public void AddTable(string instanceName, string databaseName, Table newTable, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _tableRepository.AddTable(instanceName, databaseName, newTable);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void InsertRow(string instanceName, string databaseName, string tableName, Dictionary<string, object> rowData, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _tableRepository.InsertRow(instanceName, databaseName, tableName, rowData);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}