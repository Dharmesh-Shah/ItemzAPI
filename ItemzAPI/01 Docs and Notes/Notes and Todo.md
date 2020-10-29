# Purpose

In this file we will include information about things to be considered while working on Itemz app. Team members are reading and learning on multiple sites and are picking up ideas for improvement. We shall use this file to make sure that we capture those ideas for considerations. 

### [Related data and serialization](https://docs.microsoft.com/en-us/ef/core/querying/related-data#related-data-and-serialization)

Because EF Core will automatically fix-up navigation properties, you can end up with cycles in your object graph. For example, loading a blog and its related posts will result in a blog object that references a collection of posts. Each of those posts will have a reference back to the blog.

 ```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...

    services.AddMvc()
        .AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = 
                       Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

    ...
}
```

Another alternative is to decorate one of the navigation properties with the `[JsonIgnore]` attribute, which instructs Json.NET to not traverse that navigation property while serializing.

> [!NOTE]  
> TODO: For every Entity that is used in Itemz App, decide if we wanted to utilize `[JsonIgnore]` attribute or not. 
> 
> Also considering `Newtonsoft.Json.ReferenceLoopHandling.Ignore` to be used in `ConfigureServices` method.

### [DONE: Repository to use Asynchronous DB Queries methods](https://docs.microsoft.com/en-us/ef/core/querying/async)

> DONE: Most of the DB calls are now Async apart from the one where we are returning PagedList due to custom implementation of OrderBy on top of `IQueryable`.

### [Building Expressions for Dynamic Queries in EF Core](https://docs.microsoft.com/en-us/ef/core/querying/how-query-works)

In this article, there is an important Warning message as per below.


> [!WARNING]  
> **Always validate user input:** While EF Core protects against SQL injection attacks by using parameters and escaping literals in queries, it does not validate inputs. Appropriate validation, per the application's requirements, should be performed before values from un-trusted sources are used in LINQ queries, assigned to entity properties, or passed to other EF Core APIs. This includes any user input used to dynamically construct queries. Even when using LINQ, if you are accepting user input to build expressions, you need to make sure that only intended expressions can be constructed.

> [!NOTE]  
> TODO: Check and Findout if there are recommended practices and options described by someone on the internet. This is a most common problem that many users of EF Core might have already solved and shared their experience with the community.
> 
> This should be part of the checklist while introducing Dynamic Queries via Expresson Tree.

### [Learn and Use Dumpsql()](https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#entity-deletion-examples)


> [!NOTE]  
> TODO: In the given example on the identified documentation page, notice that there is a method called as `DumpSQL()` that is used for generating SQL commands that are executed or going ot be executed.
> This will be a nice one to know more about and then use it in the Itemz App.


### [Utilizing EF Core internal mapper for updating Entity Properties](https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities#saving-single-entities)

Lets assume that an Entity has 50 properties on it and you are required to save that entity to the database after performing update to only say 3 properties.

In this case, instead of manually identifying and upating those 3 properties one can simply upate entire entity that is tracked in EF Core and internally EF Core will use some sort of mapper and identifier to update those 3 properties only.

Checkout following line of code which was picked up from an example found at above identified docs.

``` CSharp

context.Entry(existingBlog).CurrentValues.SetValues(blog);

```

Observe how `.CurrentValues.SetValues()` method is used in the example where `blog` objet is passed in as parameter.


### [In Future, We could consider using EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions)

This is a very good extension library available for EFCore and it's very promising. In case if we hit performance issues,
especially for bulk updating many itemz in a single operation then we could use `EFCore.BulkExtensions`

### [EF Core Tools and Extensions](https://docs.microsoft.com/en-us/ef/core/extensions/)

This page contains list of tools and extensions that are recommended by Microsoft. This page is part of EF Core documentation page.

### [Clean way to add Swagger into ASP.NET Core Application](https://www.talkingdotnet.com/clean-way-to-add-swagger-asp-net-core-application/)

This page shows very nice way to introduce services like Swagger in `ConfigureServices()` and `Configure()` methods within `Startup.cs` file.

> [!NOTE]  
> TODO: Let's implement this change in Itemz App and introduce a new extension methods that can help in keeping code clean within Itemz App.

### [EF.Property uses dynamic expression tree for identifying properties](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/advanced?view=aspnetcore-3.1#use-dynamic-linq-to-simplify-code)

While Dynamic and Expression Tree are very useful for application like Itemz App, it will be nice to learn and understand what Microsoft did as part of Entity Framework Core while developing feature like [EF.Property<TProperty>(Object, String) Method](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.ef.property?view=efcore-3.1)

> [!NOTE]  
> TODO: Check if we can utilize EF.Property for Sorting Order rathern then developing our own Dynamic method for it.

### [Logging Request and Response in ASP .NET Core](https://itnext.io/log-requests-and-responses-in-asp-net-core-3-a1bebd49c996)

This is a clean way to introduce Middleware for logging requst and responses. This could come very handy for debugging purposes.

Great article by <span style="background-color: #99ff66">Eric Anderson</Span>

> [!NOTE]
> TODO: it will be ideal to add such option in Itemz App so that we can help customers for identifying and resolving production issues.

### [Read the Docs](https://readthedocs.org/)

Opensource projects like Automapper publishes their docs to [https://docs.automapper.org/en/stable/Projection.html](https://docs.automapper.org/en/stable/Projection.html) They are using <span style="background-color: #99ff66">Read the Docs</span> for hosting their docs for lifelong.

It's nice to have Docs published along with code and auto populted via Read the Docs.

It could be useful for Itemz App to host docs like this.

> [!NOTE]
> TODO: Check-out if Read the Docs can be used for Itemz App.

### [dotnet counters monitor --process-id 31984 System.Runtime Microsoft.AspNetCore.Hosting](https://channel9.msdn.com/Shows/On-NET/ASPNET-Core-Series-Performance-Testing-Techniques)

After installing dotnet counters tool, one can monitor few matrix as described in the above video.

I was able to see whats happening with Itemz App via above command. 

Note that the `--process-id` is a different number everytime Itemz App is executed.

### [EF Core Explicitly compiled queries](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#explicitly-compiled-queries)

For queries that are quite high in demand and we expect it to be executed regularly by users then it would be ideal to create an explicitly compiled query to make sure that it does not create new query each time or perform look-up for cached query but instead just use the query that is <span style="background-color: #99ff66">explicitly compiled</span> for that purpose.


``` CSharp
// Create an explicitly compiled query
private static Func<CustomerContext, int, Customer> _customerById =
    EF.CompileQuery((CustomerContext db, int id) =>
        db.Customers
            .Include(c => c.Address)
            .Single(c => c.Id == id));

// Use the compiled query by invoking it
using (var db = new CustomerContext())
{
   var customer = _customerById(db, 147);
}
```

In above example ` _customerById` has will always be availabel and there isnot need for EF Core to perform cache look-up for the same.

### Rename ItemzContext to more meaningful name

Now that team has started expanding their entities and tables in the DB, it does not make good sense to continue calling DBContext as ItemzContext. We should think of some appropriate name that is generic so that every type of entity can use it in their concrete implementation of CRUD operations against the DB. 

### [EF Core User Defined Function in Linq Where Clause](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Entity-Framework-Core-In-Depth-Part-8)
In this Channel9 video, <span style="background-color: #99ff66">[Phil Japikse](https://twitter.com/skimedic) </span> explains how to effectively use SQL Server User Defined functions in ASP .NET Core application that utilizes Entity Framework Core.

Instead of calling [Raw SQL Queries](https://docs.microsoft.com/en-us/ef/core/querying/raw-sql), it will ideal to use the technique shared by Phil Japikse in his demo.

### [Exception Handling in Asynchronous Code is tricky](https://hamidmosalla.com/2018/06/19/exception-handling-in-asynchronous-code/)

Great article by <span style="background-color: #99ff66">[Hamid Mosalla](https://twitter.com/Xellarix) </span>
Soon in ItemzAPI, we are going to start using Async for Endpoints and DB Repository related activities. It will be ideal to make sure that we prepare well for handling errors via exception handling. 

Above article contains very nice details about exception handling via simple examples. 

Also note that if we throw our own custom errors then we have to use AggregateException and then check for InnerException for our custom errors.

For example, in the below code, we are checking if we received our custom exception.


 ```csharp

try
{
    // await (call) async function that is expected to throw MyOwnCustomExceptionException 
{

catch (Exception e)
when (((e as AggregateException)?.InnerException ?? e) is MyOwnCustomExceptionException duplicateException)
{
     // Now we know that it's MyOwnCustomExceptionException  and so handle it gracefully. 
}

 ```

### [Hierarchyid is not supported in EF Core 3.1](https://github.com/dotnet/efcore/issues/365)

This is going to be an issue. That said, there is an unofficial version that was published by EF Core Team member at ...[here](https://github.com/efcore/EFCore.SqlServer.HierarchyId)

checkout this Blog which has Hiearchy related example ... [Here...](https://www.thereformedprogrammer.net/ef-core-in-depth-what-happens-when-ef-core-reads-from-the-database/)

Instead of using old Parent / Child (self referencing PK and FK in the same table) way of handling hierarchy, it will be ideal to use hierarchyid as supported in SQL Server natively.

What is concerning is the fact that in above issue, one of the team member wrote... 

> It was contributed to EF6--we just clicked merge. 
> We probably would have done this on EF Core as part of the spatial 
> implementation if Microsoft.SqlServer.Types was 
> supported on .NET Core or even if there was a third-party l
> ibrary like NTS for hierarchyid. <span style="background-color: #99ff66"> Basically, it
>  doesn't seem prudent for our small team to implement, support, 
> and maintain a HierarchyId library for .NET right now, 
> so we're effectively blocked on implementing this <span>.

> [!NOTE - [02 Sep 2020]]
> I just found this new BLOG published by user <span style="background-color: #99ff66">[Dmitry Pavlov](https://twitter.com/dr_dimaka) </span> with subject [Tree Structure in EF Core: How to configure a self-referencing table and use it](https://habr.com/en/post/516596/).
> This blog has a working example of how to use self referencing column in the DB to show Hierarhcy data and how to configure the same.
> This post was also endorsed by <span style="background-color: #99ff66">[Jeremy Likness](https://twitter.com/jeremylikness) </span> who happens to be hosting EF Core Community meetings and working very closely with EF Core Development Team.
> We should consider trialing this approach as part of evaluating best possible way to handle Hierarchy data in ItemzApp.

> [!Note - [02 Sep 2020]]
> A separate blog that explains about using HierarchyID instead of self referencing Table. Check-it-out at ...
> [Using SQL Server HierarchyId with Entity Framework Core](https://www.meziantou.net/using-hierarchyid-with-entity-framework-core.htm)

### [Soft Delete well described for EF Core](https://www.thereformedprogrammer.net/ef-core-in-depth-soft-deleting-data-with-global-query-filters/)

Great Article by <span style="background-color: #99ff66">[Jon P Smith](https://twitter.com/thereformedprog) </span>

This is an in-depth article about soft deleting data while using EF Core. It covers many options as well as things to consider while designing Soft Delete.

> [!NOTE]
> TODO: In ItemZApp, it will be very useful to have SoftDelete option to make sure that we give opportunity for people to undo their action. 
> We have to think about how to implement the same while we have `Parking Lot` in place.

### [Consider MediatR Patter](https://www.youtube.com/watch?v=YzOBrVlthMk)

This YouTube Video by <span style="background-color: #99ff66">[Nick Chapsas](https://twitter.com/nickchapsas)</span> provides great introduction to MediatR pattern and CQRS Pattern.

We have to consider if we want to implement this pattern in ItemzApp. 

It's good to have separation of concerns and decoupling introduced in controllers and handlers / services but then it required to create many classes in the project.

It surely will help for better Unit Testing individual handlers / services as well as to keep code cleaner.

Please also consider learning curve for new joiners to the project. It's good on one hand that people who has access to this code will see real world example of how MediatR is implemented but then one has to also consider what will happen when contributors would like to send pull requests to the project where learning curve is a concern.

### [Fluent API better then Attributes and IEntityTypeConfiguration](https://dotnetcoretutorials.com/2020/06/27/a-cleaner-way-to-do-entity-configuration-with-ef-core/)

This Blog provides convincing argument that Fluent API is better then Attributes for configuring DTOs and Entities in ASP .NET Core Web API and Entity Framework Core application.

We should move away from Attribute in our ItemzAPP and consider using this clean way of configuring Entities via `IEntityTypeConfiguration` 

on 28th Sep 2020, I found this video on YouTube with title ["Migrations and Seed Data in Entity Framework Core](https://www.youtube.com/watch?v=5r_p8TiNX3Y). It has very good demo on how to use `IEntityTypeConfiguration<TEntity>` Interface. Also, there was this article written by Changhui Xu that explains how to configure Entity Types  in EF Core. Check it out at [here...](https://codeburst.io/ientitytypeconfiguration-t-in-entityframework-core-3fe7abc5ee7a)

### [What happens in ASP.NET Core 3.1 Requests explained very well](https://www.youtube.com/watch?v=0UZf_7c_EeE)

This YouTube Video by <span style="background-color: #99ff66">[Steve Gorden](https://twitter.com/stevejgordon)</span> provides great introduction to Anatomy of ASP.NET Core Requests. 

Very well explained about what happens to request when it goes from the client all the way to Controller + Action Method and back to the client. It covers topics like...

- TCP + TLS
- Kestrel
- HttpContext
- Middleware
- Endpoint Routing
- MVC Filter Pipleline
- Model Binding

Watching this video once is highly recommended for team members who will work on Itemz API.



### [ASP.NET Core Beyond the Basics](https://www.youtube.com/watch?v=6czpapfDu-c)

From Stockholm, Sweden, <span style="background-color: #99ff66">[Chris Klug](https://twitter.com/ZeroKoll) </span> presents following key concepts in NDC {Porto} 21-24 April 2020.

Checkout following table for topics that are covered as well as their timing in the video.

|Start Time| Desription  |
|--|--|
| 3:00  | Using inline Anonymous method for inserting inline middleware within Startup.cs file. Then he moves it into it's own file which is a better way to organize custom middleware in ASP .NET Core. |
| 14:25 | Great demo on how to implement custom headers and different formatters for output. Chris uses `XmlSerializerOutputFormatter` against custom accept headers which is very useful. Chris also uses Custom Attribute that inherits from Attribute and IActionConstraint to demonstrate how to decorate action method with your own attribute |
|34:45 |Good practices to introduce `ValidateAntiForgeryToken` in ASP .NET Core application |
|44:30 | How does ASP.NET Core introduces "Intellisense" in my application without adding any code in my application? This is well described by Chris and he demonstrates how to enable your own extensions that can be identified via dll files that are just placed in your server. |

### [Enum to String Converter for Web API](https://github.com/bmitchinson/Classroom/blob/master/backend/Data/EntityConfigurations/CourseEntityConfiguration.cs)

This Github repository has a nice example of how to use Enum for validating data that is posted via Web API against a specified property. 

```csharp
        public void Configure(EntityTypeBuilder<Course> entity)
        {
            entity.Property(_ => _.Level)
            .HasConversion(new EnumToStringConverter<TopicLevel>());

            entity.Property(_ => _.SoftDeleted)
            .HasDefaultValue(false);
        }
```

As you can see in the above code, it uses `EnumToStringConverter` as part of `HasConversion` for Level Property

> [!NOTE]
> TODO: Check if Web API validation for speicified values work as expected and then see if we want to use it in ItemzApp.

### [EF Core Community Meeting 19th Aug 2020](https://www.youtube.com/watch?time_continue=3733&v=W1sxepfIMRM&feature=emb_logo)

Great Demo by <span style="background-color: #99ff66"> [Arthur Vickers](https://twitter.com/ajcvickers)</span> where he covers following topics in his demo related to EF 5.0

 - Many to Many relationship
 - Indexer Properties e.g. private readonly Dictionary<string,object> _propertyValues = new Dictionary<string,object>();
 - SharedEntity Type e.g. public class NamedEntity{...} instead of public class Product{...}
 - Property Bags e.g. all entities in DBSets are registered with Dictionary<string,object>. Magic!!!

FUNNY: EF Team themselves says that preview version is tagged as 6.0.0... consider them as EF Core 5.0.

### [Schedule Background Jobs Using Hangfire in .NET Core]( https://codeburst.io/schedule-background-jobs-using-hangfire-in-net-core-2d98eb64b196)

Written By … <span style="background-color: #99ff66"> [Changhui Xu]( https://twitter.com/changhuixu)</span>

In this post Changhui talks about Hangfire which is an opensource utility that plugs in very nicely into ASP .NET Core application. It utilized Dependency Injection as well as `ConfigureServices (IServiceCollection services)` method in `Startup` class. Plus it can be configured in the ASP .Net Pipeline via `Configure(IApplicationBuilder app, IHostingEnvironment env)` method in `Startup` class.

There are few things to be considered. First, Hangfire utilizes notation for scheduling background task using CRON Expressions. It will be ideal to understand more in details about CRON Expression for which information can be found on Wikipedia at …  [CRON Expression]( https://en.wikipedia.org/wiki/Cron#CRON_expression)

Secondly, because Hangfire is utilizing Date and Time Scheduler for multiple regions, it will be ideal to understand how this has been done in this application. This will provide information about handling timezone issues in .NET Core application.

Thirdly, Hangfire itself might be very useful to be used in ItemzApp application as it would be ideal to kick start some tasks in the background which are event driven in the application. 






