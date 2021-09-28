# Concept of Traceability

There are many reasons for linking to other requirements in the same project or across multiple projects. Traceability shall enable such feature by which one requirement can be linked to another requirement. Purpose of the trace could be of different nature. For example, 

 - One requirement is dependent on another requirement as part of delivery cycle. 
 - There are common features developed between two requirements and so they are supposed to be related with each other
 - Different flavour of the same component is injected in different model of the product with minor variation.
 - A change in one requirement would essentially impact another requirement(s)
 - Technical feasibility relates two features. In case if we find technical solution for one then we would essentially be able to deliver other related requirements too.
 - Customer has demanded to either deliver certain pack of requirements together. They would like to have either all of the identified features delivered or none to be delivered from the same pack.

So, there could be many more reasons to have related requirements that has trace / link established between them. In the initial versions of ItemzAPP, we shall not be able to support custom attributes on Traceability metadata to identify different types of traces. We shall start with supporting related link between different requirements within a single repository. 

#### What type of relation will there be between different requirements?
It shall be many-to-many via traceability. That means one requirement could be linked to many other requirements as well as traced requirement will be able to link to many other requirements. 

#### How will traceability data be maintained within a Baseline / Snapshot?
Traceability data shall be captured as part of the Baseline. That said, it will only capture traceability information for those requirements that are within the scope of the included requirements that are part of the baseline itself. Any traceability pointing to requirements that are not part of the captured baseline snapshot shall not be included in the baseline itself. 

Our thought process here is that requirements are fluid and they change in various ways. On the other hand, baseline is to show information about in scope requirements at the time of its creation. In scope data within the baseline is expected to remain static and so we don’t include information about traceability that points to other requirements outside of the scoped baseline requirements pack. 

#### What will happen to traces when requirement move from one requirement type to another?
Traceability between requirements shall point to individual requirements by their ID. It does not point to a requirement object via it’s Requirement Type (container) path. This means when requirements change it’s location by moving from one requirement type over to another one then existing established trace shall follow it over to the new location automatically. Traces are established between requirements directly within the same repository.

#### How will direction of the trace be established?
System shall support `From` and `To` trace properties. This way, user would be able to understand the direction of the trace and hierarchical relationship between two linked objects. In general, requirements that are considered as parent / contextually higher in the nature are identified as `From` requirements while establishing the trace relationship between two requirements object.

#### Will system support Cyclical trace?
System shall allow creation of Cyclical traces. That said, when user requests traceability hierarchy data then it shall identify such Cyclical traces and indicate the same via returning Cyclical Trace Object instead of vanilla requirement details. Users shall then not request further more data about Cyclical traces otherwise it will end up looping through the same data again and again.

We have to come-up with necessary design to make sure that custom applications that are programmed against ItemzAPI does not end-up in the situation where they keep calling for partial traceability information at a time which ends-up into endless loop of getting trace information that brings entire server application down.

#### Will system warn users about potential cyclical trace being established at the time of creation?

There are two separate cases that we need to focus on when it comes to warning user with appropriate response. First one would be when user uses ItemzAPP Client application in which they create one requirement at a time. In this case, it will be ideal to let user know that new changes that are registered in the repository will end-up establishing cyclical trace. 

That said, if user is importing large number of requirements into the system via import utility or some third-party integration then they might would like to ignore check for cyclical traces as it will slow the entire process down and the user is not able to take decision about each individual requirement as they would like to perform bulk import. 

Our current design should be to respond back with traceability data if it has created Cyclical Trace or not and then it’s up to the client application to establish if they would like to take any action based on returned information that they have received as part of ItemzAPI endpoint call.

#### Will system allow running health check to identify cyclical traces?
ItemzAPP shall support checking for Cyclical Traces at different container levels. It would be either Requirement Type, Project, Repository or a Baseline.  If there are cyclical traces found then the system shall include necessary metadata of the requirements and trace path that creates such cyclical traceability so that users can take corrective action about the same.

#### Will system support admins to define constrains about trace direction as well as target areas for traceability?
It will be ideal for Project Admins to be able to set-up trace direction and then introduce constrains to block users from creating traceability outside of the defined trace direction. That said, it’s also very important for system to remain flexible enough for users to be able to establish trace relationship between two requirements for different purposes. 

In the initial versions of ItemzAPP, we shall keep things relaxed and not introduce concept of constrains in terms of trace direction and target containers for traces. This means, one could establish trace relationship between two requirements irrespective of their location in the project structure. As long as they are in the same repository, establishing trace relationship between them shall be allowed.

It will be ideal to allow users to generate report indicating which trace relationship violates certain user defined trace constrains. This can help in identifying trace relationship which potentially should be removed. 



#### Will system support creating baseline by traceability relationship?
Such capabilities will help few advanced scenarios in which user would like to create a baseline from a given project based on established traceability. For example, if trace relationship is defined between different requirements across multiple types within a project to discuss with a supplier who is suppose to pick those requirements and deliver necessary components. In this scenario, it will be ideal to create a baseline from parent requirement via it’s trace hierarchy that covers different types of requirements from that single project. Then generate necessary documentation to be given to the supplier from that baseline which essentially contains traceability based requirements details. This could lead to better communication and so better quality of product and services that supplier is contracted for. 

Instead of given entire project access one could establish supplier specific baseline that gives them access to their concerned requirements without giving them too much details about the overall project / product / services for which requirements are captured in the repository.

#### Can traceability data be modified within a given baseline?
In the initial version, we are not expecting traceability data to be modified within a given baseline. That said, if we allow traceability to have more metadata in the future then it will be ideal to enable editing those metadata in the baseline so that one could further filter data based on traceability filter criteria.

#### Will user be able to request several level of traceability information in one call?
Users shall be allowed to request several level of traceability information in one call. That said, one should also have default limit of number of records that are rendered as part of a single call. We don’t want to end-up with heavy query and returning large amount of data if user is not going to consume them in one go. This is why we should establish limits based on number of records that we are returning and/or number of hierarchy levels that we return data for.

#### What will happen when requirement that is traced with other requirement is deleted?
We shall remove trace data for delete requirement from the repository. That said, any existing baseline that has taken snapshot with necessary trace information will not be changed / impacted due to deletion of requirement. 

#### Will there be a need for establishing trace between requirement and non-requirement objects?
In the initial versions of ItemzApp, we don’t expect trace relationship to be defined between non-requirement object. Because users shall be allowed to create different types of requirements then current design of allowing traceability between two requirements object within the same repository shall cover most of the cases. 

# Conclusion
Traceability should be allowed between two different requirements that are stored in the same repository.

Traceability should be established between two requirements records and system shall not support tracing to non-requirements objects.

Traceability details should be captured as part of baseline / snapshot. Trace information for requirements that are in scope of the baseline shall be included

Deleting requirement will remove it’s Parent and Child traces. Existing baseline shall continue to hold information about traces at the time of creating the baseline. 

Repository shall have ZERO Orphaned Traces. Itemz traceability and Baseline traceability tables shall not have any orphaned traceability data stored in them as we expect them to be delete when parent objects are deleted.




