using System.Collections.Generic;
using System.Threading.Tasks;
using WaZuF.Models;

namespace WaZuF.Services
{
    public interface IQuestionService
    {
        Task<List<Question>> GetQuestionsByJobRequestIdAsync(int jobRequestId);
        Task<int?> GetLatestJobRequestIdAsync();
        Task<Question> GetQuestionByIdAsync(int questionId); // لجلب سؤال معين
        Task<bool> UpdateQuestionAsync(Question question); // لتعديل سؤال
        Task<bool> RegenerateQuestionAsync(int questionId); // لإعادة توليد سؤال
        Task<bool> DeleteQuestionAsync(int questionId); // لحذف سؤال
    }
}