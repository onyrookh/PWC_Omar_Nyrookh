using PWC.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Business
{
    public static class Startup
    {
        public static void InitializeUI()
        {
            Data.Startup.Initialize();
            AutoMapperHelper.InitializeAutoMapper();
        }

        public static async Task CommitRepositoriesChanges()
        {
            await Data.Startup.CommitRepositoriesChangesAsync();
        }

        public static void DisposeRepositories()
        {
            Data.Startup.DisposeRepositories();
        }

        public static void DisposeRabbitMQBuses()
        {

        }
    }
}
