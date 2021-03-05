using System;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace ItemzApp.API.DbContexts.Interceptors
{
    public class ItemzContexInterceptor : ISaveChangesInterceptor
    {
        void ISaveChangesInterceptor.SaveChangesFailed(DbContextErrorEventData eventData)
        {
           
        }

        Task ISaveChangesInterceptor.SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        int ISaveChangesInterceptor.SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return result;
        }

        async ValueTask<int> ISaveChangesInterceptor.SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken)
        {
            return result;
        }

        InterceptionResult<int> ISaveChangesInterceptor.SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {

            return result;
        }

        async ValueTask<InterceptionResult<int>> ISaveChangesInterceptor.SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
        {
            return result;
        }
    }
}
