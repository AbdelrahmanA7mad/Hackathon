using WaZuF.EmpViewModel;
using WaZuF.Models;

namespace WaZuF.EmpServices
{
    public interface IEmpService
    {
        Task<string> GenerateCodingTasksAsync(Exam viewModel);
        Task<string> CheckSolutionAsync(string problemStatement, string userSolution);
        Task<string> EvaluateCodeAsync(CodeSubmission submission);

        Task<List<Exam>> GetAllExams();



    }
}
