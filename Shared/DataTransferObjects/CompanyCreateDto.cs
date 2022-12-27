using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record CompanyCreateDto(string Name, string Address, string Country, IEnumerable<EmployeeCreateDto> Employees) : 
    CompanyManipulationDto(Name, Address, Country, Employees);