using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
    {
        var companies = await FindAll(trackChanges)
            .Search(companyParameters.SearchTerm)
            .OrderBy(c => c.Name)
            .Skip((companyParameters.PageNumber - 1) * companyParameters.PageSize)
            .Take(companyParameters.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<Company>
            (companies,count, companyParameters.PageNumber, companyParameters.PageSize);
    }

    public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) =>
        await FindByCondition(c => c.Id == id, trackChanges).SingleOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

    public void DeleteCompany(Company company) => Delete(company);
}