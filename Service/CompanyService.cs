using Contracts;
using Contracts.Repository;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = 
                repository.Company.GetAllCompanies(trackChanges);

            var companiesDto = companies.Select(c =>
                new CompanyDto(c.Id, c.Name ?? "", string.Join(' ',
                    c.Address, c.Country))).ToList();

            return companiesDto;
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}