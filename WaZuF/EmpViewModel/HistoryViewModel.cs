using WaZuF.Models;

namespace WaZuF.EmpViewModel
{
    public class HistoryViewModel
    {
        public int Id { get; set; }

        public string Solved_or_Not { get; set; }

        public int Attempts { get; set; }

        public List<Exam> exams { get; set; }


    }
}
