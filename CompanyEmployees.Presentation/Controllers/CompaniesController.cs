using System.Runtime.InteropServices;
using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Exceptions;
using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;


namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager service;

    public CompaniesController(IServiceManager service) => this.service = service;

    [HttpGet]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
    {
        var linkParams = new LinkParameters<CompanyParameters>(companyParameters, HttpContext);

        var result =
                await service.CompanyService.GetAllCompaniesAsync(linkParams, trackChanges: true);

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(result.metaData));

        return result.links.HasLinks ?
                Ok(result.links.LinkedEntities) :
                Ok(result.links.ShapedEntities);
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto company)
    {
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyUpdateDto company)
    {
        await service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }

}