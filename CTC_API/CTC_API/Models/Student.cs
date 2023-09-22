namespace CTC_API.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string SessionCode { get; set; }
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
    }
}
