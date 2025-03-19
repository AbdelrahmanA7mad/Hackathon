using WaZuF.Models;

namespace WaZuF.ViewModels
{
    public class ExamViewModel
    {
        public int EmployeeId { get; set; } // معرف الموظف الذي يقوم بالامتحان
        public List<Question> Questions { get; set; } // قائمة الأسئلة الخاصة بالامتحان

        public ExamViewModel()
        {
            Questions = new List<Question>(); // تهيئة القائمة لمنع حدوث NullReferenceException
        }
    }
}
