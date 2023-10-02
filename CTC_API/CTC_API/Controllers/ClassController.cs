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
            string commandText = "INSERT INTO schools (class_id, class_name) " +
                                "VALUES (@class_id, @class_name);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@class_id", SqlDbType.Int);
                    cmd.Parameters["@class_id"].Value = classes.ClassId;

                    cmd.Parameters.Add("@class_name", SqlDbType.NVarChar);
                    cmd.Parameters["@class_name"].Value = classes.ClassName;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
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
    }
}
