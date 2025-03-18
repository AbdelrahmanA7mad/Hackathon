using System.ComponentModel.DataAnnotations.Schema;

namespace WaZuF.Models
{
    public class CodeQuiz
    {
        public int Id { get; set; }
        public string ? Description { get; set; }

        public List<Exam> codes { get; set; }

        public string UserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

    }
}
