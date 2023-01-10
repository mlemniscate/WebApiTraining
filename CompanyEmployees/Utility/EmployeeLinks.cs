using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Utility;

public class EmployeeLinks : IEmployeeLinks
{
    private readonly LinkGenerator linkGenerator;
    private readonly IDataShaper<EmployeeDto> dataShaper;

    public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
    {
        this.linkGenerator = linkGenerator;
        this.dataShaper = dataShaper;
    }


    public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
    {
        var shapedEmployees = ShapeData(employeesDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedEmployees(employeesDto.ToList(), fields, companyId,
                httpContext, shapedEmployees);

        return ReturnShapedEmployees(shapedEmployees);
    }

    private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields) =>
        dataShaper.ShapeData(employeesDto, fields)
            .Select(e => e.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
            StringComparison.InvariantCultureIgnoreCase);
    }

    private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) => new() { ShapedEntities = shapedEmployees };

    private LinkResponse ReturnLinkdedEmployees(List<EmployeeDto> employeesDto, string fields,
        Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
    {
        for (int i = 0; i < employeesDto.Count; i++)
        {
            var employeeLink = CreateLinksForEmployee(httpContext, companyId,
                employeesDto[i].Id, fields);
            shapedEmployees[i].Add("Links", employeeLink);
        }

        var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
        var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForEmployee(HttpContext httpContext,
        Guid companyId, Guid id, string fields)
    {
        var links = new List<Link>
        {
            new Link(linkGenerator.GetUriByAction(httpContext, "GetEmployee",
                values: new { companyId, id, fields }), "self", "GET"),
            new Link(
                linkGenerator.GetUriByAction(httpContext, "DeleteEmployee", values: new { companyId, id }),
                "delete_employee", "DELETE"),
            new Link(
                linkGenerator.GetUriByAction(httpContext, "UpdateEmployee", values: new { companyId, id }),
                "update_employee", "PUT"),
            new Link(
                linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployee", values: new { companyId, id }),
                "partially_update_employee", "PATCH")
        };

        return links;
    }

    private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext,
        LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(
            new Link(linkGenerator.GetUriByAction(httpContext, "GetEmployees", values: new { }),
                "self", "GET"));
        return employeesWrapper;
    }
}