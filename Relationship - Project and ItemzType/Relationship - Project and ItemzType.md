
# Requirements of the Project and ItemzType APIs 
Thoughts and Ideas related to Project and ItemzType relationship.

## Event - Creation of Project
Everytime a project is created then we expect "Parking Lot" ItemzType to be created along with it. Parking ItemzType will have it's own unique GUID associated as ID but then it will have specific name like **Parking Lot**. 

Following table kind of shows Table Join Output from Project and ItemzType based on ProjectID.

|ProjectID   |ProjectName    |ItemzTypeID   |ItemzTypeName|
|----------------|-------------------------------|-----------------------------|----------------------|
|GUID_1|ProjectABC|GUID_101|Parking Lot| 
|GUID_2|ProjectLMN|GUID_102|Parking Lot|
|GUID_3|ProjectXYZ|GUID_103|Parking Lot|

Essentially, you can see that **Project Name** keeps changing but the Parking Lot ItemzType name stays as it is. 

> ***VERIFY*** that Duplicate Project Name does not exist in a single repository. This will otherwise cause lot of confusion to the user. Intention is to show Project Name in the selection list to choose from when user want's to connect and work with the project over UI.

## Event - Deletion of Project
Every associated ItemzType has to be removed when project is actually deleted. This in tern will remove all the Project data. In a simple words users must be asked confirmation on the client side before actually performing delete operation.

## Event - Updating Project
Updating project shall mainly involve ...

 - Renaming the Project.
 - Updating Description of the Project.
 - Changing Owner of the Project.
 - Changing Status of the Project between Active and Inactive.

Updating project shall not have any impact on ItemzType associated with it. Updating ItemzType shall be a separate activity outside of updating project itself.

 > ***VERIFY*** While renaming project, one has to make sure that Duplicate Project Name does not exist in a single repository.

## Query - List all ItemzType for a given Project
This will be required for client application to produce list of ItemzType when user logs into a given Project.

Think of Sorting order and make sure that Parking Lot is at the bottom of the list.

## Query - Number of Itemz in a Given Project
Calculate total number of requirements in the given project across multiple ItemzType. 

## Event - Creating ItemzType
ItemzType shall be always associated with one project. We don't see any need to associated ItemzType to multiple project for now. When project is deleted then all it's associated ItemzType shall be deleted as well.

One has to pass in Project ID while creating ItemzType so that it will be first checked for existence and then for verification and subsequently for associating with it.

> ***VERIFY*** That duplicate ItemsType Name is not present within the project.

## Event - Updating ItemzType
It's okay to support updating properties of ItemzType like Name, Description, etc.

> ***VERIFY*** That ItemsType with name "Parking Lot" remains read-only as far as updating ItemzType is concerned. One shall not see and and shall not perform update on "Parking Lot" system types. 

> ***VERIFY*** That duplicate ItemsType Name is not present within the project.

 In the future, we might also think of allowing moving ItemzType from one project to another project. This way, without performing lot of data migrations, one should be able to move ItemzType between projects. Again for now we don't need this feature as this is something that needs to be carefully evaluated. 

## Event - Deleting ItemzType
First, client UI application shall get confirmation from the user before deleting ItemzType.

When ItemzType is deleted then all the data associated with the given ItemzType shall be removed.

In the future we might look at shared Itemz between ItemzType. In such a scenario, we have to make sure that we keep count for Itemz to make sure that we remove it from the database when count goes down to ZERO. i.e. no one is referencing the Itemz.

## Query - Number of Itemz in a Given ItemzType
Calculate total number of requirements in the given ItemzType. 

## Check Periodically

 - Every Project has "Parking Lot" ItemzType
 - No Duplicate Project Names found in the Repository
 - No Duplicate ItemzType Names found in a given project
 - Each ItemzType belongs to one Project.
