using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;

namespace Entities.LinkModels;

public record LinkParameters<T> where T : RequestParameters
{
    public LinkParameters() { }
    public LinkParameters(T requestParameters, HttpContext httpContext)
    {
        RequestParameters = requestParameters;
        HttpContext = httpContext;
    }

    public T RequestParameters { get; init; }
    public HttpContext HttpContext { get; init; }
}