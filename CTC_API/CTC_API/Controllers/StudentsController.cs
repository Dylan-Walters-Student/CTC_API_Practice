using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;

namespace CTC_API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;

        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
        }

        [HttpGet(Name = "GetStudents")]
        public IEnumerable<Student> Get()
        {
            var students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM students", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentId = (int)reader["student_id"],
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Username = reader["username"].ToString(),
                            SessionCode = reader["session_code"].ToString(),
                            SchoolId = (int)reader["school_id"],
                            ClassId = (int)reader["class_id"]
                        });
                    }
                }
            }
            return students;
        }
    }
}
