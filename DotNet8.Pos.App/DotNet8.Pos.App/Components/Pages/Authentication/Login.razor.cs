using DotNet8.Pos.App.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;

namespace DotNet8.Pos.App.Components.Pages.Authentication;

public partial class Login
{
    private LoginRequestModel _loginRequest = new LoginRequestModel();
    public async Task LoginUser()
    {
        var responseModel = await HttpClientService.ExecuteAsync<LoginResponseModel>(
            $"{Endpoints.Login}",
            EnumHttpMethod.Post,
            _loginRequest);

        if(responseModel.Message.IsSuccess == false && responseModel.Message.IsError == true && responseModel.token == null)
        {
            NavigationManager.NavigateTo("/login");
        }      

        if(responseModel.Message.IsSuccess == true && responseModel.Message.IsError == false && responseModel.token != null)
        {
            NavigationManager.NavigateTo("/");
        }

    }
}