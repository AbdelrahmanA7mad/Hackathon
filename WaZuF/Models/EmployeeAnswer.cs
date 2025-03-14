namespace WaZuF.Models
{
    public class EmployeeAnswer
    {
        public int Id { get; set; }

        public int EmployeesId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public char SelectedAnswer { get; set; } // 'A' أو 'B' أو 'C' أو 'D'
    }

}
