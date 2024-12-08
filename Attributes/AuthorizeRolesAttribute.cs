using System.Security.Claims;
using Api.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes
{

    /// <summary>
    /// Attribute to authorize roles for API endpoints.
    /// Allows access only to users whose roles match the specified roles in the attribute.
    /// The `SuperAdmin` role is authorized by default for all endpoints.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeRolesAttribute(string[] roles) : Attribute, IAuthorizationFilter
    {
        private string[] Roles { get; } = roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if the [AllowAnonymous] attribute is present
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                return;
            
            var user = context.HttpContext.User;
            var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            
            if (userRole == StaffType.SuperAdmin)
                return; // SuperAdmin is authorized for everything
            
            var allRoles = context.ActionDescriptor.EndpointMetadata
                 .OfType<AuthorizeRolesAttribute>()
                 .SelectMany(attr => attr.Roles)
                 .Distinct()
                 .ToArray();

            if (userRole != null && allRoles.Contains(userRole)) return;
            
            // Respond with Forbidden if the user role doesn't match
            context.Result = new ForbidResult();
            context.HttpContext.Response.Headers.Append("X-Error-Message", $"You must be an {string.Join(" or ", allRoles)} to access this.");
        }
    }
}