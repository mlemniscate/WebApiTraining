using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly ICompanyLinks companyLinks;

    public CompanyService(IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper,
        ICompanyLinks companyLinks)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
        this.companyLinks = companyLinks;
    }

    public async Task<(LinkResponse links, MetaData metaData)> GetAllCompaniesAsync(
        LinkParameters<CompanyParameters> linkParameters, bool trackChanges)
    {
        var companiesWithMetaData =
            await repository.Company.GetAllCompaniesAsync(linkParameters.RequestParameters, trackChanges);

        var companiesDto = 
            mapper.Map<IEnumerable<CompanyDto>>(companiesWithMetaData);

        var links = companyLinks
            .TryGenerateLinks(companiesDto, linkParameters.RequestParameters.Fields, linkParameters.HttpContext);
        
        return (links: links, matadata: companiesWithMetaData.MetaData);
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyCreateDto company)
    {
        var companyEntity = mapper.Map<Company>(company);

        repository.Company.CreateCompany(companyEntity);
        await repository.SaveAsync();

        var companyToReturn = mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var companyEntities = await repository.Company.GetByIdsAsync(ids, trackChanges);
        if (ids.Count() != companyEntities.Count())
            throw new CollectionByIdsBadRequestException();

        return mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyCreateDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();

        var companyEntities = mapper.Map<IEnumerable<Company>>(companyCollection);

        foreach (var company in companyEntities)
        {
            repository.Company.CreateCompany(company);
        }

        await repository.SaveAsync();

        var companyCollectionToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
        return (companies: companyCollectionToReturn, ids: ids);
    }

    public async Task DeleteCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        repository.Company.DeleteCompany(company);
        await repository.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid id, CompanyUpdateDto companyUpdateDto, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        mapper.Map(companyUpdateDto, company);
        await repository.SaveAsync();
    }

    private async Task<Company?> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(id, trackChanges);
        if (company is null) throw new CompanyNotFoundException(id);
        return company;
    }
}
