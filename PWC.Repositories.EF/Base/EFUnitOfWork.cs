using PWC.Common.Helpers;
using PWC.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace PWC.Repositories.EF.Base
{
    /// <summary>
    /// Defines a Unit of Work using an EF DbContext under the hood.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private static ILogger log = ApplicationLogging.CreateLogger("EFUnitOfWork");

        public WebsiteContext Context { get; private set; }


        /// <summary>
        /// Initializes a new instance of the EFUnitOfWork class.
        /// </summary>
        /// <param name="forceNewContext">When true, clears out any existing data context first.</param>
        public EFUnitOfWork(WebsiteContext context)
        {
            Context = context;
            context.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Saves the changes to the underlying DbContext.
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Saves the changes to the underlying DbContext.
        /// </summary>
        /// <param name="resetAfterCommit">When true, clears out the data context afterwards.</param>
        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves the changes to the underlying DbContext.
        /// </summary>
        /// <param name="resetAfterCommit">When true, clears out the data context afterwards.</param>
        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
