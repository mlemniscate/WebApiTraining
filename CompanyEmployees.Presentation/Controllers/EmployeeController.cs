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
    public IActionResult GetEmployees(Guid companyId)
    {
        var employees = service.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetCompanyEmployee")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeCreateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeCreateDto object is null");

        var employeeDto = service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);
        return CreatedAtRoute("GetCompanyEmployee", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployee(Guid companyId, Guid id,
        [FromBody] EmployeeUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeUpdateDto object is null");

        service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, 
            compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid id)
    {
        service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("object patchDoc send from client is null");

        var result = service.EmployeeService.GetEmployeeForPatch(companyId, id,
            compTrackChanges: false,
            empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch);
        service.EmployeeService.SaveChangesForPatch(
            result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }

}