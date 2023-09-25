using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;

namespace CTC_API.Controllers
{
    [ApiController]
    [Route("api/schools")]
    public class SchoolController : ControllerBase
    {
        private readonly string _connectionString;

        public SchoolController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
        }

        [HttpGet(Name = "GetSchools")]
        public IEnumerable<School> Get()
        {
            var school = new List<School>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM schools", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        school.Add(new School
                        {
                            SchoolId = (int)reader["school_id"],
                            SchoolName = reader["school_name"].ToString()
                        });
                    }
                }
            }
            return school;
        }
    }
}
