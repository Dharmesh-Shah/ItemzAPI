# Purpose

In this file we will include details about different options to be considered while debugging issues in different environments. While learning about different techniques around ASP .NET Core, we come across many good ideas that one should consider while supporting customers who might be facing issues that we have not encountered. We hope that this tips and tricks will prove useful for this project as well as for others in the community.


### [ASP .NET CORE - Display environment variables](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#display-environment-variables)

ASP .NET CORE picks up configuration settings from many different sources. Current version of ASP .NET Core is 5.0 at the time of writing this notes and we see following possibilities for configuring options in it. 

CreateDefaultBuilder provides default configuration for the app in the following order:

- ChainedConfigurationProvider : Adds an existing `IConfiguration` as a source. In the default configuration case, adds the host configuration and setting it as the first source for the app configuration.
- appsettings.json using the JSON configuration provider.
- appsettings.`Environment`.json using the JSON configuration provider. For example, appsettings.Production.json and appsettings.Development.json.
- App secrets when the app runs in the `Development` environment.
- Environment variables using the Environment Variables configuration provider.
- Command-line arguments using the Command-line configuration provider.

Configuration options (key value pairs) are read in the order in which they are listed above.

Now to print all the configuraiton / environemnt information at start-up, one could use following code which is handy for debugging purposes.

 ```csharp
public static void Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();

    var config = host.Services.GetRequiredService<IConfiguration>();

    foreach (var c in config.AsEnumerable())
    {
        Console.WriteLine(c.Key + " = " + c.Value);
    }
    host.Run();
}
```

### [Serilog - Include details about SourceContext and Full Path to the executing method](https://stackoverflow.com/questions/29470863/serilog-output-enrich-all-messages-with-methodname-from-which-log-entry-was-ca)

In above stackoverflow.com response user Jaya B. has responded with few options that could be useful to be included in the output format for Serilog.

Like, one could modify Serilog outputTemplate in `appsettings.json` file as per below by including `Properties`

``` JSON
   "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{Properties}] {Message:lj}{NewLine}{Exception}"
```

This will generate output that include many details like ...

``` TEXT
2021-05-21 18:14:06.315 +05:30 [Debug] [{ SourceContext: "ItemzApp.API.Controllers.ItemzsController", ActionId: "ef7bfb35-8511-47c9-959b-ac870b5a4e90", ActionName: "ItemzApp.API.Controllers.ItemzsController.GetItemzAsync (ItemzApp.API)", RequestId: "0HM8SAOF4N00D:00000005", RequestPath: "/api/Itemzs/9153a516-d69e-4364-b17e-03b87442e21c", ConnectionId: "0HM8SAOF4N00D" }] ::Itemzs-GetItemz:: Processing request to get Itemz for ID "9153a516-d69e-4364-b17e-03b87442e21c"


Notice that including properties added following extra values in the output file.

SourceContext: "ItemzApp.API.Controllers.ItemzsController"
ActionId: "ef7bfb35-8511-47c9-959b-ac870b5a4e90", 
ActionName: "ItemzApp.API.Controllers.ItemzsController.GetItemzAsync (ItemzApp.API)", 
RequestId: "0HM8SAOF4N00D:00000005",
RequestPath: "/api/Itemzs/9153a516-d69e-4364-b17e-03b87442e21c", 
ConnectionId: "0HM8SAOF4N00D" 

```

If you just want to include `ActionName` (called method name with full path including namespace) in the Serilog output file then it can be achieved by setting `outputTemplate`  value to ...


``` JSON
   "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{ActionName}] {Message:lj}{NewLine}{Exception}"
```

This will generate output log as per below...

``` TEXT
2021-05-21 18:18:51.837 +05:30 [Debug] [ItemzApp.API.Controllers.ItemzsController.GetItemzAsync (ItemzApp.API)] ::Itemzs-GetItemz:: Processing request to get Itemz for ID "9153a516-d69e-4364-b17e-03b87442e21c"

Notice that it included following details related to the ActionName just after [Debug] level

[ItemzApp.API.Controllers.ItemzsController.GetItemzAsync (ItemzApp.API)]

```

Such option to include extra details in the Serilog output via structured logging will prove to be handy for debugging issues.

