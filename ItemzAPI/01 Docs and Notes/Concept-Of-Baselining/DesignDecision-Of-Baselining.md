
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

### Conclusion
Given that we are in the initial phase of application development for Itemz API, our current decision is to write baseline logic directly in the Stored Procedure and get the feature rolled in. This will require us to work with empty migration in EF Core in which we hand type Stored Procedure code for Up and Down methods. 

In the future we might decide to move such Stored Procedure logic over to Web API App or introduce some sort of hybrid option where some part of the logic is ported over to Web API App and multiple Stored Procedures are created to support the overall process of Baseline Management.

---

# Removing BaselineItemz post deleting Project / Baseline / BaselineType
While working on implementing Baselines in ItemzAPI, we learned one more thing for which it's important to capture design decision details as per below.


Key difference between Itemz and BaselineItemz is that when Project is deleted then all it's associated Itemz are marked as Orphend Itemz. But in the same case, when we delete a Project then BaselineItemz shall be removed instead of staying in the system as Orphend BaselineItemz. So this means we have to either 
 - set-up trigger in the database to remove BaselineItemz
 - or Make sure that we call Delete Orphend BaselineItemz from ItemzAPI
 - Or we periodically remove them from the system

Following diagram shows relationship between different baselines tables within ItemzApp

![Baseline Tables Relationships](./BaselineTablesRelationships.jpg)

As you can see in the above diagram, Database is designed to automatically delete Baseline, BaselineTypes, BaselineTypesJoinBaselineItemz when Project is deleted. This is because we perform Cascade Delete in Child Table when Parent table data is deleted.

We don't do this in the other direction. i.e. When record in BaselineTypesJoinBaselineItemz table is deleted then we don't delete this record from it's Parent table BaselineItemz. 

This way, we are left with Orphend BaselineItemz. 

For now, we think the best way to take care of this would be to delete records from BaselineItemz that does not have related referenced data in BaselineTypesJoinBaselineItemz via Stored Procedure that can then be called from within ItemzAPI. This means that in the following events, we have to call this Stored Procedure to perform necessary clean-up.

 - When Project is Deleted
 - When Baseline is Deleted
 - When BaselineType is Deleted

### Conclusion
Instead of adding trigger in the SQL Server Database for table BaselineTypesJoinBaselineItemz to remove unreferenced data from it's parent table BaselineItemz, we will create a separate Stored Procedure to perform this clean-up. This way, we are not depending too much into capabilities provided by SQL Server for now.

---
# Reversing relation between BaselineTypeJoinBaselineItemz and BaselineItemz

Right now it's not deleting BaselineItemz because it's parent to BaselineTypeJoinBaselineItemz table. It has to be child of BaselineTypeJoinBaselineItemz table. 

What is the concequence of changing referencial integrity other way round?

i.e. we first insert BaselineTypeJoinBaselineItemz and then capture newly created GUID as OUTPUT - INSERTED value that we use for inserting record in BaselineItemz. This way, we can mark BaselineTypeJoinBaselineItemz as parent and BaselineItemz as child. 

Later if we delete Project / Baseline / BaselineType then we automatically remove records via CASCADE delete from both the tables, BaselineTypeJoinBaselineItemz and BaselineItemz respectively. 

We are not sure what will be the impact of this when it comes to quering data via EF Core. As well as what will be the impact when we support custom Data Types in the future. I do not think it will be a huge problem as referenced records are well supported in EF Core. Ultimately it will have One to many refencial integrity between Project --> Baseline --> BaselineType --> BaselineTypeJoinBaselineItemz. And ultimately it will have One to Zero OR One  refencial integrity between  BaselineTypeJoinBaselineItemz --> BaselineItemz.

We will have to try this option out in a separate branch that we create from BaselineBranch. 

### Conclusion

Due to time constrains as well as further complexity expected in future version of ItemzApp with respect to introduction of Custom Attributes on Itemz, we are dropping this idea of reversing Parent and Child relationship between tables  BaselineTypeJoinBaselineItemz and BaselineItemz. 

Lets leave BaselineItemz as Parent and BaselineTypeJoinBaselineItemz as child with One to Many relationship. 

What we might encounter is that some repositories may have large number of Orphend BaselineItemz when Project / Baseline / BaselineItemz is deleted. 








