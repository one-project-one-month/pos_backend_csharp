using DotNet8.Pos.App.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DotNet8.Pos.App.Components.Pages.Authentication;

public partial class Login
{
    private LoginResponseModel? responseModel;

    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default;

    // [SupplyParameterFromForm]
    // public Credentials UserCredentials { get; set; } = new Credentials();
    public async Task LoginUser()
    {
        responseModel = await HttpClientService.ExecuteAsync<LoginResponseModel>(
            $"{Endpoints.Login}",
            EnumHttpMethod.Post);
        if (responseModel == null) return;

            if (responseModel. == "MK" && UserCredentials.Password == "MK123")
        {
            var claims = new List<Claim>
        {
            new Claim(type: ClaimTypes.Name,UserCredentials.UserName)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            NavigationManager.NavigateTo("/");
        }
    }


