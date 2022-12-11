using Contracts;
using Contracts.Repository;
using Service.Contracts;

namespace Service;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
}