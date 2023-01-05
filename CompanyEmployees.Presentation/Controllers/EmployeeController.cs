using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager service;

    public EmployeeController(IServiceManager service) => this.service = service;

    [HttpGet]
    public async Task<IActionResult> GetEmployees(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await service.EmployeeService.GetEmployeesAsync(companyId,
            employeeParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.employees);
    }

    [HttpGet("{id:guid}", Name = "GetCompanyEmployee")]
    public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
    {
        var employee = await service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto employee, Guid companyId)
    {
        var employeeDto = await service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
        return CreatedAtRoute("GetCompanyEmployee", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id,
        [FromBody] EmployeeUpdateDto employee)
    {
        await service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, 
            compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
    {
        await service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, false);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeUpdateDto> patchDoc)
    {
        var result = await service.EmployeeService.GetEmployeeForPatchAsync(companyId, id,
            compTrackChanges: false,
            empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await service.EmployeeService.SaveChangesForPatchAsync(
            result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}