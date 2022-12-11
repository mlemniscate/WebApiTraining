using Contracts;
using Contracts.Repository;
using Service.Contracts;

namespace Service;

public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
}