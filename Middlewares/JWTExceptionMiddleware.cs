using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares
{
    public class JwtExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items.TryGetValue(AuthenticationService.AuthErrorKey, out var error) && error is SecurityTokenException ex)
            {
                await HandleExceptionAsync(context, ex);
                return;
            }

            await next(context);
        }

        private static Task HandleExceptionAsync(HttpContext context, SecurityTokenException exception)
        {

            // Only handle if path requires auth
            if (context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.CompletedTask;
            }
            var problem = Results.Problem(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Authentication Failed",
            detail: exception.Message
        );

            return problem.ExecuteAsync(context);
        }
    }
}