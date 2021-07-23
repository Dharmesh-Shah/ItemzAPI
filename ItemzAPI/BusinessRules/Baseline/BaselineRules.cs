// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ItemzApp.API.BusinessRules.Baseline
{
    public class BaselineRules : IBaselineRules
    {
        private readonly IBaselineRepository _baselineRepository;
        private readonly ILogger<BaselineRules> _logger;
        public BaselineRules(IBaselineRepository baselineRepository,
                                 ILogger<BaselineRules> logger)
        {
            _baselineRepository = baselineRepository ?? throw new ArgumentNullException(nameof(baselineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        /// <summary>
        /// Use this method to check if the Baseline with given name already exists. In General, 
        /// This check shall be performed before inserting or updating Baseline.
        /// </summary>
        /// <param name="projectId">Project Id in Guid form in which we are checking for Baseline with a specific name</param>
        /// <param name="baselineName">Name of the Baseline to be checked for uniqueness</param>
        /// <returns>true if Baseline with BaselineName found otherwise false</returns>
        private async Task<bool> HasBaselineWithNameAsync(Guid projectId, string baselineName)
        {
            if (await _baselineRepository.HasBaselineWithNameAsync(projectId ,baselineName.Trim().ToLower()))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Used for verifying if project contains baseline with the 
        /// same name as the one used for inserting or updating
        /// </summary>
        /// <param name="projectId">Project Id in Guid form in which we are checking for Baseline with a specific name</param>
        /// <param name="targetBaselineName">New or updated baseline name</param>
        /// <param name="sourceBaselineName">Old baseline name. No need to pass this for checking rule against creating baseline action</param>
        /// <returns>true if baseline with same name exist in the repository otherwise false</returns>
        public async Task<bool> UniqueBaselineNameRuleAsync(System.Guid projectId, string targetBaselineName, string? sourceBaselineName = null)
        {
            if (sourceBaselineName != null)
            { // Update existing baseline name
                if (sourceBaselineName != targetBaselineName)
                { // Source and Target are different names
                    return await HasBaselineWithNameAsync(projectId, targetBaselineName);
                }
                return false;
            }
            else
            { // Create new baseline action
                return await HasBaselineWithNameAsync(projectId, targetBaselineName);
            }
        }
    }
}
