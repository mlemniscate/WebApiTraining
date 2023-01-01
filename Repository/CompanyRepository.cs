using Contracts.Repository;
using Entities.Models;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
     FindAll(trackChanges)
    .OrderBy(c => c.Name)
    .ToList();

    public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) =>
        FindByCondition(c => c.Id == id, trackChanges).SingleOrDefault();

    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

    public void DeleteCompany(Company company) => Delete(company);
}