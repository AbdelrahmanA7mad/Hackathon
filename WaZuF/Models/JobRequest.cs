using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaZuF.Models
{
    public class JobRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Required Skills")]
        public string RequiredSkills { get; set; } = string.Empty;

        [Range(1, 50)]
        public int NumberOfQuestions { get; set; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Medium;

        [NotMapped]
        public Timer? MyTimer { get; set; }

        [Required]
        public string CompanyId { get; set; } = null!;

        [ForeignKey("CompanyId")]
        public AppUser AppUser { get; set; } = null!;
        [StringLength(500)]

        public string ? Location { get; set; } = "Not specified"; // Default if no location is provided
        public string ?Type { get; set; } = "Full-time"; // Default Job Type
        public DateTime ? Posted { get; set; } = DateTime.Now; // Default Posted Date

        public string? ExamLink { get; set; }
        public List<Question> Questions { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
    }

}
