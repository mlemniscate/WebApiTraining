using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract class EmployeeManipulationDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; set; }

    [Range(18, int.MaxValue, ErrorMessage = "Age is a required and it can't be lower then 18.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
    public string? Position { get; set; }
}   