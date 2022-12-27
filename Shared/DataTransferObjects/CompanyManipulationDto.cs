using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class CompanyManipulationDto
{
    [Required(ErrorMessage = "Name field is required.")]
    [MaxLength(30, ErrorMessage = "Name max length is 30 character.")]
    public string Nam { get; set; }

    [Required(ErrorMessage = "Address field is required.")]
    [MaxLength(300, ErrorMessage = "Address max length is 300 character.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Country field is required.")]
    [MaxLength(50, ErrorMessage = "Country max length is 50 character.")]
    public string Country { get; set; }

    public IEnumerable<EmployeeCreateDto> Employees { get; set; }
}