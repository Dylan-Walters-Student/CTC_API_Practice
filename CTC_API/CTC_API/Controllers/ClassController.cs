using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;

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
