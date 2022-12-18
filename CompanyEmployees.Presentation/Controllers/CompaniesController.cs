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
    public IActionResult GetCompanies()
    {
        var companies =
                service.CompanyService.GetAllCompanies(trackChanges: true);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(BinderType = 
        typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = service.CompanyService.GetByIds(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyCreateDto company)
    {
        if (company is null)
            return BadRequest("CompanyCreateDto object is null");

        var createdCompany = service.CompanyService.CreateCompany(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyCreateDto> companyCollection)
    {
        var result = 
            service.CompanyService.CreateCompanyCollection(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateCompany(Guid id, CompanyUpdateDto company)
    {
        if (company is null)
            return BadRequest("CompanyUpdateDto object is null.");

        service.CompanyService.UpdateCompany(id, company, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCompany(Guid id)
    {
        service.CompanyService.DeleteCompany(id, trackChanges: false);

        return NoContent();
    }

    

}