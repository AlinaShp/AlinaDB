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
        public void CheckConnection(string instanceName, WindowsIdentity user);

        public List<DB> GetDBInfo(string instanceName, WindowsIdentity user);
        public string ExecuteQuery(string instanceName, string request, WindowsIdentity user);
        public void RestoreDB(string instanceName, string databaseName, WindowsIdentity user);
        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user);
        public void AddDB(string instanceName, string databaseName, string collection, WindowsIdentity user);
        public void DeleteDB(string instanceName, string databaseName, WindowsIdentity user);
        public bool IsDBExists(string instanceName, string databaseName, WindowsIdentity user);

}

    public class DBService(IDBRepository dBRepository) : IDBService
    {

        public readonly IDBRepository _dbRepository = dBRepository;

        public void CheckConnection(string instanceName, WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.CheckConnection(instanceName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

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

        public void RestoreDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            if (!IsDBExists(instanceName, databaseName, user))
            {
                
                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.RestoreDB(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void BackupDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            if (!IsDBExists(instanceName, databaseName, user))
            {
                
                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }


#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.BackupDB(instanceName, databaseName);
            });
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

        public void DeleteDB(string instanceName, string databaseName, WindowsIdentity user)
        {
            if (!IsDBExists(instanceName, databaseName, user))
            {

                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }

#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.DeleteDB(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public bool IsDBExists(string instanceName, string databaseName, WindowsIdentity user)
        {
            bool isExist = false;

#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                isExist = _dbRepository.IsDBExists(instanceName, databaseName);
            });
#pragma warning restore CA1416 // Validate platform compatibility

            return isExist;
        }

    }
}