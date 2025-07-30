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
    public class TableController(ITableService tableService) : ControllerBase
    {
        public readonly ITableService _tableService = tableService;

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
                tablesList = _tableService.GetAllTablesData(instanceName, databaseName, user);
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
                response.StatusCode = 404;
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
                tableDataList = _tableService.GetTableData(instanceName, databaseName, tableName, user);
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
                response.StatusCode = 404;
                response.ErrorMessage = ex.Message;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        [Route("addTable/{databaseName}")]
        public ActionResult<string> AddTable(string databaseName, string instanceName, [FromBody] Table newTable)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName) || newTable == null || !newTable.IsNotNull(newTable))
            {
                return BadRequest("Database name , instance name and table cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _tableService.AddTable(instanceName, databaseName, newTable, user);
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

        [HttpDelete]
        [Authorize]
        [Route("dropTable/{databaseName}/{tableName}")]
        public ActionResult<string> DropTable(string databaseName, string tableName, string instanceName)
        {

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Database name, table name and instance name cannot be null or empty.");
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            ApiResponse response = new ApiResponse(); 
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _tableService.DropTable(instanceName, databaseName, tableName, user);
#pragma warning restore CS8604 // Possible null reference argument.
                return Ok("Table dropped successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }
            catch (KeyNotFoundException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 404;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        [Route("insertRow/{databaseName}/{tableName}")]
        public ActionResult<string> InsertRow(string tableName, string databaseName, string instanceName, [FromBody] Dictionary<string, object> rowData)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName) || rowData == null || rowData.Count == 0)
            {

                return BadRequest("Table name, database name, instance name and data cannot be null or empty.");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _tableService.InsertRow(instanceName, databaseName, tableName, rowData, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Row inserted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }
            catch (KeyNotFoundException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 404;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpPatch]
        [Authorize]
        [Route("updateRow/{databaseName}/{tableName}/{primaryKeyName}/{primaryKeyValue}")]
        public ActionResult<string> UpdateRow(string tableName, string databaseName, string instanceName, [FromRoute] string primaryKeyName, [FromRoute] string primaryKeyValue, [FromBody] Dictionary<string, object> rowData)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(primaryKeyName) || primaryKeyValue == null || rowData == null || rowData.Count == 0)
            {
                return BadRequest("Table name, database name, instance name, primary key and new data cannot be null or empty.");
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _tableService.UpdateRow(instanceName, databaseName, tableName, rowData, primaryKeyName, primaryKeyValue, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Row updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }
            catch (KeyNotFoundException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 404;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

        [HttpDelete]
        [Authorize]
        [Route("deleteRow/{databaseName}/{tableName}/{primaryKeyName}/{primaryKeyValue}")]
        public ActionResult<string> DeleteRow(string tableName, string databaseName, string instanceName, string primaryKeyName, string primaryKeyValue)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(primaryKeyName) || primaryKeyValue == null)
            {
                return BadRequest("Table name, database name, instance name and primary key cannot be null or empty.");
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            ApiResponse response = new ApiResponse();

            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _tableService.DeleteRow(instanceName, databaseName, tableName, primaryKeyName, primaryKeyValue, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Row removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }
            catch (KeyNotFoundException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 404;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }
    }
}