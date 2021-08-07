using PWC.Common.Helpers;
using PWC.Infrastructure;
using Microsoft.AspNetCore.Http;
using StructureMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PWC.Repositories.EF.Base
{
    public static class UnitOfWork
    {
        private const string HTTPCONTEXTKEY = "PWC.Repositories.Base.HttpContext.Key";

        private static IUnitOfWorkFactory _unitOfWorkFactory;
        private static readonly Hashtable _threads = new Hashtable();

        public static async Task CommitAsync()
        {
            IUnitOfWork unitOfWork = GetUnitOfWork();

            if (unitOfWork != null)
            {
                await unitOfWork.CommitAsync();
            }
        }

        public static IUnitOfWork Current
        {
            get
            {
                IUnitOfWork unitOfWork = GetUnitOfWork();

                if (unitOfWork == null)
                {
                    _unitOfWorkFactory = BaseRepository.ObjectFactory.GetInstance<IUnitOfWorkFactory>();
                    unitOfWork = _unitOfWorkFactory.Create();
                    SaveUnitOfWork(unitOfWork);
                }

                return unitOfWork;
            }
        }

        private static IUnitOfWork GetUnitOfWork()
        {
            HttpContext ctx = SessionHelper.GetHttpContext();
            if (ctx != null)
            {
                if (ctx.Items.Keys.Contains(HTTPCONTEXTKEY))
                {
                    return (IUnitOfWork)ctx.Items[HTTPCONTEXTKEY];
                }

                return null;
            }
            else
            {
                Thread thread = Thread.CurrentThread;
                if (string.IsNullOrEmpty(thread.Name))
                {
                    thread.Name = Guid.NewGuid().ToString();
                    return null;
                }
                else
                {
                    lock (_threads.SyncRoot)
                    {
                        return (IUnitOfWork)_threads[Thread.CurrentThread.Name];
                    }
                }
            }
        }

        private static void SaveUnitOfWork(IUnitOfWork unitOfWork)
        {
            HttpContext ctx = SessionHelper.GetHttpContext();
            if (ctx != null)
            {
                ctx.Items[HTTPCONTEXTKEY] = unitOfWork;
            }
            else
            {
                lock (_threads.SyncRoot)
                {
                    _threads[Thread.CurrentThread.Name] = unitOfWork;
                }
            }
        }
    }
}
