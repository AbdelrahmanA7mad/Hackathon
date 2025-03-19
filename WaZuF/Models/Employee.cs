namespace WaZuF.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Score { get; set; }
        public int JobRequestId { get; set; }
        public JobRequest JobRequest { get; set; } = null!;
        public DateTime AttemptDate { get; set; } = DateTime.Now;
        public List<EmployeeAnswer> EmployeeAnswers { get; set; } = new();
    }

}
