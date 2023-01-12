using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly LinkGenerator linkGenerator;

    public RootController(LinkGenerator linkGenerator)
    {
        this.linkGenerator = linkGenerator;
    }

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/vnd.lemniscate.apiroot"))
        {
            var list = new List<Link>
            {
                new Link
                {
                    Href = linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new { }),
                    Rel = "self",
                    Method = "GET",
                },
                new Link
                {
                    Href = linkGenerator.GetUriByName(HttpContext, "GetCompanies", new {}),
                    Rel = "companies",
                    Method = "GET",
                },
                new Link
                {
                    Href = linkGenerator.GetUriByName(HttpContext, "CreateCompany", new {}),
                    Rel = "create_company",
                    Method = "Post",
                }
            };

            return Ok(list);
        }

        return NoContent();
    }
}