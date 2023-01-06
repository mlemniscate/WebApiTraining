﻿using Entities.Models;

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
}