using System.Linq.Dynamic.Core;
using Entities.Models;
using Repository.Extensions.Utility;

namespace Repository.Extensions;

public static class CompanyRepositoryExtensions
{
    public static IQueryable<Company> Sort(this IQueryable<Company> companies, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString)) 
            return companies.OrderBy(x => x.Name);

        string orderQuery = OrderQueryBuilder.CreateOrderQuery<Company>(orderByQueryString);

        return orderQuery is null ? 
            companies.OrderBy(c => c.Name) : 
            companies.OrderBy(orderQuery);
    } 

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