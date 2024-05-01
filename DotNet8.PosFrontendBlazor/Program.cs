using DotNet8.PosFrontendBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7164") });
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddMudServices();

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<InjectService>();

await builder.Build().RunAsync();
