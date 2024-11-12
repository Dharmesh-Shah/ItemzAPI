using ItemzApp.WebUI.Client.Pages;
using ItemzApp.WebUI.Client.Services.Project;
using ItemzApp.WebUI.Client.Services.ItemzType;
using ItemzApp.WebUI.Client.Services.ItemzTypeItemzsService;
using ItemzApp.WebUI.Client.Services.Itemz;
using ItemzApp.WebUI.Client.Services.Hierarchy;
using ItemzApp.WebUI.Client.Services.ItemzCollection;
using ItemzApp.WebUI.Client.Services.ItemzChangeHistory;
using ItemzApp.WebUI.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ItemzApp.WebUI.Client.Services.ItemzTrace;
using ItemzApp.WebUI.Client.Services.BaselinesService;


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

builder.Services.AddHttpClient<IItemzTraceService, ItemzTraceService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IItemzCollectionService, ItemzCollectionService>(client =>
{
	client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IItemzChangeHistoryService, ItemzChangeHistoryService>(client =>
{
client.BaseAddress = new Uri("http://localhost:51087");
});

builder.Services.AddHttpClient<IBaselinesService, BaselinesService>(client =>
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
