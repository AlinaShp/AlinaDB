using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;
using Mahat.Server.DTOs;
using Mahat.Server.Enums;
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
        public void RestoreDB(string databaseName, string instanceName, List<BackupInfo> backupInfo, WindowsIdentity user);
        public void BackupDB(string instanceName, string databaseName, BackupInfo backupInfo, WindowsIdentity user);
        public void AddDB(string instanceName, string databaseName, string collation , WindowsIdentity user);
        public void DeleteDB(string instanceName, string databaseName, WindowsIdentity user);
        public bool IsDBExists(string instanceName, string databaseName, WindowsIdentity user);
        public List<string> GetExistingBackups(string instanceName, string databaseName, BackupTypes backupType, WindowsIdentity user);
        public bool IsBackupExist(string instanceName, string databaseName, string filePath, WindowsIdentity user);

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

        public void RestoreDB(string instanceName, string databaseName,  List<BackupInfo> backupInfo, WindowsIdentity user)
        {
            if (!IsDBExists(instanceName, databaseName, user))
            {
                
                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.RestoreDB(instanceName, databaseName, backupInfo);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void BackupDB(string instanceName, string databaseName, BackupInfo backupInfo, WindowsIdentity user)
        {
            if (!IsDBExists(instanceName, databaseName, user))
            {
                
                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }


#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.BackupDB(instanceName, databaseName, backupInfo);
            });
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public void AddDB(string instanceName, string databaseName, string collation , WindowsIdentity user)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                _dbRepository.AddDB(instanceName, databaseName, collation);
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
        public List<string> GetExistingBackups(string instanceName, string databaseName, BackupTypes backupType, WindowsIdentity user)
        {
            
            if (!IsDBExists(instanceName, databaseName, user))
            {

                throw new KeyNotFoundException($"Database '{databaseName}' does not exist on instance '{instanceName}'.");
            }

            List<string> backupList = new List<string>();

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                backupList = _dbRepository.GetExistingBackups(instanceName, databaseName, backupType);
            });
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility

            backupList = backupList.FindAll(backup => IsBackupExist(instanceName, databaseName, backup, user));

            if (backupList.Count > 0)
            {

                return backupList;
            }

            else
            {

                throw new KeyNotFoundException($"No backups of type {backupType} for database '{databaseName}' on instance '{instanceName}'.");
            }

        }
        public bool IsBackupExist(string instanceName, string databaseName, string filePath, WindowsIdentity user)
        {
            bool isExist = false;

#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {
                isExist = _dbRepository.IsBackupExist(instanceName, databaseName, filePath);
            });
#pragma warning restore CA1416 // Validate platform compatibility

            return isExist;
        }
    }
}