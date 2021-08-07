using PWC.Infrastructure;
using StructureMap;

namespace PWC.Data.Base
{
    public static class EFBaseMapper
    {
        public static Container ObjectFactory;

        static EFBaseMapper()
        {
            ObjectFactory = new Container(scanner =>
            {
                scanner.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                });
                scanner.For(typeof(IMapper<,>)).Use(typeof(EFMapper<,>));
                //                x.For<IExample>().Use<Example>();
            });
        }
    }
}
