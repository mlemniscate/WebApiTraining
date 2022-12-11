using Contracts;
using Contracts.Repository;
using Service.Contracts;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager repositoryManager;
    private readonly Lazy<IEmployeeService> employeeService;
    private readonly Lazy<ICompanyService> companyService;

    public ServiceManager(ILoggerManager logger, IRepositoryManager repositoryManager)
    {
        this.repositoryManager = repositoryManager;
        this.employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, logger));
        this.companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager, logger));
    }

    public ICompanyService CompanyService => companyService.Value;
    public IEmployeeService EmployeeService => employeeService.Value;
}