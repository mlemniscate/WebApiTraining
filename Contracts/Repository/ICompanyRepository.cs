using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts.Repository;

public interface ICompanyRepository
{
    Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);
    Task<Company> GetCompanyAsync(Guid id, bool trackChanges);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteCompany(Company company);
}