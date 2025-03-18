using WaZuF.EmpViewModel;
using WaZuF.Models;

namespace WaZuF.EmpServices
{
    public interface IEmpService
    {
        Task<string> GenerateCodingTasksAsync(CodeQuiz viewModel);
        Task<string> CheckSolutionAsync(string problemStatement, string userSolution);
        Task<string> EvaluateCodeAsync(CodeSubmission submission);
    }
}
