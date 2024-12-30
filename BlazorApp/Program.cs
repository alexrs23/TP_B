using BlazorApp.Components;
using RCLAPI.Services;
using Microsoft.AspNetCore.Components.Web;
using RCLProdutos.Services.Interfaces; // Adiciona o namespace correto

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for dependency injection
builder.Services.AddScoped<HttpClient>(sp =>
    new HttpClient { BaseAddress = new Uri("https://xjjfwfbn-7260.uks1.devtunnels.ms") });

// Register IApiServices
builder.Services.AddScoped<IApiServices, ApiService>();

// Register ICardsUtilsServices
builder.Services.AddScoped<ICardsUtilsServices, RCLProdutos.Services.CardsUtilsServices>();

// Register ISliderUtilsServices
builder.Services.AddScoped<ISliderUtilsServices, RCLProdutos.Services.SliderUtilsServices>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(RCLAPI._Imports).Assembly);

app.Run();