using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record CompanyManipulationDto(
        [Required(ErrorMessage = "Name field is required.")]
        [MaxLength(30, ErrorMessage = "Name max length is 30 character.")]
        string Name,
        [Required(ErrorMessage = "Address field is required.")]
        [MaxLength(300, ErrorMessage = "Address max length is 300 character.")]
        string Address,
        [Required(ErrorMessage = "Country field is required.")]
        [MaxLength(50, ErrorMessage = "Country max length is 50 character.")]
        string Country,

        IEnumerable<EmployeeCreateDto> Employees);