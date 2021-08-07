using System;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    /// <summary>
    /// Represents a unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits the changes to the underlying data store. 
        /// </summary>
        /// <param name="resetAfterCommit">When true, all the previously retrieved objects should be cleared from the underlying model / cache.</param>
        Task CommitAsync();
    }
}
