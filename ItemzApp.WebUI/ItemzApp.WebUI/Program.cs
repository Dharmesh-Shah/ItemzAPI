using ItemzApp.WebUI.Client.Pages;
using ItemzApp.WebUI.Client.Services.Project;
using ItemzApp.WebUI.Client.Services.ItemzType;
using ItemzApp.WebUI.Client.Services.ItemzTypeItemzsService;
using ItemzApp.WebUI.Client.Services.Itemz;
using ItemzApp.WebUI.Client.Services.Hierarchy;
using ItemzApp.WebUI.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient<IProjectService, ProjectService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IItemzTypeService, ItemzTypeService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IItemzTypeItemzsService, ItemzTypeItemzsService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IItemzService, ItemzService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IHierarchyService, HierarchyService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});


builder.Services.AddMudServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ItemzApp.WebUI.Client._Imports).Assembly);

app.Run();