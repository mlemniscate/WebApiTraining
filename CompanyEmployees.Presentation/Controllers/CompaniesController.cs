﻿using System.Runtime.InteropServices;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;


namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager service;

    public CompaniesController(IServiceManager service) => this.service = service;

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies =
                await service.CompanyService.GetAllCompaniesAsync(trackChanges: true);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = 
        typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await service.CompanyService.GetByIdsAsync(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto company)
    {
        if (company is null)
            return BadRequest("CompanyCreateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdCompany = await service.CompanyService.CreateCompanyAsync(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyCreateDto> companyCollection)
    {
        var result = 
            await service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyUpdateDto company)
    {
        if (company is null)
            return BadRequest("CompanyUpdateDto object is null.");

        await service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }

    

}