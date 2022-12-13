using AutoMapper;
using Contracts;
using Contracts.Repository;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;

    public CompanyService(IRepositoryManager repository, 
        ILoggerManager logger,
        IMapper mapper)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = 
                repository.Company.GetAllCompanies(trackChanges);

            var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}
