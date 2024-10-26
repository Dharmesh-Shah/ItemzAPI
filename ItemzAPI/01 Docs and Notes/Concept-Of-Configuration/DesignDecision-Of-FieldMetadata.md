
# Design Decision of Field Metadata

Many fields in the system would rely on certain pre-existing meta data for allowing different choices to the user. This will help in developing User Interface and provide choices to the users while interacting with different types of data within the system. 

For example, users should be provided with a list of values in a drop down field while choosing status value for Itemz. 

There are many different ways in which solution can be implemented for obtaining this metadata for different types of fields. To start with, we shall keep this information in two different locations. One would be kept in the server API based application and another similar information will be kept on the client side within Enumerations. At the start, we need to provide values for drop down list within the user interface of ItemzApp and so we decided to keep it as shared enumeration that can be called by different pages within WebClient application. 

In the future we may change this to provide this information from the server side. This way, we can control customizations of this values as well as certain navigation rules to allow Admins to define workflows. 

At the start, we are looking at simplicity over complex workflow implementations for ItemzApp and so initial decision of holding similar information at two different places was ideal. One in the ServerSide APIs and another one would be in the WebClient application. 

That said, our Server Side APIs are designed to reject incoming insert / update request for different types of records where field values are not as per the pre-defined values. For example, if someone uses Custom API client to insert Itemz where status is “NOTDEFINED” then it will reject this request on the server side. 

At the time of writing this article, one can find such values in “EntitiesEnums.cs” and “EntityPropertyDefaultValues” files within Namespace “ItemzApp.WebUI.Client.SharedEntities” in the WebUI.Client project. 


