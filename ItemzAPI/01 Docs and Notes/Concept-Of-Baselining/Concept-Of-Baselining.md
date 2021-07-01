
# Concept of Baselining

Requirements are fluid and they keep changing over the period of time. That said many requirements remain same over a long period of time too. Due to this, a software application that is managing requirements needs to cater to both this need. 

Baselining is a process of taking snapshot of collection of requirements along with associated attributes value. This can be used for many different purposes like

-	Finalized list of agreed requirements
-	Draft version of requirements
-	Identify specific portion of requirements from overall collection of requirements in a project
-	Captured requirements post brainstorming session
-	Defined set of requirements that shall be passed down to sub-contractors
-	Loosely defined requirements that needs to be worked upon
-	Requirements that are assigned to the team to implement while there are new requirements that are being captured for next phase
-	Ideas that are important but there is no budget for it

These are few reasons why Baselining / Snapshotting would be important feature as part of software application that manages requirements. 


#### Where does Baseline fit in the given structure

Baselines are associated with Project. It does not matter if you include all the requirements into Baseline or just few from the given project. Every baseline shall be linked to only one project and on the other hand a given project can have zero to many baselines associated with it.

#### Can Baseline be associated with Requirement Type?

ItemzAPP shall support baselines to be associated with project and not with requirements type. It’s possible that a baseline might include requirements that are included within a single requirement type within a project but baseline itself shall be linked to the project itself.

#### How will Baseline data be stored in the repository?

Each baseline will take a snapshot of the current state of the requirements in scope and store all the values in a separate table that are easy to query and does not change. Basically, we will end-up with duplicate copies of the requirements in the repository. This might look like an extreme overhead in terms of number of records we shall have in the repository if number of baselines are high. That said, it will be much easier to retrieve data for querying and reporting purposes based on Baseline information.

#### Will baseline store requirements change history as well?

No, baseline is a snapshot of requirements and it shall only have requirements data without it’s change history log. This can be retrieved directly via requirements itself if needed. 

In general, people using ItemzAPP for managing their requirements shall use baselining feature as a contract / document. Users shall not be encouraged to use this feature for maintaining requirements from within the baseline which might lead to confusion. It’s always possible to capture new baselines within the target project. 

#### What about system and custom attributes associated with requirements?

Baselines shall not only copy information about requirements data itself but also it’s associated attribute details too. This way, we are able to store information about system and custom attributes like status, priority, estimation, customer priority, etc. as part of Snapshot that we have taken at the time of creating baseline. Later, this attribute data / metadata for the requirements might change but values in the baseline will be static and shall remain unchanged. 

#### What about traceability of requirements?

At the time of writing these requirements, we have not yet captured requirement for traceability feature for ItemzAPP. In a nutshell, Baselines shall capture traceability information of requirements. If a requirement is tracing to another requirement outside of the given project then that information shall be captured as part of the Baseline. That said, we need to give more thoughts about how we manage links / traces between requirements that are part of different projects within the ItemzAPP. 

We will have to look into it at the time of capturing requirements for traceability and while implementing the same in ItemzAPP. 

#### Will attachments be associated with Baselines?

Attachments are important and are special within ItemzAPP. It will be important to associate attachments to the snapshot requirements that are part of the given Baseline. That said, this attachment might be changed / removed as requirements are refined within the project. We need to make sure that attachments that were present at the time of taking the Baseline snapshot remains static and unmodified. 

To achieve this, we will have to introduce a mechanism whereby we keep counter of the pointer for the attachment file. This feature of supporting attachment via pointer is explained as per below scenario.

1st Jan – A new requirement is captured with attachment of a given file. This attachment shall be stored in the repository with pointer counter of 1
5th Jan – Attachment file is now updated and a new version of the file is uploaded in the same requirement. This will replace original file with the new version but the pointer counter shall remain 1.
11th Jan – Baseline is captured for the entire project include the requirement in which attachment is present. At this stage, pointer counter for the attachment file will increase to 2. One from the requirement from the project and the second pointer is from the baseline snapshot. 
13th Jan – Original requirement’s attachment is modified yet again and uploaded as attachment with the same file name. This will add new attachment records for the modified file with pointer counter of 1 which is coming in from the project itself. But the pointer counter for the previous version of the file (from 11th Jan ) shall be reduced from 2 to 1 as only Baseline pointer is pointing at it. 
15th Jan – Baseline that was captured on 11th Jan is now removed / deleted. Due to this the single pointer to the attachment file that was coming in from the Baseline is now set to zero and so the attachment file is removed as well.
This process is important as Attachments could be heavy and duplicating the same instead of maintaining pointer counters could have performance implications as well as increase in the size of the repository. 

NOTE: At the time of writing this requirement, ItemzAPP did not support attachments for the requirements yet. This feature is planned in the future.

#### Are there plans to capture baselines across multiple projects within a single repository?

In the first iteration, ItemzAPP shall keep it simple and we shall implement supporting Baselining at project level as described above. Once this implementation is done then we look into enhancing the feature to allow capturing baselines across multiple projects from within a single repository. 

If we allow baselines to be captured across multiple projects from within a single repository then we might be able to support traceability and Link based baselines. i.e. capture snapshot of all the requirements that are directly or indirectly linked to this higher level of requirement across all the projects in a given repository. 

#### Will I be able to create new project based on Baseline?

This is a great idea and for now we are not fully ready to implement the same in ItemzAPP. Purpose of creating project from the given baseline would allow user to bring baselined requirements to life and start modifying the same. This means a new project can be started whose base itself was a Baseline. This feature will be very useful for deliveries which are considered to be repetitive. Let’s consider Olympics for that matter. Requirements that were captured for Olympics that were hosted in Brazil during 2016 event can be baselined and a new project can be created based on this baseline for next event that is supposed to be hosted in Japan in year 2020 or China in 2022. 

Similarly, a film director could possibly reuse several common requirements from the given baseline from his previous film into the next upcoming film by establishing new project based on previously taken snapshot. 

Another example where this feature could be very useful would be for companies producing cars / automobiles. There are multiple different car models being delivered by a given company. Baselined requirements from the previous version of the car can be utilized by creating new car project based on it. This will make sure that large number of requirements, including requirements for component supplier are getting ready. Well thought-out requirements from the earlier model can be used as baseline and lessons learned from that model can be used to further enhance requirements for the next model which is planned for delivery in few years. 

#### Can new baseline be created based on existing baseline?

Yes, ItemzAPI shall support creating new baseline based on existing baseline. This way, one could leave original baseline untouched and then use the new baseline to perform necessary adjustments of including and excluding requirements in to it. 

#### Will baseline support including and excluding of requirements?

We shall support Shrinking Baseline model to start with. This means, if the baseline was created with 1000 requirements in it then we allow requirements to be removed from it but new requirements can NOT be added. So we can shrink baseline down to 900 requirements and then to 750 requirements and so on. 

In the future we might consider allowing adding new requirements into the baseline or updating existing requirements from the current lot. We are not considering this feature in the current iteration because when we include new requirements into the baseline then we have to take care of custom attributes, traceability, attachments, etc. This is a complex thing to implement and the benefit it would bring to the users has to be assessed. Also, the purpose of the baseline is to take a snapshot of the data at the time baseline was created. By supporting shrinking baselines, we emphasize culture of “move forward and repeat baselining” within the userbase of ItemzAPP. Baselining should be considered as throwaway snapshots and new ones shall be created as and when new milestones are achieved. 

#### Generating report based on Baseline.

At the time of writing these requirements for baselining, ItemzAPP did not have feature of generating reports implemented in the application. In the future, we expect that report generating capabilities shall allow generating variety of report based on requirements stored against Project as well as Baseline. 

# Conclusion
-	Baselining is a very important concept that takes snapshot of requirements including it’s attributes, attachments, traces / links.
-	We shall support Shrinking Baseline model to start with.
-	New projects shall be allowed to be created based on existing baseline
-	Cross project baselines are desirable but first we have to implement single project based baselines.
-	Because attachments could be heavy files, we expect baselines to utilize counter of pointers that are associated with attachment files. This will improve performance of creating baseline feature as well as keep repository size in check. 
-	Because version history / change history is not considered to be first class citizens, i.e. we allow removal of change history and squash requirements history by cut-of date and time, it makes it difficult to adjust baselines to different version of requirements.  

# Checkout 
 - [Design Decision of Baselining](./DesignDecision-Of-Baselining.md)