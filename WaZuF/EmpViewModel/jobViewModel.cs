using System.ComponentModel.DataAnnotations;

namespace WaZuF.EmpViewModel
{
    public class jobViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string RequiredSkills { get; set; } = string.Empty;

        [Range(1, 50)]
        public int NumberOfQuestions { get; set; }

        [Required]
        public string DifficultyLevel { get; set; } = "Medium"; // Convert Enum to String

        [Required]
        public string Company { get; set; } = string.Empty; // Display Company Name Instead of ID

        public string? ExamLink { get; set; }

        public string Location { get; set; } = "Not specified"; // Default if no location is provided
        public string Type { get; set; } = "Full-time"; // Default Job Type
        public DateTime Posted { get; set; } = DateTime.Now; // Default Posted Date
    }
}

