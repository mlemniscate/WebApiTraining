using CompanyEmployees.Presentation.ActionFilters;
using Entities.Exceptions;
using Entities.LinkModels;
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
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetEmployees(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
    {
        var linkParams = new LinkParameters<EmployeeParameters>(employeeParameters, HttpContext);

        var result = await service.EmployeeService.GetEmployeesAsync(companyId,
            linkParams, trackChanges: false);

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(result.metaData));

        return result.links.HasLinks
            ? Ok(result.links.LinkedEntities)
            : Ok(result.links.ShapedEntities);
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
    public async Task<IActionResult> PartiallyUpdateEmployee(Guid companyId, Guid id,
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