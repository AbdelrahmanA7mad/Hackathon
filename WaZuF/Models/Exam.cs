using System.ComponentModel.DataAnnotations.Schema;

namespace WaZuF.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public string description { get; set; }
        public string Question { get; set; }
        public string ? solution { get; set; }
        public int ?QuizId { get; set; }

        public int Tries { get; set; } = 1;

        public bool Solved { get; set; }

        public string? AppUserId { get; set; }  // Allow null values

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; } = null!;


    }
}
