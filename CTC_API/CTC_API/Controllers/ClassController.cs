using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace CTC_API.Controllers
{
    [ApiController]
    [Route("api/classes")]
    public class ClassController : ControllerBase
    {
        private readonly string _connectionString;

        public ClassController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
        }

        [HttpPost(Name = "CreateClasses")]
        public async Task<IActionResult> Create([FromBody] Class classes)
        {
            string commandText = "INSERT INTO classes (class_name) " +
                                "VALUES (@class_name);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@class_name", SqlDbType.NVarChar);
                        cmd.Parameters["@class_name"].Value = classes.ClassName;

                        cmd.ExecuteNonQuery();
                        return Ok("woot woot");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        [HttpGet(Name = "GetClasses")]
        public IEnumerable<Class> Get()
        {
            var _class = new List<Class>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM classes", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _class.Add(new Class
                        {
                            ClassId = (int)reader["class_id"],
                            ClassName = reader["class_name"].ToString()
                        });
                    }
                }
            }
            return _class;
        }

        [HttpPut(Name = "UpdateClasses")]
        public async Task<IActionResult> Update([FromBody] Class classes)
        {
            string commandText = $"UPDATE classes SET class_name = @class_name WHERE class_id = @class_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@class_id", SqlDbType.Int);
                        cmd.Parameters["@class_id"].Value = classes.ClassId;

                        cmd.Parameters.Add("@class_name", SqlDbType.NVarChar);
                        cmd.Parameters["@class_name"].Value = classes.ClassName;

                        cmd.ExecuteNonQuery();
                        return Ok("woot woot");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        [HttpDelete(Name = "DeleteClasses")]
        public async Task<IActionResult> Delete([FromBody] Class classes)
        {
            string commandText = "DELETE FROM classes WHERE class_id LIKE @class_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@class_id", SqlDbType.Int);
                    cmd.Parameters["@class_id"].Value = classes.ClassId;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
                }
            }
        }
    }
}
