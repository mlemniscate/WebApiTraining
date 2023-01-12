using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;

namespace Contracts;

public interface ICompanyLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<CompanyDto> companiesDto,
        string fields, HttpContext httpContext);
}