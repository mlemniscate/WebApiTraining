﻿using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.Exceptions;
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

    public CompanyService(IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<CompanyDto>, MetaData)> GetAllCompaniesAsync(
        CompanyParameters companyParameters, bool trackChanges)
    {
        var companiesPagedList =
            await repository.Company.GetAllCompaniesAsync(companyParameters, trackChanges);

        var companiesDto = 
            mapper.Map<IEnumerable<CompanyDto>>(companiesPagedList);

        return (companies: companiesDto, matadata: companiesPagedList.MetaData);
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
