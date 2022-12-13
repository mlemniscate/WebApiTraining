using Entities.Models;

namespace Contracts.Repository;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company GetCompany(Guid id, bool trackChanges);
}