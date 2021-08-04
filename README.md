# ItemzAPI

This is a ***GREENFIELD*** project.

![GreenField Pic](./images/GreenField.jpg)

Main purpose of this project is to help users and teams to **`do it right`** and ultimately **`reduce waste`**.

Currently we are capturing  requirements for this project as Markdown files within this same repository. You can find them under [Docs and Notes](https://github.com/Dharmesh-Shah/ItemzAPI/tree/master/ItemzAPI/01%20Docs%20and%20Notes). 

Current challenges that we are working on can be found at ... [Feature we are developing](#Feature-we-are-developing).

**Your contributions as well as direction / support from community is much appreciated.**

ItemzAPI is developed as Open Source Application Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

## Purpose

ItemzAPP is developed with focus on ease of defining, maintaining, sharing and reporting on different types of Itemz.

Itemz could be anything like

- Contract Document
- Needs and Wants Specification
- List of Activities
- Standard Specification
- Journal on Specific Topic
- Rule Book
- Cook Book
- Nicely described Life Memories in a Specific Order (Personal Notes)
- Research Report on specific topics (people collecting common findings on COVID-19 - Coronavirus)

BUT primarily, Itemz is nothing but Requirements / Needs / Wants.

## Discussions at ItemzAPI repository is now open.

![Ideas Pic](./images/ideas.png)

**Your Ideas and Contributions are much appreciated.**

We encourage you to ask us questions and provide your suggestions via Discussions forum which is now enabled for ItemzAPI repository at ... [ItemzAPI Discussions](https://github.com/Dharmesh-Shah/ItemzAPI/discussions) 

We hope you will also learn and gain from what has already being delivered in this project.

## Overview of Current State

We are in the process of defining Web APIs for Itemz Application. This Web APIs are mainly considered as server side programming that covers API and Repository  Database development.

Following are the key areas that we are working on for now...

- ASP .NET Core Web API
- Entity Framework Core as ORM tool.
- SQL Server as Database
- Postman for writing tests and simulate Web API with.

## Feature we recently delivered

Recently in April and May 2021 we implemented 

 - Individual Itemz SQUASH feature. 
 - Itemz SQUASH feature by ItemzType.
 - Itemz SQUASH feature by Project.
 - Managing Itemz change history for Orphan and Non-Orphan itemz as per ... [Concept of Versioning - Orphand Itemz and ItemzType](https://github.com/Dharmesh-Shah/ItemzAPI/blob/master/ItemzAPI/01%20Docs%20and%20Notes/Concept-Of-Versioning-Itemz/Concept-Of-Versioning-Itemz.md#what-about-attaching-orphand-itemz-to-itemztype)
 - Nullable Reference Types
 - Upgraded all the Nuget Packages in May 2021
 - Concept of Baselining

In  June and July 2021 we implemented

 - Designed table Schema for supporting Baselining Feature
 - First time implemented Stored Procedure as part of EF Core migrations
 - Implemented Baselining Feature
 - Handling of Orphaned BaselineItemz as per design decision
 - Added POSTMAN scripts for testing various scenarios around Baselining Feature
 - Upgrade to latest version of dependent Nuget packages

Project's Next Challenges are 

 - Design and implement Shrinking Baselines capabilities
 - Research and Prototype for HTML / MarkDown editor
 - Concept of Traceability
 - Upgrade to latest version of dependent Nuget packages
 - Setup Build Automation via Continuous Integration for Master Branch
 - Improve Postman Tests to cover more scenarios
 - Take decision about separating out Itemz and Orphaned Itemz

**Your contributions as well as direction / support from community is much appreciated.**
