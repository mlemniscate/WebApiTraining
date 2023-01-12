using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Utility;

public class CompanyLinks : ICompanyLinks
{
    private readonly LinkGenerator linkGenerator;
    private readonly IDataShaper<CompanyDto> dataShaper;

    public CompanyLinks(LinkGenerator linkGenerator, IDataShaper<CompanyDto> dataShaper)
    {
        this.linkGenerator = linkGenerator;
        this.dataShaper = dataShaper;
    }


    public LinkResponse TryGenerateLinks(IEnumerable<CompanyDto> companiesDto, string fields, HttpContext httpContext)
    {
        var shapedCompanies = ShapeData(companiesDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedEmployees(companiesDto.ToList(), fields,
                httpContext, shapedCompanies);

        return ReturnShapedEmployees(shapedCompanies);
    }

    private List<Entity> ShapeData(IEnumerable<CompanyDto> companiesDto, string fields) =>
        dataShaper.ShapeData(companiesDto, fields)
            .Select(e => e.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
            StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) => new() { ShapedEntities = shapedEmployees };

    private LinkResponse ReturnLinkdedEmployees(List<CompanyDto> companiesDto, string fields,
        HttpContext httpContext, List<Entity> shapedEmployees)
    {
        for (int i = 0; i < companiesDto.Count; i++)
        {
            var employeeLink = CreateLinksForEmployee(httpContext,
                companiesDto[i].Id, fields);
            shapedEmployees[i].Add("Links", employeeLink);
        }

        var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
        var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid id, string fields)
    {
        var links = new List<Link>
        {
            new Link(linkGenerator.GetUriByAction(httpContext, "GetCompany",
                values: new { id, fields }), "self", "GET"),
            new Link(
                linkGenerator.GetUriByAction(httpContext, "DeleteCompany", values: new { id }),
                "delete_employee", "DELETE"),
            new Link(
                linkGenerator.GetUriByAction(httpContext, "UpdateCompany", values: new { id }),
                "update_employee", "PUT"),
            //TODO: You can create partially update for company with patch method
            // new Link(
            //     linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateCompany", values: new { id }),
            //     "partially_update_employee", "PATCH")
        };

        return links;
    }

    private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext,
        LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(
            new Link(linkGenerator.GetUriByAction(httpContext, "GetCompanies", values: new { }),
                "self", "GET"));
        return employeesWrapper;
    }
}