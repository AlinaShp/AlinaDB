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
        [Route("checkConnection")]
        public ActionResult<string> CheckConnection(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return BadRequest("Instance name cannot be null or empty.");
            }

            var response = new ApiResponse();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (WindowsIdentity)HttpContext.User.Identity;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                _dBService.CheckConnection(instanceName, user);
#pragma warning restore CS8604 // Possible null reference argument.
                return Ok("Connection successful.");
            }
            catch (InvalidOperationException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 500;
            }

            return StatusCode(response.StatusCode, response.ErrorMessage);
        }

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
                response.StatusCode = 404;
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

#pragma warning disable CS8604 // Possible null reference argument.
                result = _dBService.ExecuteQuery(instanceName, request, user);
#pragma warning restore CS8604 // Possible null reference argument.

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
        public ActionResult<string> RestoreDB(string databaseName, string instanceName)
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
                _dBService.RestoreDB(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok("Restore completed successfully.");
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
            catch (KeyNotFoundException ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 404;
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

        [HttpDelete]
        [Authorize]
        [Route("deleteDB/{databaseName}")]
        public ActionResult<string> DeleteDB(string databaseName, string instanceName)
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
                _dBService.DeleteDB(instanceName, databaseName, user);
#pragma warning restore CS8604 // Possible null reference argument.
                return Ok("DB deleted successfully.");
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