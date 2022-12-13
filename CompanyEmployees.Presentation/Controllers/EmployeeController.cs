﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
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

}