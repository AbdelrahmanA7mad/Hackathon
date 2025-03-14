using WaZuF.ViewModels;

namespace WaZuF.Services
{
    public interface IJopService
    {
        Task CreateAsync(CreateJopViewModel viewModel, string companyId);

    }
}
