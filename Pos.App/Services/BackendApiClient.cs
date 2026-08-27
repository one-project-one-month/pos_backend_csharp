using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Pos.BackendApi.Models.Setup.Login;

namespace Pos.App.Services;

public sealed class BackendApiClient(
    IHttpClientFactory clientFactory,
    IHttpContextAccessor contextAccessor)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<HttpResponseMessage> GetAsync(string uri) => SendAsync(HttpMethod.Get, uri);
    public Task<HttpResponseMessage> DeleteAsync(string uri) => SendAsync(HttpMethod.Delete, uri);
    public Task<HttpResponseMessage> PostAsync<T>(string uri, T value) => SendAsync(HttpMethod.Post, uri, value);
    public Task<HttpResponseMessage> PatchAsync<T>(string uri, T value) => SendAsync(HttpMethod.Patch, uri, value);

    public Task<string?> GetValidAccessTokenAsync() =>
        GetValidAccessTokenAsync(clientFactory.CreateClient("BackendApi"));

    public async Task<T?> GetFromJsonAsync<T>(string uri)
    {
        using var response = await GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return default;
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<JsonDocument?> GetDocumentAsync(string uri)
    {
        using var response = await GetAsync(uri);
        if (!response.IsSuccessStatusCode)
            return null;
        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }

    public async Task<ApiCommandResult> ReadCommandResultAsync(HttpResponseMessage response)
    {
        using (response)
        {
            var body = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
                return new(response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Saved." : "Request failed.");
            try
            {
                using var json = JsonDocument.Parse(body);
                var root = json.RootElement;
                var success = response.IsSuccessStatusCode &&
                    (!root.TryGetProperty("isSuccess", out var flag) || flag.GetBoolean());
                var message = root.TryGetProperty("message", out var text) ? text.GetString() :
                    root.TryGetProperty("detail", out var detail) ? detail.GetString() : null;
                return new(success, message ?? (success ? "Saved." : "Request failed."));
            }
            catch (JsonException)
            {
                return new(response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Saved." : "Request failed.");
            }
        }
    }

    private async Task<HttpResponseMessage> SendAsync<T>(HttpMethod method, string uri, T? value = default)
    {
        var client = clientFactory.CreateClient("BackendApi");
        var token = await GetValidAccessTokenAsync(client);
        using var request = new HttpRequestMessage(method, uri);
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (value is not null)
            request.Content = JsonContent.Create(value, options: JsonOptions);
        return await client.SendAsync(request);
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string uri) =>
        SendAsync<object?>(method, uri, null);

    private async Task<string?> GetValidAccessTokenAsync(HttpClient client)
    {
        var context = contextAccessor.HttpContext;
        if (context is null)
            return null;

        var auth = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!auth.Succeeded || auth.Properties is null)
            return null;

        var accessToken = auth.Properties.GetTokenValue("access_token");
        var expiresText = auth.Properties.GetTokenValue("expires_at");
        if (DateTimeOffset.TryParse(expiresText, out var expiresAt) &&
            expiresAt > DateTimeOffset.UtcNow.AddMinutes(2))
            return accessToken;

        var refreshToken = auth.Properties.GetTokenValue("refresh_token");
        if (string.IsNullOrWhiteSpace(refreshToken))
            return accessToken;

        using var refreshResponse = await client.PostAsJsonAsync(
            "api/v1/auth/refresh",
            new RefreshTokenRequestModel { RefreshToken = refreshToken },
            JsonOptions);
        if (!refreshResponse.IsSuccessStatusCode)
            return null;

        var pair = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseModel>(JsonOptions);
        if (pair?.Message.IsSuccess != true)
            return null;

        auth.Properties.StoreTokens(
        [
            new AuthenticationToken { Name = "access_token", Value = pair.AccessToken },
            new AuthenticationToken { Name = "refresh_token", Value = pair.RefreshToken },
            new AuthenticationToken { Name = "expires_at", Value = pair.AccessTokenExpiresAtUtc.ToString("O") },
            new AuthenticationToken { Name = "refresh_expires_at", Value = pair.RefreshTokenExpiresAtUtc.ToString("O") },
        ]);
        auth.Properties.ExpiresUtc = pair.RefreshTokenExpiresAtUtc;
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            auth.Principal!,
            auth.Properties);
        return pair.AccessToken;
    }
}

public readonly record struct ApiCommandResult(bool IsSuccess, string Message);
