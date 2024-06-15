using DotNet8.Pos.App.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;

namespace DotNet8.Pos.App.Components.Pages.Authentication;

public partial class Login
{
    private LoginRequestModel? requestModel;

    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default;

    [Parameter] public LoginRequestModel townshipListResponseModel { get; set; } = new();


    [SupplyParameterFromForm]
    public LoginRequestModel Input { get; set; } = new LoginRequestModel { UserName = string.Empty, Password = string.Empty };

    // [SupplyParameterFromForm]
    // public Credentials UserCredentials { get; set; } = new Credentials();
    public async Task LoginUser()
    {
        var responseModel = await HttpClientService.ExecuteAsync<LoginRequestModel>(
            $"{Endpoints.Login}",
            EnumHttpMethod.Post);

        //if (responseModel == null) return;

        //if (responseModel.UserName == requestModel.UserName && UserCredentials.Password == "MK123")
        //{
        //    var claims = new List<Claim>
        //{
        //    new Claim(type: ClaimTypes.Name,UserCredentials.UserName)
        //};

        //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //var principal = new ClaimsPrincipal(identity);
        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        NavigationManager.NavigateTo("/");

    }
}