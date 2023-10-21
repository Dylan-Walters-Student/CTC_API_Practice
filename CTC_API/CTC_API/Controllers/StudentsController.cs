using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;

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

        [HttpPost(Name = "CreateStudents")]
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            string commandText = "INSERT INTO students (first_name, last_name, username, session_code, school_id, class_id) " +
                                "VALUES (@first_name, @last_name, @username, @session_code, @school_id, @class_id);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();
                    try
                    {
                        //first name
                        cmd.Parameters.Add("@first_name", SqlDbType.NVarChar);
                        cmd.Parameters["@first_name"].Value = student.FirstName;
                        //last name
                        cmd.Parameters.Add("@last_name", SqlDbType.NVarChar);
                        cmd.Parameters["@last_name"].Value = student.LastName;
                        //username
                        cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                        cmd.Parameters["@username"].Value = student.Username;
                        //session code
                        cmd.Parameters.Add("@session_code", SqlDbType.NVarChar);
                        cmd.Parameters["@session_code"].Value = student.SessionCode;
                        //school id
                        cmd.Parameters.Add("@school_id", SqlDbType.Int);
                        cmd.Parameters["@school_id"].Value = student.SchoolId;
                        //class id
                        cmd.Parameters.Add("@class_id", SqlDbType.Int);
                        cmd.Parameters["@class_id"].Value = student.ClassId;

                        cmd.ExecuteNonQuery();
                        return Ok("Woot Woot");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
            }
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

        [HttpPut(Name = "UpdateStudents")]
        public async Task<IActionResult> Update([FromBody] Student student)
        {
            string commandText = $"UPDATE students " +
                $"SET first_name = @first_name " +
                $",last_name = @last_name " +
                $",username = @username " +
                $",session_code = @session_code " +
                $",school_id = @school_id " +
                $",class_id = @class_id " +
                $"WHERE student_id = @student_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    //student id
                    cmd.Parameters.Add("@student_id", SqlDbType.Int);
                    cmd.Parameters["@student_id"].Value = student.StudentId;
                    //first name
                    cmd.Parameters.Add("@first_name", SqlDbType.NVarChar);
                    cmd.Parameters["@first_name"].Value = student.FirstName;
                    //last name
                    cmd.Parameters.Add("@last_name", SqlDbType.NVarChar);
                    cmd.Parameters["@last_name"].Value = student.LastName;
                    //username
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                    cmd.Parameters["@username"].Value = student.Username;
                    //session code
                    cmd.Parameters.Add("@session_code", SqlDbType.NVarChar);
                    cmd.Parameters["@session_code"].Value = student.SessionCode;
                    //school id
                    cmd.Parameters.Add("@school_id", SqlDbType.Int);
                    cmd.Parameters["@school_id"].Value = student.SchoolId;
                    //class id
                    cmd.Parameters.Add("@class_id", SqlDbType.Int);
                    cmd.Parameters["@class_id"].Value = student.ClassId;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
                }
            }
        }

        [HttpDelete(Name = "DeleteStudent")]
        public async Task<IActionResult> Delete([FromBody] Student student)
        {
            string commandText = "DELETE FROM students WHERE student_id LIKE @student_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@student_id", SqlDbType.Int);
                    cmd.Parameters["@student_id"].Value = student.StudentId;

                    cmd.ExecuteNonQuery();
                    return Ok("woot woot");
                }
            }
        }
    }
}
