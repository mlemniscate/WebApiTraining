using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record EmployeeCreateDto(string? Name, int Age, string? Position) : 
    EmployeeManipulationDto(Name, Age, Position);