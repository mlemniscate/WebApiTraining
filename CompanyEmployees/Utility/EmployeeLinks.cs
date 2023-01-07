using Contracts;
using Entities.LinkModels;
using Entities.Models;
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
}