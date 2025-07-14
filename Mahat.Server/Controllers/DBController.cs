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
using Azure;
using Mahat.Server.DTOs;
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
        public ActionResult<List<DB>> DBdata(string instanceName)
        {

            if (string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Instance name cannot be null or empty.");
            }

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

                return Ok(dbList);
            }
            catch (InvalidOperationException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }


        [HttpGet]
        [Authorize]
        [Route("tablesInfo/{databaseName}")]
        public ActionResult<List<TableDto>> AllTablesData(string databaseName, string instanceName)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name and instance name cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                List<TableDto> tablesList = new List<TableDto>();

#pragma warning disable CS8604 // Possible null reference argument.
                tablesList = _dBService.GetAllTablesData(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok(tablesList);
            }
            catch (InvalidOperationException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpGet]
        [Authorize]
        [Route("tableData/{databaseName}/{tableName}")]
        public ActionResult<List<Dictionary<string, object>>> TableData(string databaseName, string tableName, string instanceName)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name, table name, and instance name cannot be null or empty.");
            }

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

                return Ok(tableDataList);
            }
            catch (InvalidOperationException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
            }
            catch (KeyNotFoundException ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = ex.Message;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }


        [HttpPost]
        [Authorize]
        [Route("executeQuery")]
        public ActionResult<string> ExecuteQuery(string instanceName, [FromBody] string request)
        {

            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(request))
            {
                return BadRequest("Instance name and request cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
                string result = string.Empty;

                result = _dBService.ExecuteQuery(instanceName, request, user);

                return Ok(result);

            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        [Route("restore/{databaseName}")]
        public ActionResult<DataTable> RestoreDB(string databaseName, string instanceName)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name and instance name cannot be null or empty.");
            }

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

                return Ok(resultTable);
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        [Route("backup/{databaseName}")]
        public ActionResult<string> BackupDB(string databaseName, string instanceName)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name and instance name cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.BackupDB(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Backup completed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);

        }
        [HttpPost]
        [Authorize]
        [Route("addDB/{databaseName}")]
        public ActionResult<string> AddDB(string databaseName, string instanceName, string collection = "SQL_Latin1_General_CP1_CI_AS")
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name and instance name cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.AddDB(instanceName, databaseName, collection, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("DB created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        [Route("addTable/{databaseName}")]
        public ActionResult<string> AddTable(string databaseName, string instanceName, [FromBody] Table newTable)
        {


            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name and instance name cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.AddTable(instanceName, databaseName, newTable, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Table created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);

        }

        [HttpPost]
        [Authorize]
        [Route("insertRow/{databaseName}/{tableName}")]
        public ActionResult<string> InsertRow(string tableName, string databaseName, string instanceName, [FromBody] Dictionary<string, object> rowData)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Table name, database name, and instance name cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.InsertRow(instanceName, databaseName, tableName, rowData, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Row inserted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);

        }
    }
}