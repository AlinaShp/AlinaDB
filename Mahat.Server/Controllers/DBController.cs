using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using Mahat.Server.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Text;
using Mahat.Server.Services;
namespace Mahat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBcontroller(IDBService dBService) : ControllerBase
    {
        public readonly IDBService _dBService = dBService;

        [HttpGet]
        [Authorize]
        [Route("DBdata")]
        public string DBdata(string instanceName)
        {
            ApiResponse response = new ApiResponse();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            try
            {
                List<DB> dbList = new List<DB>();

#pragma warning disable CS8604 // Possible null reference argument.
                dbList = _dBService.GetDBInfo(instanceName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return JsonConvert.SerializeObject(dbList);
            }
            catch (SqlException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return JsonConvert.SerializeObject(response);
        }


        [HttpGet]
        [Authorize]
        [Route("tablesInfo/{databaseName}")]
        public string AllTablesData(string databaseName, string instanceName)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                List<Table> tablesList = new List<Table>();

#pragma warning disable CS8604 // Possible null reference argument.
                tablesList = _dBService.GetAllTablesData(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return JsonConvert.SerializeObject(tablesList);
            }
            catch (SqlException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        [Authorize]
        [Route("tableData/{databaseName}/{tableName}")]
        public string TableData(string databaseName, string tableName, string instanceName)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();

#pragma warning disable CS8604 // Possible null reference argument.
                tableDataList = _dBService.GetTableData(instanceName, databaseName, tableName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return JsonConvert.SerializeObject(tableDataList);
            }
            catch (InvalidOperationException ex)
            {
                response.StatusCode = 400;
                response.ErrorMessage = ex.Message;

            }

            catch (SqlException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }

            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return JsonConvert.SerializeObject(response);
        }


        [HttpPost]
        [Authorize]
        [Route("executeQuery")]
        public string ExecuteQuery(string instanceName, [FromBody] string request)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                string result = string.Empty;

                result = _dBService.ExecuteQuery(instanceName, request, user);

                return result;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        [Authorize]
        [Route("restore/{databaseName}")]
        public string RestoreDB(string databaseName, string instanceName)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                DataTable resultTable = new DataTable();

#pragma warning disable CS8604 // Possible null reference argument.
                resultTable = _dBService.RestoreDB(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return JsonConvert.SerializeObject(resultTable);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Internal server error: {ex.Message}";
                response.StatusCode = 500;
            }

            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        [Authorize]
        [Route("backup/{databaseName}")]
        public string BackupDB(string databaseName, string instanceName)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.BackupDB(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.
                response.StatusCode = 200;
                response.ErrorMessage = "Backup completed successfully.";
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return JsonConvert.SerializeObject(response);

        }

        [HttpPatch]
        [Authorize]
        [Route("changeRecoveryModel/{databaseName}")]
        public string ChangeRecoveryModel(string instanceName, string databaseName, [FromBody] string recoveryModel)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                DataTable resultTable = new DataTable();

#pragma warning disable CS8604 // Possible null reference argument.
                resultTable = _dBService.ChangeRecoveryModel(instanceName, databaseName, recoveryModel, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return JsonConvert.SerializeObject(resultTable);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return JsonConvert.SerializeObject(response);
        }
    }
}