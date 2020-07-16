// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using ItemzApp.API.Entities;
using AutoMapper;
using ItemzApp.API.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ItemzApp.API.ResourceParameters;
using ItemzApp.API.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace ItemzApp.API.Controllers
{
    //[ApiController]
    //[Route("api/ProjectItemzs")] // e.g. http://HOST:PORT/api/ProjectItemzs
    ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
    ////[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public class ProjectItemzsController : ControllerBase
    //{
    //    private readonly IItemzRepository _itemzRepository;
    //    private readonly IMapper _mapper;
    //    private readonly IPropertyMappingService _propertyMappingService;
    //    private readonly ILogger<ProjectItemzsController> _logger;

    //    public ProjectItemzsController(IItemzRepository itemzRepository,
    //        IMapper mapper,
    //        IPropertyMappingService propertyMappingService,
    //        ILogger<ProjectItemzsController> logger)
    //    {
    //        _itemzRepository = itemzRepository ?? throw new ArgumentNullException(nameof(itemzRepository));
    //        _mapper = mapper ??
    //            throw new ArgumentNullException(nameof(mapper));
    //        _propertyMappingService = propertyMappingService ??
    //            throw new ArgumentNullException(nameof(propertyMappingService));
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    //    }

        ///// <summary>
        ///// Gets collection of Itemzs by Project ID
        ///// </summary>
        ///// <param name="ProjectId">Project ID for which Itemz are queried</param>
        ///// <param name="itemzResourceParameter">Pass in information related to Pagination and Sorting Order via this parameter</param>
        ///// <returns>Collection of Itemz based on expectated pagination and sorting order.</returns>
        ///// <response code="200">Returns collection of Itemzs based on pagination</response>
        ///// <response code="404">No Itemzs were found</response>
        //[HttpGet("{ProjectId:Guid}", Name = "__GET_Itemzs_By_Project__")]
        //[HttpHead("{ProjectId:Guid}", Name = "__HEAD_Itemzs_By_Project__")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public ActionResult<IEnumerable<GetItemzDTO>> GetItemzsByProject(Guid ProjectId,
        //    [FromQuery] ItemzResourceParameter itemzResourceParameter)
        //{
        //    if (!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
        //        (itemzResourceParameter.OrderBy))
        //    {
        //        _logger.LogWarning("Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!", itemzResourceParameter.OrderBy);
        //        return BadRequest();
        //    }

        //    if(!_itemzRepository.ProjectExists(ProjectId))
        //    {
        //        _logger.LogDebug("Project with ID {ProjectID} was not found in the repository", ProjectId);
        //        return NotFound();
        //    }

        //    var itemzsFromRepo = _itemzRepository.GetItemzsByProject(ProjectId, itemzResourceParameter);
        //    // EXPLANATION : Check if list is IsNullOrEmpty
        //    // By default we don't have option baked in the .NET to check
        //    // for null or empty for List type. In the following code we are first checking
        //    // for nullable itemzsFromRepo? and then for count great then zero via Any()
        //    // If any of above is true then we return true. This way we log that no itemz were
        //    // found in the database.
        //    // Ref: https://stackoverflow.com/a/54549818
        //    if (!itemzsFromRepo?.Any() ?? true)
        //    {
        //        _logger.LogDebug("No Items found in Project with ID {ProjectID}", ProjectId);
        //        // TODO: If no itemz are found in a project then shall we return an error back to the calling client?
        //        return NotFound();
        //    }
        //    _logger.LogDebug("In total {ItemzNumbers} Itemz found in Project with ID {ProjectId}", itemzsFromRepo.TotalCount, ProjectId);
        //    var previousPageLink = itemzsFromRepo.HasPrevious ?
        //        CreateProjectItemzResourceUri(itemzResourceParameter,
        //        ResourceUriType.PreviousPage) : null;

        //    var nextPageLink = itemzsFromRepo.HasNext ?
        //        CreateProjectItemzResourceUri(itemzResourceParameter,
        //        ResourceUriType.NextPage) : null;

        //    var paginationMetadata = new
        //    {
        //        totalCount = itemzsFromRepo.TotalCount,
        //        pageSize = itemzsFromRepo.PageSize,
        //        currentPage = itemzsFromRepo.CurrentPage,
        //        totalPages = itemzsFromRepo.TotalPages,
        //        previousPageLink,
        //        nextPageLink
        //    };

        //    // EXPLANATION : it's possible to send customer headers in the response.
        //    // So, before we hit 'return Ok...' statement, we can build our
        //    // own response header as you can see in following example.

        //    // TODO: Check if just passsing the header is good enough. How can we
        //    // document it so that consumers can use it effectively. Also, 
        //    // how to implement versioning of headers so that we don't break
        //    // existing applications using the headers after performing upgrade
        //    // in the future.

        //    Response.Headers.Add("X-Pagination",
        //        JsonConvert.SerializeObject(paginationMetadata));

        //    _logger.LogDebug("Returning results for {ItemzNumbers} Itemzs to the caller", itemzsFromRepo.TotalCount);
        //    return Ok(_mapper.Map<IEnumerable<GetItemzDTO>>(itemzsFromRepo));
        //}

        ///// <summary>
        ///// Check if specific Project and Itemz association exists
        ///// </summary>
        ///// <param name="projectId">Provide Project Id</param>
        ///// <param name="itemzId">Provide Itemz Id</param>
        ///// <returns>GetItemzDTO for the Itemz that has specified Project association</returns>
        ///// <response code="200">Returns GetItemzDTO for the Itemz that has specified Project association</response>
        ///// <response code="404">No Project and Itemzs association was found</response>
        //[HttpGet("CheckExists/", Name = "__GET_Check_Project_Itemz_Association_Exists__")]
        //[HttpHead("CheckExists/", Name = "__HEAD_Check_Project_Itemz_Association_Exists__")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]

        //public ActionResult<GetItemzDTO> CheckProjectItemzAssociationExists([FromQuery] Guid projectId, Guid itemzId) // TODO: Try from Query.
        //{
        //    var tempProjectItemzDTO = new ProjectItemzDTO();

        //    tempProjectItemzDTO.ProjectId = projectId;
        //    tempProjectItemzDTO.ItemzId = itemzId;
        //    if (!_itemzRepository.ProjectItemzExists(tempProjectItemzDTO))  // Check if ProjectItemz association exists or not
        //    {
        //        _logger.LogDebug("HttpGet - Project ID {ProjectId} and Itemz ID {ItemzId} association could not be found",
        //            tempProjectItemzDTO.ProjectId,
        //            tempProjectItemzDTO.ItemzId);
        //        return NotFound();
        //    }
        //    _logger.LogDebug("HttpGet - Project ID {ProjectId} and Itemz ID {ItemzId} association was found",
        //                    tempProjectItemzDTO.ProjectId,
        //                    tempProjectItemzDTO.ItemzId);
        //    return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = tempProjectItemzDTO.ItemzId });

        //}

        ///// <summary>
        ///// Used for creating new Itemz record in the database by Project ID
        ///// </summary>
        ///// <param name="projectId">Project ID in Guid Form. New Itemzs will be associated with provided Project Id</param>
        ///// <param name="itemzCollection">Used for populating information in the newly created itemz in the database by Project ID</param>
        ///// <returns>Newly created Itemzs property details</returns>
        ///// <response code="201">Returns newly created itemzs property details</response>
        //[HttpPost("{projectId:Guid}", Name = "__POST_Create_Itemz_Collecction_By_Project__")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public ActionResult<IEnumerable<GetItemzDTO>> CreateItemzCollectionByProject(
        //     Guid projectId,
        //    IEnumerable<CreateItemzDTO> itemzCollection)
        //{
        //    if (!_itemzRepository.ProjectExists(projectId))
        //    {
        //        _logger.LogDebug("Project with ID {projectID} was not found in the repository", projectId);
        //        return NotFound();
        //    }

        //    var itemzEntities = _mapper.Map<IEnumerable<Entities.Itemz>>(itemzCollection);
        //    foreach (var itemz in itemzEntities)
        //    {
        //        _itemzRepository.AddItemzByProject(itemz, projectId);
        //    }
        //    _itemzRepository.Save();

        //    var itemzCollectionToReturn = _mapper.Map<IEnumerable<GetItemzDTO>>(itemzEntities);
        //    var idConvertedToString = string.Join(",", itemzCollectionToReturn.Select(a => a.Id));

        //    _logger.LogDebug("Created {NumberOfItemzCreated} number of new Itemz and associated to Project Id {projectId}"
        //        , itemzCollectionToReturn.Count()
        //        , projectId);
        //    return CreatedAtRoute("__GET_Itemz_Collection_By_GUID_IDS__",
        //        new { Controller = "ItemzCollection", ids = idConvertedToString }, itemzCollectionToReturn);

        //}

        ///// <summary>
        ///// Used for Associating Itemz to Project 
        ///// </summary>
        ///// <param name="projectItemzDTO">Used for Associating Itemz to Project through ItemzId and ProjectId Respectively</param>
        ///// <returns>GetItemzDTO for the Itemz that has specified Project association</returns>
        ///// <response code="200">Itemz to Project association was either found or added successfully</response>
        ///// <response code="404">Either Itemz or Project was not found </response>
        //[HttpPost(Name = "__POST_Associate_Itemz_To_Project__")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public ActionResult<GetItemzDTO> AssociateItemzToProject(ProjectItemzDTO projectItemzDTO)
        //{
        //    if (!_itemzRepository.ProjectExists(projectItemzDTO.ProjectId))
        //    {
        //        _logger.LogDebug("Project with ID {projectID} was not found in the repository", projectItemzDTO.ProjectId);
        //        return NotFound();
        //    }
        //    if (!_itemzRepository.ItemzExists(projectItemzDTO.ItemzId))
        //    {
        //        _logger.LogDebug("Itemz with ID {itemzID} was not found in the repository", projectItemzDTO.ItemzId);
        //        return NotFound();
        //    }

        //    _itemzRepository.AssociateItemzToProject(projectItemzDTO);
        //    _itemzRepository.Save();
        //    _logger.LogDebug("HttpPost - Project Itemz Association was either created or found for Project ID {projectID}" +
        //        " and Itemz Id {itemzId}", projectItemzDTO.ProjectId, projectItemzDTO.ItemzId);

        //    return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = projectItemzDTO.ItemzId });
        //}

        ///// <summary>
        ///// Move Itemz from one project to another
        ///// </summary>
        ///// <param name="projectId">GUID representing an unique ID of the Target Project for moving Itemz into</param>
        ///// <param name="targetProjectItemzDTO">Details about target Project and Itemz association</param>
        ///// <returns>No contents are returned when expected Project and Itemz association is established</returns>
        ///// <response code="204">No content are returned but status of 204 indicated that expected Project and Itemz association is established</response>
        ///// <response code="404">Either Itemz or Project was not found</response>
        //[HttpPut("{projectId}", Name = "__PUT_Move_Itemz_Between_Projects__")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public ActionResult MoveItemzBetweenProjects(Guid projectId, ProjectItemzDTO targetProjectItemzDTO)
        //{
        //    if (!_itemzRepository.ItemzExists(targetProjectItemzDTO.ItemzId))// Check if Itemz exists

        //    {
        //        _logger.LogDebug("HttpPut - Itemz for ID {ItemzId} could not be found", targetProjectItemzDTO.ItemzId);
        //        return NotFound();
        //    }
        //    if (!_itemzRepository.ProjectExists(targetProjectItemzDTO.ProjectId))  // Check if Target Project Exists
        //    {
        //        _logger.LogDebug("HttpPut - Target Project for ID {ProjectId} could not be found", targetProjectItemzDTO.ProjectId);
        //        return NotFound();
        //    }

        //    var sourceProjectItemzDTO = new ProjectItemzDTO();
        //    sourceProjectItemzDTO.ItemzId = targetProjectItemzDTO.ItemzId;
        //    sourceProjectItemzDTO.ProjectId = projectId;

        //    if (!_itemzRepository.ProjectItemzExists(sourceProjectItemzDTO))  // Check if Source ProjectItemz association exists or not
        //    {
        //        _logger.LogDebug("HttpPut - Source Project ID {ProjectId} and Itemz ID {ItemzId} association could not be found",
        //            sourceProjectItemzDTO.ProjectId,
        //            sourceProjectItemzDTO.ItemzId);

        //    }
        //    _itemzRepository.MoveItemzFromOneProjectToAnother(sourceProjectItemzDTO, targetProjectItemzDTO);
        //    _itemzRepository.Save();

        //    _logger.LogDebug("HttpPut - Itemz ID {ItemzId} move from Source Project ID {sourceProjectID} " +
        //        "to Target Project ID {targetProjectID} was successfully completed", 
        //        sourceProjectItemzDTO.ItemzId,
        //        sourceProjectItemzDTO.ProjectId,
        //        targetProjectItemzDTO.ProjectId);
        //    return NoContent(); // This indicates that update was successfully saved in the DB.

        //}

        //public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        //{
        //    var options = HttpContext.RequestServices
        //        .GetRequiredService<IOptions<ApiBehaviorOptions>>();

        //    return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        //}


        ///// <summary>
        ///// Deleting a specific Itemz and Project association. This will not delete Itemz or project from the database,
        ///// instead it will only remove their association if found. 
        ///// </summary>
        ///// <returns>Status code 204 is returned without any content indicating that deletion of the specified Project and Itemz association was successful</returns>
        ///// <response code="404">Project and Itemz association not found</response>
        //[HttpDelete(Name = "__DELETE_Project_and_Itemz_Association__")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public ActionResult DeleteProjectAndItemzAssociation(ProjectItemzDTO projectItemzDTO)
        //{
        //    if (!_itemzRepository.ProjectItemzExists(projectItemzDTO))
        //    {
        //        _logger.LogDebug("Cannot find Project and Itemz asscoaition for Project ID " +
        //            "{ProjectId} and Itemz ID {ItemzId}", 
        //            projectItemzDTO.ProjectId,
        //            projectItemzDTO.ItemzId);
        //        return NotFound();
        //    }

        //    _itemzRepository.RemoveItemzFromProject(projectItemzDTO);
        //    _itemzRepository.Save();

        //    _logger.LogDebug("Delete Project and Itemz asscoaition for Project ID " +
        //        "{ProjectId} and Itemz ID {ItemzId}", 
        //        projectItemzDTO.ProjectId, 
        //        projectItemzDTO.ItemzId);
        //    return NoContent();
        //}

        ///// <summary>
        ///// Get list of supported HTTP Options for the Itemz controller.
        ///// </summary>
        ///// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        ///// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        //[HttpOptions (Name ="__OPTIONS_for_Project_Itemz_Controller__")]
        //public IActionResult GetProjectItemzOptions()
        //{
        //    Response.Headers.Add("Allow","GET,HEAD,POST,PUT,DELETE");
        //    return Ok();
        //}

        //private string CreateProjectItemzResourceUri(
        //    ItemzResourceParameter itemzResourceParameter,
        //    ResourceUriType type)
        //{
        //    switch (type)
        //    {
        //        case ResourceUriType.PreviousPage:
        //            return Url.Link("__GET_Itemzs_By_Project__",
        //                new
        //                {
        //                    orderBy = itemzResourceParameter.OrderBy,
        //                    pageNumber = itemzResourceParameter.PageNumber - 1,
        //                    pageSize = itemzResourceParameter.PageSize
        //                });
        //        case ResourceUriType.NextPage:
        //            return Url.Link("__GET_Itemzs_By_Project__",
        //                new
        //                {
        //                    orderBy = itemzResourceParameter.OrderBy,
        //                    pageNumber = itemzResourceParameter.PageNumber + 1,
        //                    pageSize = itemzResourceParameter.PageSize
        //                });
        //        default:
        //            return Url.Link("__GET_Itemzs_By_Project__",
        //                new
        //                {
        //                    orderBy = itemzResourceParameter.OrderBy,
        //                    pageNumber = itemzResourceParameter.PageNumber,
        //                    pageSize = itemzResourceParameter.PageSize
        //                });
        //    }
        //}
    //}
}