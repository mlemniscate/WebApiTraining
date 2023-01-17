using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager repositoryManager;
    private readonly IMapper mapper;
    private readonly IEmployeeLinks employeeLinks;
    private readonly Lazy<IEmployeeService> employeeService;
    private readonly Lazy<ICompanyService> companyService;
    private readonly Lazy<IAuthenticationService> authenticationService;

    public ServiceManager(ILoggerManager logger,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IEmployeeLinks employeeLinks,
        ICompanyLinks companyLinks,
        UserManager<User> userManager,
        IOptions<JwtConfiguration> configuration)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
        this.employeeLinks = employeeLinks;
        this.companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager, logger, mapper, companyLinks));
        this.employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, logger, mapper, employeeLinks));
        this.authenticationService = new Lazy<IAuthenticationService>(() => 
            new AuthenticationService(logger, mapper, userManager, configuration));
    }

    public ICompanyService CompanyService => companyService.Value;
    public IEmployeeService EmployeeService => employeeService.Value;
    public IAuthenticationService AuthenticationService => authenticationService.Value;
}