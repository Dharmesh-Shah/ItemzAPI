# ItemzAPI

This is a ***GREENFIELD*** project.

![GreenField Pic](./images/GreenField.jpg)

Main purpose of this project is to help users and teams to **`do it right`** and ultimately **`reduce waste`**.

Currently we are capturing  requirements for this project as Markdown files within this same repository. You can find them under [Docs and Notes](https://github.com/Dharmesh-Shah/ItemzAPI/tree/master/ItemzAPI/01%20Docs%20and%20Notes). 

Current challenges that we are working on can be found at ... [Feature we are developing and recently delivered section](#feature-we-recently-delivered).

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

Recently in May 2024 we implemented 

 - Developed support for getting data for Project >>> Baseline >>> BaselineItemzType >>> BaselineItemz >>> BaselineItemzTraces
 - Documented and Implemented relation between BaselineItemz and original Itemz 
 - Implemented concept that all Baselines are associated with a given Project
 - Started Researching on support for HierarchyID in SQL Server
 - Started documenting Concept of Itemz Hierarchy
 - [FIXED] RollingInterval in Logging Configuration

In  April 2024 we implemented

 - Documented Concept of Traceability
 - Implemented ItemzTrace capabilies to support Parent and Child Traces
 - Documented Design Decision of Baselining and Traceability
 - Implemented support for Scoped Traces for BaselineItemz Snapshots
 - Supporting Excluding and Including Baseline Itemz
 - Enhanced Test Scripts to cover Itemz Traces
 - Enhanced Test Scripts to cover Baseline Itemz and it's Traces
 - Ability to Create Baseline Copies including Traces

## Project's Next Challenges are 

 - Implementing support for Itemz Hierarchy
 - Decide upon UI technology to be used for ItemzApp
 - Document Conecpt of Change History for Itemz Traces
 - Upgrading to latest Nuget Packages
 - Research and Prototype for HTML / MarkDown editor
 - Upgrade to latest version of dependent Nuget packages
 - Setup Build Automation via Continuous Integration for Master Branch
 - Improve Postman Tests to cover more scenarios
 - Write T-SQL for performing HealthCheck of the System

**Your contributions as well as direction / support from community is much appreciated.**
