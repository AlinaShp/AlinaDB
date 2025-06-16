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
namespace Mahat.Server.Controllers
{
[Route("api/[controller]")]
    [ApiController]
    public class DBcontroller : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public DBcontroller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Authorize]
        [Route("DBdata")]
        public string  DBdata(string instanceName)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;


             
            string connectionString = "Server=" + instanceName + ";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT name,database_id, state_desc, recovery_model_desc, compatibility_level, " +
                "collation_name FROM sys.databases WHERE name NOT IN ('master', 'model', 'tempdb', 'msdb');\r\n ", con);
            List<DB> dbList = new List<DB>();
                ApiResponse response = new ApiResponse();

            try
            {
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {

                    con.Open();
                    DataTable dt = new DataTable();
                    da.Fill(dt);



                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DB database = new DB();
                            database.DatabaseName = Convert.ToString(dt.Rows[i]["name"]);
                            database.DatabaseId = Convert.ToInt32(dt.Rows[i]["database_id"]);
                            database.State = Convert.ToString(dt.Rows[i]["state_desc"]);
                            database.RecoveryModel = Convert.ToString(dt.Rows[i]["recovery_model_desc"]);
                            database.CompatibilityLevel = Convert.ToInt32(dt.Rows[i]["compatibility_level"]);
                            database.Collation = Convert.ToString(dt.Rows[i]["collation_name"]);
                            SqlDataAdapter daTableNames = new SqlDataAdapter("USE " + database.DatabaseName + ";  ", con);
                            DataTable tablesNameDT = new DataTable();
                            daTableNames.Fill(tablesNameDT);
                            for (int j = 0; j < tablesNameDT.Rows.Count; j++)
                            {
                                System.Diagnostics.Debug.WriteLine("USE " + database.DatabaseName + "; SELECT name FROM sys.tables;");


                                database.Tables.Add(Convert.ToString(tablesNameDT.Rows[j]["name"]));
                            }
                            dbList.Add(database);


                        }
                    }
                });
                
            }
            catch (SqlException ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = ex.Message;
                return JsonConvert.SerializeObject(response);
            }
            if (dbList.Count > 0)
            {
                return JsonConvert.SerializeObject(dbList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
            }
            return JsonConvert.SerializeObject(response);
        }
   

        [HttpGet]
        [Authorize]
        [Route("tablesInfo/{databaseName}")]
        public string allTablesData(string databaseName, string instanceName)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            ApiResponse response = new ApiResponse();

            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(databaseName))
            {
                response.StatusCode = 400;
                response.ErrorMessage = "All parameters must be provided.";
                return JsonConvert.SerializeObject(response);
            }

            string connectionString = "Server=" + instanceName + ";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("USE " + databaseName + "; SELECT tc.TABLE_NAME AS TableName, kcu.COLUMN_NAME AS PrimaryKey FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS" +
                " tc INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS kcu ON tc.CONSTRAINT_NAME" +
                " = kcu.CONSTRAINT_NAME WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY tc.TABLE_NAME;", con);
            
            DataTable dt = new DataTable();
            List<Table> tablesList = new List<Table>();

            WindowsIdentity.RunImpersonated(user.AccessToken, () =>
            {

                da.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Table table = new Table();
#pragma warning disable CS8601 // Possible null reference assignment.
                        table.TableName = Convert.ToString(dt.Rows[i]["TableName"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                        table.PrimaryKey = Convert.ToString(dt.Rows[i]["PrimaryKey"]);
                        tablesList.Add(table);


                    }
                }
            });
          
            if (tablesList.Count > 0)
            {
                return JsonConvert.SerializeObject(tablesList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
            }
            return JsonConvert.SerializeObject(response);
        }
           /* SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("USE " + databaseName + "; SELECT tc.TABLE_NAME AS TableName, kcu.COLUMN_NAME AS PrimaryKey FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS" +
                " tc INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS kcu ON tc.CONSTRAINT_NAME" +
                " = kcu.CONSTRAINT_NAME WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY tc.TABLE_NAME;", con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Table> tablesList = new List<Table>();

            ApiResponse response = new ApiResponse();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Table table = new Table();
                    table.TableName = Convert.ToString(dt.Rows[i]["TableName"]);
                    table.PrimaryKey = Convert.ToString(dt.Rows[i]["PrimaryKey"]);
                    tablesList.Add(table);


                }
            }
            if (tablesList.Count > 0)
            {
                return JsonConvert.SerializeObject(tablesList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
            }
            return JsonConvert.SerializeObject(response);
           */
        


        [HttpGet]
        [Authorize]
        [Route("tableData/{databaseName}/{tableName}")]
        public string tableData(string databaseName, string tableName, string instanceName)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();
            string connectionString = "Server=" + instanceName + ";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("USE " + databaseName + "; SELECT* FROM " + tableName, con);
            DataTable dt = new DataTable();
            ApiResponse response = new ApiResponse();

            try
            {
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    da.Fill(dt);
                });

                }
            catch
            {
                response.StatusCode = 400;
                response.ErrorMessage = "Invalid query";

            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        row[column.ColumnName] = dt.Rows[i][column];
                    }
                    tableDataList.Add(row);
                }
            }
            if (tableDataList.Count > 0)
            {
                return JsonConvert.SerializeObject(tableDataList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
            }
            return JsonConvert.SerializeObject(response);
        }


        [HttpPost]
        [Authorize]
        [Route("executeQuery")]
        public string executeQuery([FromBody] string request)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            ApiResponse response = new ApiResponse();
            if (string.IsNullOrEmpty(request))
            {
                response.ErrorMessage = "Query cannot be empty.";
                response.StatusCode= 400;
                return JsonConvert.SerializeObject(response);
            }
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon"));
            SqlCommand command = new SqlCommand(request, con);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable resultTable = new DataTable();
                    int rowsAffected = 0;

            try
            {
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    con.Open();

                    // Use ExecuteNonQuery to get the number of rows affected for non-select queries
                    if (request.TrimStart().IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0)
                    {

                        adapter.Fill(resultTable);
                    }
                    else
                    {
                        // For non-SELECT queries, use ExecuteNonQuery to get the number of rows affected
                        rowsAffected = command.ExecuteNonQuery();
                    }
                });
                    var result = new
                    {
                        RowsAffected = rowsAffected,
                        Result = resultTable
                    };

                    return JsonConvert.SerializeObject(result);
                
                }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Internal server error: {ex.Message}";
                response.StatusCode = 500;
                return JsonConvert.SerializeObject(response);

            }
        }

        [HttpPost]
        [Authorize]
        [Route("restore/{databaseName}")]
        public string restoreDB(string databaseName, string instanceName)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            string connectionString = "Server=" + instanceName + ";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString); 
            SqlDataAdapter adapter = new SqlDataAdapter("USE master; ALTER DATABASE"+ databaseName + "SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE bestplaceever FROM DISK = 'C:\\Backups\\"+databaseName+"\\File.bak' WITH REPLACE; ALTER DATABASE bestplaceever SET MULTI_USER;", con);
            DataTable resultTable = new DataTable();
            ApiResponse response = new ApiResponse();

            try
            {
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    con.Open();
                    adapter.Fill(resultTable);
                });
                

                return JsonConvert.SerializeObject(resultTable);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Internal server error: {ex.Message}";
                response.StatusCode = 500;
                return JsonConvert.SerializeObject(response);

            }
        }

        [HttpPost]
        [Authorize]
        [Route("backup/{databaseName}")]
        public string backupDB(string databaseName, string instanceName)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            ApiResponse response = new ApiResponse();

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(instanceName))
            {
                response.StatusCode = 400;
                response.ErrorMessage = "All parameters must be provided.";
                return JsonConvert.SerializeObject(response);
            }

            string connectionString = "Server=" + instanceName + ";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;";
            string backupQuery = $"BACKUP DATABASE {databaseName} TO DISK = 'C:\\Backups\\{databaseName}\\{databaseName}.bak' WITH FORMAT, INIT, SKIP;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(backupQuery, con))
                {
                    try
                    {
                        WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        });
                        response.StatusCode = 200;
                        response.ErrorMessage = "Backup completed successfully.";
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessage = $"Internal server error: {ex.Message}";
                        response.StatusCode = 500;
                    }
                }
            }
            return JsonConvert.SerializeObject(response);

        }

        /*    [HttpPatch]
            [Authorize]
            [Route("changeRecoveryModel/{databaseName}")]
            public string changeRecoveryModel(string databaseName, string recoveryModel, string username)
            {
                // SqlConnection con = new SqlConnection("Server = Project"+"SQL1"+"; Database = BestPlaceEver; Integrated Security = true");

                string connectionString = $"Server=ProjectSQL1;Database=BestPlaceEver;User Id=excelapp\\Alina;Password=Alina123456789;";
                SqlConnection con = new SqlConnection(connectionString);
                //SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon").ToString());
                SqlDataAdapter adapter = new SqlDataAdapter("ALTER DATABASE " + databaseName + " SET RECOVERY " + recoveryModel + "; SELECT name, recovery_model_desc FROM sys.databases WHERE name = 'bestplaceever'; ", con);
                DataTable resultTable = new DataTable();
                ApiResponse response = new ApiResponse();

                try
                {
                    con.Open();
                    adapter.Fill(resultTable);
                    return JsonConvert.SerializeObject(resultTable);
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = $"Internal server error: {ex.Message}";
                    response.StatusCode = 500;
                    return JsonConvert.SerializeObject(response);

                }
            }


        }
        */



        [HttpPatch]
        [Authorize]
        [Route("changeRecoveryModel/{databaseName}")]
        public string ChangeRecoveryModel(string instanceName,string databaseName, [FromBody] string recoveryModel)
        {
            var user = (WindowsIdentity)HttpContext.User.Identity;

            ApiResponse response = new ApiResponse();

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(recoveryModel) || string.IsNullOrEmpty(instanceName))
            {
                response.StatusCode = 400;
                response.ErrorMessage = "All parameters must be provided.";
                return JsonConvert.SerializeObject(response);
            }

            string connectionString = "Server="+instanceName+";Database=BestPlaceEver;Integrated Security=SSPI;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);

            SqlDataAdapter adapter = new SqlDataAdapter("ALTER DATABASE " + databaseName + " SET RECOVERY " + recoveryModel + "; SELECT name, recovery_model_desc FROM sys.databases WHERE name = '"+databaseName+"'; ", con);
            DataTable resultTable = new DataTable();

            try
            {
                WindowsIdentity.RunImpersonated(user.AccessToken, () =>
                {
                    con.Open();
                    adapter.Fill(resultTable);
                });
                return JsonConvert.SerializeObject(resultTable);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Internal server error: {ex.Message}";
                response.StatusCode = 500;
                return JsonConvert.SerializeObject(response);

            }
        }

        public class Impersonator : IDisposable
        {
            public WindowsIdentity Identity { get; private set; }

            public Impersonator(string username, string domain, string password)
            {
                var token = IntPtr.Zero;
                var tokenDuplicate = IntPtr.Zero;

                try
                {
                    if (LogonUser(username, domain, password, 2, 0, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            Identity = new WindowsIdentity(tokenDuplicate);
                        }
                        else
                        {
                            throw new ApplicationException("Failed to duplicate token.");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Failed to logon user.");
                    }
                }
                finally
                {
                    if (token != IntPtr.Zero)
                    {
                        CloseHandle(token);
                    }
                    if (tokenDuplicate != IntPtr.Zero)
                    {
                        CloseHandle(tokenDuplicate);
                    }
                }
            }

            public void Dispose()
            {
                Identity?.Dispose();
            }

            [DllImport("advapi32.dll", SetLastError = true)]
            private static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            private extern static bool CloseHandle(IntPtr handle);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private extern static int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);
        }
    }
}