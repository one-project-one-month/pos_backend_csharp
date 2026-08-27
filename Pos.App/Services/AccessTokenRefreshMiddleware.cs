using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pos.App.Services;

public sealed class AccessTokenRefreshMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, BackendApiClient api)
    {
        if (ShouldRefresh(context))
        {
            var token = await api.GetValidAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (HttpMethods.IsGet(context.Request.Method))
                {
                    var returnUrl = context.Request.PathBase + context.Request.Path + context.Request.QueryString;
                    context.Response.Redirect($"/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
                    return;
                }
            }
        }

        await next(context);
    }

    private static bool ShouldRefresh(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated != true)
            return false;
        var path = context.Request.Path;
        return !path.StartsWithSegments("/login") &&
               !path.StartsWithSegments("/_framework") &&
               !path.StartsWithSegments("/css") &&
               !path.StartsWithSegments("/favicon.png");
    }
}
