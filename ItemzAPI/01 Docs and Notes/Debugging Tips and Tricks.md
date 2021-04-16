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

