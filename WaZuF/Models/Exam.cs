using System.ComponentModel.DataAnnotations.Schema;

namespace WaZuF.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public string description { get; set; }
        public string Question { get; set; }
        public int QuizId { get; set; }
        public CodeQuiz CodeQuiz { get; set; }

    }
}
