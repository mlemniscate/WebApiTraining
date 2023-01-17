using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceManager service;

    public TokenController(IServiceManager service)
    {
        this.service = service;
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await service.AuthenticationService
            .RefreshToken(tokenDto);

        return Ok(tokenDtoToReturn);
    }

}