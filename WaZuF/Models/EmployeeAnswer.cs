namespace WaZuF.Models
{
    public class EmployeeAnswer
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; } // كان EmployeesId
        public Employee Employee { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public char SelectedAnswer { get; set; }
    }

}
