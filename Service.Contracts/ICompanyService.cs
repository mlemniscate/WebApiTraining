
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<(IEnumerable<Entity> companies, MetaData metaData)> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyCreateDto company);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
        IEnumerable<CompanyCreateDto> companyCollection);
    Task DeleteCompanyAsync(Guid id, bool trackChanges);
    Task UpdateCompanyAsync(Guid id, CompanyUpdateDto companyUpdateDto, bool trackChanges);
}