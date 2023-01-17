using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers;

[ApiVersion("2.0", Deprecated = true)]
[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class CompaniesV2Controller : ControllerBase
{
    private readonly IServiceManager service;

    public CompaniesV2Controller(IServiceManager service)
    {
        this.service = service;
    }

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
}