using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;

namespace poczta;

public sealed class BasicAuthMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IUserService userService)
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers.Authorization!);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];
            context.Items["User"] = await userService.Authenticate(username, password);
        }
        catch
        {
            throw new AuthenticationException("Unauthorized access");
        }

        await next(context);
    }
}