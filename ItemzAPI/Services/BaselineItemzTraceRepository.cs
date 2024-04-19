// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ItemzApp.API.Services
{
    public class BaselineItemzTraceRepository : IBaselineItemzTraceRepository, IDisposable
    {
        private readonly BaselineContext _baselineContext;
        private readonly BaselineItemzTraceContext _baselineItemzTraceContext;
        private readonly IPropertyMappingService _propertyMappingService;
        public BaselineItemzTraceRepository(BaselineContext baselineContext,
                                        BaselineItemzTraceContext baselineItemzTraceContext,
                                        IPropertyMappingService propertyMappingService)
        {
            _baselineContext = baselineContext ?? throw new ArgumentNullException(nameof(baselineContext));
            _baselineItemzTraceContext = baselineItemzTraceContext ?? throw new ArgumentNullException(nameof(baselineItemzTraceContext));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }


        //public async Task<bool> SaveAsync() // This method is not expected to be called because Baseline Itemz Traces are managed via Stored Procedure
        //{
        //    return (await _baselineItemzTraceContext.SaveChangesAsync() >= 0);
        //}

        public async Task<IEnumerable<BaselineItemzJoinItemzTrace>> GetAllTracesByBaselineItemzIdAsync(Guid baselineItemzId)
        {
            if (baselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzId));
            }

            return await _baselineItemzTraceContext.BaselineItemzJoinItemzTrace!
                .Where(bijit => bijit.BaselineFromItemzId == baselineItemzId || bijit.BaselineToItemzId == baselineItemzId).AsNoTracking().ToListAsync();
        }

        public async Task<BaselineItemzParentAndChildTraceDTO> GetAllParentAndChildTracesByBaselineItemzIdAsync(Guid baselineItemzId)
        {
            if (baselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzId));
            }

            var baselineItemzParentAndChildTraceDTO = new BaselineItemzParentAndChildTraceDTO();

            baselineItemzParentAndChildTraceDTO.BaselineItemz = new();
            baselineItemzParentAndChildTraceDTO.BaselineItemz.ChildBaselineItemz = new();
            baselineItemzParentAndChildTraceDTO.BaselineItemz.ParentBaselineItemz = new();
            baselineItemzParentAndChildTraceDTO.BaselineItemz.ID = baselineItemzId;

            var allChildTraceBaselineItemzs = _baselineItemzTraceContext.BaselineItemzJoinItemzTrace!
                .Where(bijit => bijit.BaselineFromItemzId == baselineItemzId);

            foreach (var childTraceBaselineItemz in allChildTraceBaselineItemzs)
            {
                var tempChildTraceBaselineItemzDTO = new ChildTraceBaselineItemz__DTO();
                tempChildTraceBaselineItemzDTO.BaselineItemzID = childTraceBaselineItemz.BaselineToItemzId;
                baselineItemzParentAndChildTraceDTO.BaselineItemz.ChildBaselineItemz.Add(tempChildTraceBaselineItemzDTO);
            }

            var allParentTraceBaselineItemzs = _baselineItemzTraceContext.BaselineItemzJoinItemzTrace!
                .Where(bijit => bijit.BaselineToItemzId == baselineItemzId);

            foreach (var parentTraceBaselineItemz in allParentTraceBaselineItemzs)
            {
                var tempParentTraceBaselineItemzDTO = new ParentTraceBaselineItemz__DTO();
                tempParentTraceBaselineItemzDTO.BaselineItemzID = parentTraceBaselineItemz.BaselineFromItemzId;
                baselineItemzParentAndChildTraceDTO.BaselineItemz.ParentBaselineItemz.Add(tempParentTraceBaselineItemzDTO);
            }

            return baselineItemzParentAndChildTraceDTO;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        /// <summary>
        /// Purpose of this method is to check if Baseline Trace is already found 
        /// between FromBaselineItemz and ToBaselineItemz
        /// </summary>
        /// <param name="baselineItemzTraceDTO"></param>
        /// <returns></returns>

        public async Task<bool> BaselineItemzsTraceExistsAsync(BaselineItemzTraceDTO baselineItemzTraceDTO)
        {
            if (baselineItemzTraceDTO.FromTraceBaselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzTraceDTO.FromTraceBaselineItemzId));
            }

            if (baselineItemzTraceDTO.ToTraceBaselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzTraceDTO.ToTraceBaselineItemzId));
            }

            return await _baselineItemzTraceContext.BaselineItemzJoinItemzTrace
                            .AsNoTracking()
                            .AnyAsync(bijit => bijit.BaselineFromItemzId == baselineItemzTraceDTO.FromTraceBaselineItemzId
                                        && bijit.BaselineToItemzId == baselineItemzTraceDTO.ToTraceBaselineItemzId);
        }

        public async Task<bool> BaselineItemzExistsAsync(Guid baselineItemzId)
        {
            if (baselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzId));
            }

            // EXPLANATION: We expect ItemzExists to be used independently on it's own without
            // expecting it to track the itemz that was found in the database. That's why it's not
            // a good idea to use "!(_context.Itemzs.Find(itemzId) == null)" option
            // to "Find()" Itemz. This is because Find is designed to track the itemz in the memory.
            // In "Itemz Delete controller method", we are first checking if ItemzExists and then 
            // we call Itemz Delete to actually remove it. This is going to be in the single scoped
            // DBContext. If we use "Find()" method then it will start tracking the itemz and then we can't
            // get the itemz once again from the DB as it's already being tracked. We have a choice here
            // to decide if we should always use Find via ItemzExists and then yet again in the subsequent
            // operations like Delete / Update or we use ItemzExists as independent method and not rely on 
            // it for subsequent operations like Delete / Update.

            return await _baselineContext.BaselineItemz.AsNoTracking().AnyAsync(a => a.Id == baselineItemzId);
            // return  !(_baselineContext.BaselineItemz.Find(baselineItemzId) == null);
        }

        public async Task<int> GetFromTraceCountByBaselineItemz(Guid baselineItemzId)
        {
            return await _baselineItemzTraceContext.BaselineItemzJoinItemzTrace
                .Include(bijit => bijit.BaselineFromItemz)
                .Where(bijit => bijit.BaselineToItemzId == baselineItemzId).CountAsync();
        }

        public async Task<int> GetToTraceCountByBaselineItemz(Guid baselineItemzId)
        {
            return await _baselineItemzTraceContext.BaselineItemzJoinItemzTrace
                .Include(bijit => bijit.BaselineToItemz)
                .Where(bijit => bijit.BaselineFromItemzId == baselineItemzId).CountAsync();
        }

        public async Task<int> GetAllFromAndToTracesCountByBaselineItemzIdAsync(Guid baselineItemzId)
        {
            if (baselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzId));
            }

            return await _baselineItemzTraceContext.BaselineItemzJoinItemzTrace!
                    .Where(bijit => bijit.BaselineFromItemzId == baselineItemzId || bijit.BaselineToItemzId == baselineItemzId).CountAsync();
        }
    }
}
