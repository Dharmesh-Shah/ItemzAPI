
# Design Decision of Baselining

Broadly speaking there are two options for creating Baseline. First, write Baseline Creation Logic in the Web API Application itself. Second, write a store procedure in SQL Server that has Baseline Creating Logic implemented directly in the Database.

Let’s have a look at cons and pros for both these options.

In the First option where we keep baseline creation logic within Web API Application.

### Pros:

-	Abstraction. Here the advantage would be that without depending on the Database Code, one could implement branch creation process at the Web API App level. This means tomorrow if we need to replace SQL Server with some other datastore then actual code of creating baseline shall work nicely with minimal effort to make it compatible and work with different datastore. 
-	Support for Custom Attributes. In the future, we have plans to implement custom attributes on requirements records. Users shall be allowed to introduce their own custom fields / attributes on requirements type / project / repository level. In such a case, where user defined attributes are present in requirements then the baseline that we capture shall include details about such custom fields. This will be easier to handle in Web API Application rather then directly in the database.
-	Small and incremental changes to the actual logic of Baseline creation shall be easily performed via incremental releases of Web API Apps. Not every change would touch database schema / stored procedure code. This shall enable smooth upgrade process for customers and for the development team as well.
-	It becomes possible to write Unit Tests for complex logics that are introduced in Web API App for managing Baseline features. 

### Cons:

-	Performance. Everytime baseline creation process has to bring in all the data from the database all the way to Web API App and then process it and send back insert / update commands back to the database. This could slow down the entire process.
-	Complexity. Code in C# could be lot more complex compared to a Stored Procedure code that we store in SQL Server directly.
-	Concurrency. As we need to process every record in a given Project / Requirement Type from Web API App, it could lead to concurrency issues as there are high probability that underlying data in the project might be changed by someone else during baseline management process.
-	In future, it might become harder to support clustering features for Web API App as baselining could be a long running task that will touch several records. This could be much better if we allow management of baselines directly in Database via Stored Procedures.
-	Overall Baseline Operation Transaction Management could become difficult as multiple commands will be executed against the database. 

Mainly the Pros from the first option above could potentially become Cons for the second option and vice versa.

# Conclusion
Given that we are in the initial phase of application development for Itemz API, our current decision is to write baseline logic directly in the Stored Procedure and get the feature rolled in. This will require us to work with empty migration in EF Core in which we hand type Stored Procedure code for Up and Down methods. 

In the future we might decide to move such Stored Procedure logic over to Web API App or introduce some sort of hybrid option where some part of the logic is ported over to Web API App and multiple Stored Procedures are created to support the overall process of Baseline Management.


