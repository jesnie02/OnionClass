using Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Rest.Extensions;

public static class JwtAuthenticationExtensions
{
    public static AuthenticationBuilder AddCustomJwtAuthentication(
        this IServiceCollection services, 
        IOptionsMonitor<AppOptions> optionsMonitor)
    {
        return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                
                // Use custom validator that works with JWT library
                options.UseSecurityTokenValidators = true;  // Enable the deprecated property
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(
                    new CustomJwtSecurityTokenValidator(optionsMonitor.CurrentValue.JwtSecret));
            });
    }
}