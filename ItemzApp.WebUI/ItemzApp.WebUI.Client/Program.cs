using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddScoped<IProjectService, ProjectService>();
//builder.Services.AddHttpClient<IProjectService, ProjectService>(client =>
//{
//    //client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//	client.BaseAddress = new Uri("http://localhost:51087");
//});

await builder.Build().RunAsync();
