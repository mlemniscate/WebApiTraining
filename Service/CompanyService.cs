using Contracts;
using Contracts.Repository;
using Entities.Models;
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

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = 
                repository.Company.GetAllCompanies(trackChanges);
            return companies;
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}