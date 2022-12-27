using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record EmployeeCreateDto
(
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    string? Name,

    [Required(ErrorMessage = "Age is a required field.")]
    int Age,

    [Required(ErrorMessage = "Position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
    string? Position
);