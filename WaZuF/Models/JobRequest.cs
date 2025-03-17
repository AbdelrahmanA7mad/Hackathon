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

        public List<Question> Questions { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
    }

}
