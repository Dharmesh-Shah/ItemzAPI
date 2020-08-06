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



