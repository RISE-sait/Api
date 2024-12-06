using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Api.helpers
{
    public static class Auth
    {
        public static bool IsUserInRole(this ControllerBase controller, params string[] allowedRoles)
        {
            var userRole = controller.User.Claims.FirstOrDefault(c => c.Type == "staffTypeName")?.Value;
            var response = controller.Response;

            if (userRole == null)
            {
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.Headers.Append("X-Error-Message", "Staff type name is missing in claims.");
                return false;
            }

            if (!allowedRoles.Contains(userRole))
            {
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.Headers.Append("X-Error-Message", $"You must be an {string.Join(" or ", allowedRoles)} to access this.");
                return false;
            }

            return true;
        }
    }
}