using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager service;

    public EmployeeController(IServiceManager service) => this.service = service;

    [HttpGet]
    public async Task<IActionResult> GetEmployees(Guid companyId)
    {
        var employees = await service.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetCompanyEmployee")]
    public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
    {
        var employee = await service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto employee, Guid companyId)
    {
        if (employee is null)
            return BadRequest("EmployeeCreateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var employeeDto = await service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
        return CreatedAtRoute("GetCompanyEmployee", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id,
        [FromBody] EmployeeUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

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
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("object patchDoc send from client is null");

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