using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;

    // You can use this for checking that roles exists in database or not with RoleExitsAsync method
    // private readonly RoleManager<IdentityRole> roleManager;

    public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.userManager = userManager;
        this.configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = mapper.Map<User>(userForRegistration);

        var result = await userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
            await userManager.AddToRolesAsync(user, userForRegistration.Roles);

        return result;
    }
}