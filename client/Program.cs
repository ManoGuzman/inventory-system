using client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add MudBlazor services
builder.Services.AddMudServices();

// Configure HttpClient for API communication
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5184/") // API base URL
});

// Add application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

await builder.Build().RunAsync();
