using PWC.Infrastructure;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace PWC.Repositories.EF.Base
{

  /// <summary>
  /// Creates new instances of an EF unit of Work.
  /// </summary>
  public class EFUnitOfWorkFactory : IUnitOfWorkFactory
  {
      private static Func<WebsiteContext> _dbContextDelegate;
      private static readonly Object _lockObject = new object();

      public static void SetDbContext(Func<WebsiteContext> dbContextDelegate)
      {
          _dbContextDelegate = dbContextDelegate;
      }

      public IUnitOfWork Create()
      {
          WebsiteContext context;

          lock (_lockObject)
          {
              context = _dbContextDelegate();
          }

          return new EFUnitOfWork(context);
      }
  }
}
