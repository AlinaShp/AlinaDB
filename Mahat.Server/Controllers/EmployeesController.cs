using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mahat.Server.Models;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Mahat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Требуется аутентификация для доступа к этому контроллеру
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                // Получение access token из запроса
                //var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Connection string для SQL Server с Windows аутентификацией
                string connectionString = _configuration.GetConnectionString("EmployeeAppCon");

                using (var con = new SqlConnection(connectionString))
                {
                    // Создание SQL команды для выполнения запроса к базе данных
                    using (var cmd = new SqlCommand("SELECT id, name, lastname, job FROM Employees", con))
                    {
                        con.Open();

                        // Если требуется, можно использовать полученный токен для других целей
                        // Например, передать его как параметр запроса или в заголовке

                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Преобразование данных из DataTable в список объектов Employee
                        var employeeList = new List<Employee>();
                        foreach (DataRow row in dt.Rows)
                        {
                            var employee = new Employee
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                LastName = Convert.ToString(row["lastname"]),
                                Job = Convert.ToString(row["job"])
                            };
                            employeeList.Add(employee);
                        }

                        // Проверка наличия данных и возврат результата
                        if (employeeList.Count > 0)
                        {
                            return Ok(employeeList);
                        }
                        else
                        {
                            return NotFound("No employees found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
