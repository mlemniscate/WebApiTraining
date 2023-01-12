using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Entities.Models;
using Repository.Extensions.Utility;

namespace Repository.Extensions;

public static class EmployeeRepositoryExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees,
        uint minAge, uint maxAge) =>
        employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees,
        string? searchTerm)
    {
        if(string.IsNullOrWhiteSpace(searchTerm)) return employees;

        return employees.Where(e => 
            e.Name.ToLower().Contains(searchTerm.ToLower()));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,
        string? orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString)) 
            return employees.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery) ? 
            employees.OrderBy(e => e.Name) : 
            employees.OrderBy(orderQuery);
    }
}