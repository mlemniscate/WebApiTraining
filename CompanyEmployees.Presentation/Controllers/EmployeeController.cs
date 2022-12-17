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

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid id)
    {
        service.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);
        return NoContent();
    }

}