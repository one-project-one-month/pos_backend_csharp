using System.Security.Claims;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Pos.BackendApi.Models.Setup.Login;

namespace Pos.App.Services;

public sealed class AuthSessionService(
    IHttpClientFactory clientFactory,
    IHttpContextAccessor contextAccessor)
{
    public async Task<ApiCommandResult> SignInAsync(string userName, string password)
    {
        var client = clientFactory.CreateClient("BackendApi");
        HttpResponseMessage response;
        try
        {
            response = await client.PostAsJsonAsync("api/v1/auth/login", new LoginRequestModel
            {
                UserName = userName,
                Password = password,
            });
        }
        catch (HttpRequestException)
        {
            return new(false, "The sign-in service is currently unavailable.");
        }

        using (response)
        {
            var model = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            if (!response.IsSuccessStatusCode || model?.Message.IsSuccess != true || model.Staff is null)
                return new(false, model?.Message.Message ?? "Username or password is incorrect.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, model.Staff.StaffId.ToString()),
                new(ClaimTypes.Name, model.Staff.StaffName),
                new("StaffId", model.Staff.StaffId.ToString()),
                new("StaffCode", model.Staff.StaffCode),
                new("StaffName", model.Staff.StaffName),
                new(ClaimTypes.Role, model.Staff.Position ?? string.Empty),
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = model.RefreshTokenExpiresAtUtc,
            };
            properties.StoreTokens(
            [
                new AuthenticationToken { Name = "access_token", Value = model.AccessToken },
                new AuthenticationToken { Name = "refresh_token", Value = model.RefreshToken },
                new AuthenticationToken { Name = "expires_at", Value = model.AccessTokenExpiresAtUtc.ToString("O") },
                new AuthenticationToken { Name = "refresh_expires_at", Value = model.RefreshTokenExpiresAtUtc.ToString("O") },
            ]);

            var context = contextAccessor.HttpContext
                ?? throw new InvalidOperationException("An active HTTP context is required.");
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
            return new(true, "Welcome back.");
        }
    }

    public async Task SignOutAsync()
    {
        var context = contextAccessor.HttpContext
            ?? throw new InvalidOperationException("An active HTTP context is required.");
        var refreshToken = await context.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, "refresh_token");
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            var client = clientFactory.CreateClient("BackendApi");
            try
            {
                using var _ = await client.PostAsJsonAsync("api/v1/auth/revoke",
                    new RevokeTokenRequestModel { RefreshToken = refreshToken });
            }
            catch (HttpRequestException)
            {
                // Local sign-out must still complete when the API is unavailable.
            }
        }
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
