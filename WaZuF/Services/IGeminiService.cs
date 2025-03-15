using WaZuF.Models;

namespace WaZuF.Services
{
    public interface IGeminiService
    {
        Task<List<Question>> GenerateQuizQuestionsAsync(JobRequest jobRequest);
    }
}
