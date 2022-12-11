using Contracts.Repository;
using Entities.Models;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context) : base(context)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
     FindAll(trackChanges)
    .OrderBy(c => c.Name)
    .ToList();
}