using AutoMapper;
using Contracts;
using Contracts.Repository;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager repositoryManager;
    private readonly IMapper mapper;
    private readonly IEmployeeLinks employeeLinks;
    private readonly IDataShaper<CompanyDto> companyDataShaper;
    private readonly Lazy<IEmployeeService> employeeService;
    private readonly Lazy<ICompanyService> companyService;

    public ServiceManager(ILoggerManager logger,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IEmployeeLinks employeeLinks,
        IDataShaper<CompanyDto> companyDataShaper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
        this.employeeLinks = employeeLinks;
        this.companyDataShaper = companyDataShaper;
        this.companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager, logger, mapper, companyDataShaper));
        this.employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, logger, mapper, employeeLinks));
    }

    public ICompanyService CompanyService => companyService.Value;
    public IEmployeeService EmployeeService => employeeService.Value;
}