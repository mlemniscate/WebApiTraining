using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
     await FindAll(trackChanges)
    .OrderBy(c => c.Name)
    .ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) =>
        await FindByCondition(c => c.Id == id, trackChanges).SingleOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

    public void DeleteCompany(Company company) => Delete(company);
}