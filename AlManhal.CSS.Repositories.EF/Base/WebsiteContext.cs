using PWC.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Repositories.EF.Base
{
    /// <summary>
    /// This is the main DbContext to work with data in the database.
    /// </summary>
    public class WebsiteContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the TeamManagerContext class.
        /// </summary>
        public WebsiteContext()
            : base("DBEntities")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Hooks into the Save process to get a last-minute chance to look at the entities and change them. Also intercepts exceptions and 
        /// wraps them in a new Exception type.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        public override async Task<int> SaveChangesAsync()
        {
            // Need to manually delete all "owned objects" that have been removed from their owner, otherwise they'll be orphaned.
            var orphanedObjects = ChangeTracker.Entries().Where(
              e => (e.State == EntityState.Modified || e.State == EntityState.Added) &&
                e.Entity.GetType().GetInterfaces().Any(x => x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IHasOwner<>)) &&
                e.Reference("Owner").CurrentValue == null);

            foreach (var orphanedObject in orphanedObjects)
            {
                orphanedObject.State = EntityState.Deleted;
            }

            try
            {
                var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
                foreach (DbEntityEntry item in modified)
                {
                    var changedOrAddedItem = item.Entity as IDateTracking;
                    if (changedOrAddedItem != null)
                    {
                        if (item.State == EntityState.Added)
                        {
                            changedOrAddedItem.CreationDate = DateTime.Now;
                        }
                        changedOrAddedItem.ModificationDate = DateTime.Now;
                    }
                }
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                var result = new StringBuilder();
                var allErrors = new List<ValidationResult>();
                foreach (var error in errors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        result.AppendFormat("\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n", error.Entry.Entity.GetType().ToString(), validationError.ErrorMessage, validationError.PropertyName);
                        var domainEntity = error.Entry.Entity as DomainEntity<int>;
                        if (domainEntity != null)
                        {
                            result.Append(domainEntity.IsTransient() ? "  This entity was added in this session.\r\n" : string.Format("  The Id of the entity is {0}.\r\n", domainEntity.Id));
                        }
                        allErrors.Add(new ValidationResult(validationError.ErrorMessage, new[] { validationError.PropertyName }));
                    }
                }
                throw new ModelValidationException(result.ToString(), entityException, allErrors);
            }
        }
    }
}
