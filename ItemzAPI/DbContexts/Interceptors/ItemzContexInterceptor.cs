using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace ItemzApp.API.DbContexts.Interceptors
{
    public class ItemzContexInterceptor : ISaveChangesInterceptor
    {
        private IList<ItemzChangeHistory> itemzChangeHistory;
        private bool executingFirstTime = true;

        // TODO: Utilizing dependency injection for Interceptor constructor is going to be 
        // useful. This will allow Interceptor itself to utilize other registered services from
        // dependency injection provider. e.g. having access to ILogger will be very useful for
        // writing debug messages in the configured logger. Also, in the current solution we are
        // configuring DbContextOptionsBuilder inside the Interceptor. By utilizing Dependency Injection
        // service provider, we will keep all the options and configuration for connecting to the 
        // database in the startup class itself.

        //public ItemzChangeHistoryContext _injectedItemzChangeHistoryContext;

        //public ItemzContexInterceptor(ItemzChangeHistoryContext Injectedcontext)
        //{
        //    _injectedItemzChangeHistoryContext = Injectedcontext ?? throw new ArgumentNullException(nameof(Injectedcontext));
        //}

        public ItemzContexInterceptor()
        {

        }
        void ISaveChangesInterceptor.SaveChangesFailed(DbContextErrorEventData eventData)
        {
           
        }

        Task ISaveChangesInterceptor.SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        int ISaveChangesInterceptor.SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            //if (executingFirstTime)
            //{
            //    //var resultOfInsertSQL = ((ItemzContext)eventData.Context).ItemzChangeHistory.FromSqlInterpolated(
            //    // $"INSERT INTO dbo.ITEMZCHANGEHISTORY ( ItemzId, CreatedDate, ChangeEvent) VALUES ('4adde56d-8081-45ea-bd37-5c830b63873b', '05-07-2019 00:00:00 +05:30', 'Modified')")
            //    //    .AsNoTracking();

            //    //var resultOfInsertSQL = ((ItemzChangeHistoryContext)eventData.Context).ItemzChangeHistory.FromSqlRaw(
            //    // "INSERT INTO ITEMZCHANGEHISTORY ( ItemzId, CreatedDate, ChangeEvent) VALUES ('4adde56d-8081-45ea-bd37-5c830b63873b', '05-07-2019 00:00:00 +05:30', 'Modified')")
            //    //    .AsNoTracking();



            //    executingFirstTime = false;
            //    //eventData.Context.SaveChanges();
            //}
            var optionsBuilder = new DbContextOptionsBuilder<ItemzChangeHistoryContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ItemzAppDB;Trusted_Connection=True;");

            //                using (ItemzChangeHistoryContext itemzChangeHistoryContext = new ItemzChangeHistoryContext(new DbContextOptions<ItemzChangeHistoryContext>()))
            using (ItemzChangeHistoryContext itemzChangeHistoryContext = new ItemzChangeHistoryContext(optionsBuilder.Options))
            {
                foreach (var ich in itemzChangeHistory)
                {
                    itemzChangeHistoryContext.Add(ich);
                }
                itemzChangeHistoryContext.SaveChanges();
                itemzChangeHistory.Clear();
            }
            return result;
        }

        async ValueTask<int> ISaveChangesInterceptor.SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken)
        {
            //foreach (var ich in itemzChangeHistory)
            //{
            //    //var returnValue = _injectedContext.ItemzChangeHistory.FromSqlInterpolated(
            //    //$"INSERT INTO \"ITEMZCHANGEHISTORY\" (\"ItemzId\", \"CreatedDate\",\"ChangeEvent\") VALUES ( \"4adde56d-8081-45ea-bd37-5c830b63873b\", \"05-07-2019 00:00:00 +05:30\", \"Modified\")" )
            //    //    .ToList();
            //    if (executingFirstTime)
            //    {
            //        //var resultOfInsertSQL = ((ItemzContext)eventData.Context).ItemzChangeHistory.FromSqlInterpolated(
            //        // $"INSERT INTO dbo.ITEMZCHANGEHISTORY ( ItemzId, CreatedDate, ChangeEvent) VALUES ('4adde56d-8081-45ea-bd37-5c830b63873b', '05-07-2019 00:00:00 +05:30', 'Modified')")
            //        //    .AsNoTracking();

            //        //var resultOfInsertSQL = ((ItemzChangeHistoryContext)eventData.Context).ItemzChangeHistory.FromSqlRaw(
            //        // "INSERT INTO ITEMZCHANGEHISTORY ( ItemzId, CreatedDate, ChangeEvent) VALUES ('4adde56d-8081-45ea-bd37-5c830b63873b', '05-07-2019 00:00:00 +05:30', 'Modified')")
            //        //    .AsNoTracking();

            //        executingFirstTime = false;
            //        //eventData.Context.SaveChanges();
            //    }
            //}
            ////await eventData.Context.SaveChangesAsync();

            var optionsBuilder = new DbContextOptionsBuilder<ItemzChangeHistoryContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ItemzAppDB;Trusted_Connection=True;");

            //                using (ItemzChangeHistoryContext itemzChangeHistoryContext = new ItemzChangeHistoryContext(new DbContextOptions<ItemzChangeHistoryContext>()))
            using (ItemzChangeHistoryContext itemzChangeHistoryContext = new ItemzChangeHistoryContext(optionsBuilder.Options))
            {
                foreach (var ich in itemzChangeHistory)
                {
                    itemzChangeHistoryContext.Add(ich);
                }
                itemzChangeHistoryContext.SaveChanges();
                itemzChangeHistory.Clear();
            }
            return result;
        }

        InterceptionResult<int> ISaveChangesInterceptor.SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            itemzChangeHistory = CreateItemzChangeHistory(eventData.Context);
            return result;
        }

        async ValueTask<InterceptionResult<int>> ISaveChangesInterceptor.SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
        {
            itemzChangeHistory = CreateItemzChangeHistory(eventData.Context);
            return result;
        }

        #region CreateAudit
        private static IList<ItemzChangeHistory> CreateItemzChangeHistory(DbContext context)
        {
            context.ChangeTracker.DetectChanges();

            IList<ItemzChangeHistory> listOfItemzChangeHistory = new List<ItemzChangeHistory>();

            foreach (var entry in context.ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
            {
                if (!entry.Entity.GetType().Equals(typeof(Itemz)))
                {
                    continue;
                }
                var itemzChangeHistory = new ItemzChangeHistory 
                    {
                    CreatedDate = DateTimeOffset.UtcNow,
                    ItemzId = (Guid)entry.Properties.Where(property => property.Metadata.IsPrimaryKey()).FirstOrDefault().CurrentValue
                    };

                switch (entry.State)
                {
                    //case EntityState.Deleted:
                    //    itemzChangeHistory.ChangeEvent = nameof(EntityState.Deleted);
                    //    itemzChangeHistory.OldValues= CreateDeletedChanges(entry);
                    //    break;
                    case EntityState.Added:
                        itemzChangeHistory.ChangeEvent = nameof(EntityState.Added);
                        itemzChangeHistory.CreatedDate = (DateTimeOffset) entry.Properties.Where(property => property.Metadata.Name == "CreatedDate").FirstOrDefault().CurrentValue;
                        itemzChangeHistory.NewValues = CreateAddedChanges(entry);
                        break;
                    case EntityState.Modified:
                        itemzChangeHistory.ChangeEvent = nameof(EntityState.Modified);
                        itemzChangeHistory.OldValues = CreateOldValueModifiedChanges(entry);
                        itemzChangeHistory.NewValues = CreateNewValueModifiedChanges(entry);
                        break;
                    default:
                        break;
                }

                listOfItemzChangeHistory.Add(itemzChangeHistory);
            }

            return listOfItemzChangeHistory;

            string CreateAddedChanges(EntityEntry entry)
                => entry.Properties.Where(property => !property.Metadata.IsPrimaryKey()
                        && property.Metadata.Name != "CreatedDate")
                   .Aggregate(
                    "",
                   // $"Inserting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => 
                            auditString + 
                            $"{property.Metadata.Name}: '{property.CurrentValue}' " + 
                            Environment.NewLine + Environment.NewLine);

            string CreateOldValueModifiedChanges(EntityEntry entry)
                => entry.Properties.Where(property => property.IsModified )
                    .Aggregate(
                    "",
                    (auditString, property) =>
                            auditString +
                            $"{property.Metadata.Name}: '{property.OriginalValue}' " +
                            Environment.NewLine + Environment.NewLine);



            string CreateNewValueModifiedChanges(EntityEntry entry)
                => entry.Properties.Where(property => property.IsModified)
                    .Aggregate(
                    "",
                    (auditString, property) => 
                            auditString + 
                            $"{property.Metadata.Name}: '{property.CurrentValue}' " + 
                            Environment.NewLine + Environment.NewLine);

            //string CreateDeletedChanges(EntityEntry entry)
            //    => entry.Properties.Where(property => !property.Metadata.IsPrimaryKey()
            //            && property.Metadata.Name != "CreatedDate"
            //            && property.Metadata.Name != "CreatedBy")
            //        .Aggregate(
            //        "",
            //        (auditString, property) => 
            //                auditString + 
            //                $"{property.Metadata.Name}: '{property.CurrentValue}' " + 
            //                Environment.NewLine + Environment.NewLine);
        }
        #endregion

    }
}
