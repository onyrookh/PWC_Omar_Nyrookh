using System.Threading.Tasks;

namespace PWC.Data
{
    public static class Startup
    {
        public static void Initialize()
        {
            Repositories.EF.Base.BaseRepository.Initialize();
        }

        public static void InitializeRepositories()
        {

        }

        public static async Task CommitRepositoriesChangesAsync()
        {
            await Repositories.EF.Base.BaseRepository.CommitAsync();
        }

        public static void DisposeRepositories()
        {
            Repositories.EF.Base.BaseRepository.Dispose();
        }

    }


}
