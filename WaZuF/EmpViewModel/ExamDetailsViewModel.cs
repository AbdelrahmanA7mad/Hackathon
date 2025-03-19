namespace WaZuF.EmpViewModel
{
    public class ExamDetailsViewModel
    {
        public int Id { get; set; }
        public string question { get; set; }
        public string solution { get; set; }
        public bool Status { get; set; }
        public int Attempts { get; set; }
        public double Success_Rate { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
    }
}
