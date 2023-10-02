using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;

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

        [HttpPost(Name = "CreateSchool")]
        public async Task<IActionResult> Create([FromBody] School school)
        {
            string commandText = "INSERT INTO schools (school_name) " +
                                "VALUES (@school_name);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();
                    
                    cmd.Parameters.Add("@school_name", SqlDbType.NVarChar);
                    cmd.Parameters["@school_name"].Value = school.SchoolName;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
                }
            }
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

        [HttpPut(Name = "UpdateSchools")]
        public async Task<IActionResult> Update([FromBody] School school)
        {
            string commandText = $"UPDATE schools SET school_name = @school_name WHERE school_id = @school_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@school_id", SqlDbType.Int);
                    cmd.Parameters["@school_id"].Value = school.SchoolId;

                    cmd.Parameters.Add("@school_name", SqlDbType.NVarChar);
                    cmd.Parameters["@school_name"].Value = school.SchoolName;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
                }
            }
        }
    }
}
