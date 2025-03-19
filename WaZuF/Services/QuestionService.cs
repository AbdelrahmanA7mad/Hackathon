using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WaZuF.Data;
using WaZuF.Models;
using WaZuF.Services;

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _context;

    public QuestionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> GetLatestJobRequestIdAsync()
    {
        return await _context.Questions
                             .OrderByDescending(q => q.JobRequestId)
                             .Select(q => q.JobRequestId)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<Question>> GetQuestionsByJobRequestIdAsync(int jobRequestId)
    {
        return await _context.Questions
                             .Where(q => q.JobRequestId == jobRequestId)
                             .ToListAsync();
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions.ToListAsync();
    }

    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions
                             .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<bool> UpdateQuestionAsync(Question question)
    {
        var existingQuestion = await _context.Questions.FindAsync(question.Id);
        if (existingQuestion == null)
        {
            return false;
        }

        // تحديث الحقول
        existingQuestion.Text = question.Text;
        existingQuestion.OptionA = question.OptionA;
        existingQuestion.OptionB = question.OptionB;
        existingQuestion.OptionC = question.OptionC;
        existingQuestion.OptionD = question.OptionD;

        _context.Questions.Update(existingQuestion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RegenerateQuestionAsync(int questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question == null)
        {
            return false;
        }

        // منطق إعادة التوليد (مثال بسيط: استبدال النص بآخر جديد)
        question.Text = "Regenerated Question: " + DateTime.Now.ToString();
        question.OptionA = "New Option A";
        question.OptionB = "New Option B";
        question.OptionC = "New Option C";
        question.OptionD = "New Option D";

        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question == null)
        {
            return false;
        }

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return true;
    }
}