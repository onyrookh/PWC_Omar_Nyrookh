using PWC.Infrastructure;
using StructureMap;
using StructureMap.Graph;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

namespace PWC.Repositories.EF.Base
{
    public static class BaseRepository
    {
        public static Container ObjectFactory;

        static BaseRepository()
        {
            ObjectFactory = new Container(scanner =>
            {
                scanner.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                });
                scanner.For<IUnitOfWorkFactory>().Use<EFUnitOfWorkFactory>();
                scanner.For(typeof(IRepository<,>)).Use(typeof(Repository<,>));
                //                x.For<IExample>().Use<Example>();
            });
        }

        public static void Initialize()
        {
            EFUnitOfWorkFactory.SetDbContext(() => new WebsiteContext());
        }

        public static async Task CommitAsync()
        {
            await UnitOfWork.CommitAsync();
        }

        public static void Dispose()
        {
            UnitOfWork.Current.Dispose();
        }
    }
}
