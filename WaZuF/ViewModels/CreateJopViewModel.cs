using System.ComponentModel.DataAnnotations;
using WaZuF.Models;

namespace WaZuF.ViewModels
{
    public class CreateJopViewModel
    {
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Required Skills")]
        public string RequiredSkills { get; set; } = string.Empty;

        [Required]
        [Range(1, 50)]
        [Display(Name = "Number of Questions")]
        public int NumberOfQuestions { get; set; }

        [Required]
        [Display(Name = "Difficulty Level")]
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Medium;

        public string ExamLink { get; set; } = string.Empty;

    }
}
