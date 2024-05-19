# Concept of Hierarchy

To understand scope and proper meaning of an Itemz from a project perspective, it will be ideal to have it structured in some form of parent and child relations. This will help in understanding perspective of an independent Itemz in the wider concept and it will also help in defining order and scope of execution when it comes to implementing Itemz and overall project. 

ItemzApp shall be designed with flexibility in mind to allow individual projects to work according to their choice for defining such Itemz structure.

#### Conceptually, which options are available to define parent and child relation between Itemz?

There are two main ways to define Hierarchy between Itemz. 

1.	Parent and Child Trace relationship
2.	Itemz breakdown structure.

ItemzApp shall consider both of this type of relationships as separate independent ways to define structure. i.e. creating Parent and Child links between two different Itemz will not enforce that they are Parent and Child Itemz as far as breakdown structure is concerned. The same way, ItemzApp will not be creating Parent and Child Traces between Itemz which are having Parent and Child relation within hierarchical breakdown structure. These two concepts will be used completely manually by the application users to define relationship between two Itemz as Parent and Child. 

Projects using ItemzApp may choose different options to define Itemz relationship. Here are some common ways of working with respect to Itemz relationship. 

*	Fully rely on Parent and Child hierarchical breakdown structure to define their Itemz relationship. 
*	Keep Itemz structure almost flat in terms of hierarchical breakdown structure but use Traces to defined Parent and Child relationship. 
*	Use hybrid model where partial Itemz relationship is defined within hierarchical breakdown structure and also use Traces to define parent and child relationship between Itemz. 

ItemzApp shall not enforce one way over another when it comes to defining and using it for maintaining parent and child relationship amongst Itemz. Project using ItemzApp will have to define it by themselves based on different functionality supported in the application and document their own ways of working as per their project needs. 

#### Overall Hierarchy Structure

As far as Parent and Child Structure is concerned, logically speaking, ItemzApp has following hierarchy structure.

ItemzApp Repository >>> Project(s) >>> ItemzType(s) >>> Itemz(s) >>> multiple level of SubItemz(s). 

Within ItemzApp, we allow users to define their own ItemzType. That said, we also support concept of “Parking Lot” ItemzType. Every project by default has system ItemzType which is called “Parking Lot”. This is where users park their Itemz which are either not fully defined or they are not in scope of a given project but can be considered in the future.

ItemzApp shall allow defining Parent and Child Itemz Hierarchy support between Itemz that are stored in “Parking Lot” and this way, we can move a given set of Parent and Child Itemz together between different types including “Parking Lot” inbuild system ItemzType.  

#### Will hierarchy be maintained in Recycle Bin as well?

No, Recycle Bin contains Itemz which are marked for deletion. They are intended to be fully removed from the system. So, it’s not going to store details about Itemz Parent and Child Hierarchy relationship in it. If user would like to restore Itemz from Recycle Bin then they will have to re-create Parent and Child relationship for the restored Itemz. 

This way, it’s not necessary that Itemz within a parent and child hierarchical set will have to be restored together. Users will be able to restore some Itemz as necessary from the Recycle Bin without having to restore entire sets. In future we may also support restoring hierarchical sets of requirements from Recycle Bin but for now it’s something we will not be supporting.

Projects and users who wants to de-scope a given Itemz parent and child hierarchical data set from the project shall utilize “Parking Lot” system ItemzType. Instead of deleting Itemz hierarchical sets, one shall consider moving them into “Parking Lot” so that they can bring it back in the future. 

#### Where shall we compute and identify Hirarchy ID value for Itemz?

We have two choices here. We can pass in only the Parent OR Child Itemz information to the client side software and let the client side software compute Hierarchy ID depending upon how Itemz are displayed to the users. In this design we do  not store actual HierarchyID number in the datastore but instead we only have to capture information about ParentID in every Child Itemz record. Because, in this architecture we expect client software to compute and produce the Hierarchy ID number  for Itemz based on their position in the context in which it appears. This will require Client software to be more intelligent and will not be very easy to test it and enhance it in the future. This will make client software to hold some processing logic which can be avoided if we decide to compute and store hierarchy ID in the application tier. 

This is the reason why we shall now compute this at server side and store Itemz Hierarchy ID details along with Itemz in the datastore. Because we will support obtaining information about Hierarchy ID through Rest API, it will be easy to use the same in different types of client application for processing this information appropriately. 

#### Are there any reserved numbers within Hierarchy ID?

As far as implementation is concerned, we expect following reserved numbers formatting to Indicate information about container objects. 

**Example**

/1/1/1/1

-	First number 1 Indicates Repository ID. *This will remain as it is for all Itemz. i.e. it will always start from number 1*
-	Second number 1 indicates Project number 
-	Third number 1 indicates ItemzType number within project. Most likely this will be “Parking Lot” Itemz type as that would be 1st ItemzType within each project.
-	Fourth number 1 Indicates first Itemz

**Next example**

/1/4/2/8

-	Number 1 represents Repository 
-	Number 4 indicates 4th Project
-	Number 2 indicates 2nd ItemzType
-	Number 8 indicates 8th Itemz.

**Next example**

/1/3/7/2/3/9

-	Number 1 represents Repository
-	Number 3 indicates 3rd Project
-	Number 7 indicates 7th ItemzType
-	Number 2 indicates 2nd Itemz
-	Number 3 indicates 3rd child of 2nd Itemz
-	Number 9 indicates 9th child of 3rd child of 2nd Itemz.

**Next example**

/3/1/4/5
	
-	HierarchyID number starting with 3 is faulty number. We expect it to start with repository ID of 1 as root. 

**Next example**

/1/7/3/2/1.1

-	Number 1 represents Repository
-	Number 7 indicates 7th Project
-	Number 3 indicates 3rd ItemzType
-	Number 2 indicates 2nd Itemz
-	Number 1.1 indicates Itemz that is in between “1st child of 2ndItemz” and “1.2 or greater child of 2nd Itemz”.


**Next example**

/1/3/2
-	Number 1 represents Repository
-	Number 3 indicates 3rd Project
-	Number 2 indicates 2nd ItemzType
-	This number stops at ItemzType and it does not represent any Itemz instead it represents ItemzType.

**Next example**

1/1.1/3/5
-	Number 1 represents Repository
-	Number 1.1 is wrong as we don’t expect projects to ever move within the repository. Projects are always incrementing by 1 as there is no need to change project hierarchy within ItemzApp.

**Next example**

1/3/1.1/5
-	Number 1 represents Repository
-	Number 3 indicates 3rd Project
-	Number 1.1 indicates ItemzType which is in between ItemzType 1 and ItemzType 1.2 or greater.
-	Number 5 indicates 5th Itemz.


#### Why do we want to capture Repository number as part of Hierarchy ID and why should it always be 1?

While doing research about Hierarchy ID support for SQL Server database, we came across several articles that explains that root should always  be one object. Everything else should hang off the root object. Because highest most container of data within ItemzApp is Repository, we thought it will be easier to start with repository to be the root of the hierarchy number. 

Also, default number is 1 for the root / first object in Hierarchy context and so we decided to keep it at that as the start of the hierarchy ID which will represent Repository itself. 

We expect customers to move Itemz including it’s child Itemz hierarchy sets from one place to another place. That could also include moving it across projects and so having project being part of Hierarchy ID would help a lot. 

#### Why should Project number be whole number in HierarchyID?

In ItemzApp, we don’t expect project to move from one place to another place within Hierarchy. This is why it should always be a whole number like 1, 2, 3, etc. 

We expect customer to move “ItemzType” and “Itemz” around and so they can contain decimal numbers in the Hierarchy ID instead of just whole numbers. 

#### What’s impact of sharing Itemz in multiple places on Hierarchy ID?

As per current architecture, we expect one ItemzID to have one HierarchyID associated withit. This will unfortunately not allow Itemz to be shared across multiple places. So for now, we expect users to create copy of a given Itemz and share it at another location rather then performing a deep sharing of Itemz at multiple places. 

#### Will we transform Hierarchy Number before sending it to the client application?

At this stage, we are planning to send the exact Hierarchy ID number as we store it in the datastore. In the future we might introduce additional transformation layer(s) that will change Hierarchy ID number while sending and receiving data over ItemzAPI.

#### How should Hierarchy ID be supported for Baseline and Itemz in it?

ItemzApp shall support storing Hierarchy ID for Baseline as per following structure

Repository >>> Project >>> Baseline >>> BaselineItemzType >>> Baseline Itemz >>>> Baseline SubItemz structure. 

**So the actual format will be**

/1/2/3/4/5/6/

-	Number 1 represents repository
-	Number 2 indicates 2nd Project
-	Number 3 indicates 3rd Baseline
-	Number 4 indicates 4th BaselineItemzType
-	Number 5 indicates 5th BaselineItemz
-	Number 6 indicates 6th child of 5th BaselineItemz.

We expect Hierarchy id to be whole number across all types of nodes for Baseline data as we don’t expect them to move in the hierarchy. 

Users may decide to include / exclude BaselineItemz and this should impact BaselineItemz Parent and Child Hierarchy record sets. ItemzApp shall support multiple BaselineItemz related to each other via hierarchy record sets to be Included or Excluded together in one transaction. Any other impact this operation would have, say, on Traceability BaselineItemz shall also be considered and implemented for bulk operations of inclusion and exclusion of related hierarchy recordsets together.

#### Will Hirarchy ID Number changing be captured as part of Change Management records?

No, because there are many reasons for HierarchyID number to change due to different actions users perform, it makes it not so interesting for users to have an recoreds of each and every such change be captured as part of Hierarchy ID changes. Also from actual usage perspective, we don't see any major relavance of capturing this values. 

Because we are going to support concept of Baselining, we expect users to take such snapshot when they achieve certain milestone in their requirements definition process. This will enable them to see location of Itemz with respect to the snapshot in time taken previously by the project using ItemzApp. 


 










	










