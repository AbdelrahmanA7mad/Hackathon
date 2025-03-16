using System.ComponentModel.DataAnnotations.Schema;

namespace WaZuF.Models
{
    public class JobRequest
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public int NumberOfQuestions { get; set; }
        public string DifficultyLevel { get; set; } = "Medium"; 

        [NotMapped] 
        public Timer? MyTimer { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public List<Question> Questions { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
    }
    
}
