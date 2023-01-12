using Entities.Models;

namespace Repository.Extensions;

public static class CompanyRepositoryExtensions
{
    public static IQueryable<Company> Search(this IQueryable<Company> companies, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return companies;

        var lowerSearchTerm = searchTerm.ToLower();
        return companies.Where(c => c.Name.ToLower().Contains(lowerSearchTerm) || 
                                           c.Address.ToLower().Contains(lowerSearchTerm) ||
                                           c.Country.ToLower().Contains(lowerSearchTerm) ||
                                           c.Employees.Any(e => e.Name.ToLower().Contains(lowerSearchTerm)));
    }
}