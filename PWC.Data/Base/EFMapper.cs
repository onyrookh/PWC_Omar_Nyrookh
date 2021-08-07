using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using PWC.Infrastructure;
using PWC.Repositories.EF.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace PWC.Data.Base
{
    public class EFMapper<T, W> : IMapper<T, W> where T : DomainEntity<W>
    {
        private static IRepository<T, W> _Repository
        {
            get
            {
                return BaseRepository.ObjectFactory.GetInstance<IRepository<T, W>>();
            }
        }

        public virtual IQueryable<T> FindAll(PagingCriteria pagingCriteria, out int totalCount, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> oList = null;
            totalCount = _Repository.Count(t => true);
            if (totalCount > 0)
            {
                oList = _Repository.Find(pagingCriteria.CountToSkip, pagingCriteria.CountToTake, pagingCriteria.OrderSelector, t => true, includeProperties);
            }

            return oList;
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return _Repository.Find(predicate, includeProperties);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate,string sort, params Expression<Func<T, object>>[] includeProperties)
        {
            return _Repository.Find(predicate,sort, includeProperties);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, out int totalCount, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> oList = null;

            totalCount = _Repository.Count(predicate);
            if (totalCount > 0)
            {
                oList = _Repository.Find(predicate, includeProperties);
            }

            return oList;
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, PagingCriteria pagingCriteria, out int totalCount, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> oList = null;

            totalCount = _Repository.Count(predicate);
            if (totalCount > 0)
            {
                oList = _Repository.Find(pagingCriteria.CountToSkip, pagingCriteria.CountToTake, pagingCriteria.OrderSelector, predicate, includeProperties
                );
            }

            return oList;
        }

        public virtual T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return _Repository.FindSingle(predicate, includeProperties);
        }

        public virtual async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _Repository.FindSingleAsync(predicate, includeProperties);
        }

        public virtual T FindFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return _Repository.FindFirst(predicate, includeProperties);
        }

        public virtual async Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _Repository.FindFirstAsync(predicate, includeProperties);
        }

        public virtual Error Save(T oEntity, bool isNew = false)
        {
            Error oError = new Error();
            try
            {
                IUserTracking oUserTrackingEntity = oEntity as IUserTracking;
                IDateTracking oDateTrackingEntity = oEntity as IDateTracking;

                if (oUserTrackingEntity != null)
                {
                    AccountSessionInfoDto oCustomerAccountSessionInfo = null;
                    Microsoft.AspNetCore.Http.HttpContext ctx = SessionHelper.GetHttpContext();

                    oCustomerAccountSessionInfo = ctx.Session.GetParameter<AccountSessionInfoDto>(Enums.SessionVariablesKeys.UserInfo.ToString());

                    if (isNew)
                    {
                        oUserTrackingEntity.CreatedByID = oCustomerAccountSessionInfo.AccountID;
                        oUserTrackingEntity.ModifiedByID = oCustomerAccountSessionInfo.AccountID;

                    }

                    oUserTrackingEntity.ModifiedByID = oCustomerAccountSessionInfo.AccountID;

                }
                if (oDateTrackingEntity != null)
                {
                    if (isNew)
                        oDateTrackingEntity.CreationDate = DateTime.Now;

                    if (oDateTrackingEntity != null)
                    {
                        if (isNew)
                            oDateTrackingEntity.CreationDate = DateTime.Now;

                        oDateTrackingEntity.ModificationDate = DateTime.Now;
                    }

                    oDateTrackingEntity.ModificationDate = DateTime.Now;
                }


                if (isNew)
                {
                    _Repository.Add(oEntity);
                }

                var validationResult = oEntity.Validate();

                if (validationResult != null && validationResult.Any())
                {
                    oError.SetError(Error.ErrorCodeEnum.InvalidObjectState, string.Join("\n", validationResult.Select(v => v.ErrorMessage + ": " + string.Join(", ", v.MemberNames)).ToList()));
                }
                else
                {
                    _Repository.SaveChanges();
                    oError.SetError(Error.ErrorCodeEnum.Success);
                }
            }
            catch (Exception ex)
            {
                oError.SetError(Error.ErrorCodeEnum.InternalSystemError, ex);
            }

            return oError;
        }

        public virtual async Task<Error> SaveAsync(T oEntity, bool isNew = false)
        {
            Error oError = new Error();
            try
            {
                IUserTracking oUserTrackingEntity = oEntity as IUserTracking;

                if (oUserTrackingEntity != null)
                {
                    AccountSessionInfoDto oCurrentUser = null;
                    Microsoft.AspNetCore.Http.HttpContext ctx = SessionHelper.GetHttpContext();
                    if (ctx.Session != null)
                    {
                        oCurrentUser = ctx.Session.GetParameter<AccountSessionInfoDto>(Enums.SessionVariablesKeys.UserInfo.ToString());
                    }

                    if (oCurrentUser != null)
                    {
                        if (isNew)
                        {
                            oUserTrackingEntity.CreatedByID = oCurrentUser.AccountID;
                            oUserTrackingEntity.ModifiedByID = oCurrentUser.AccountID;
                        }
                        else
                        {
                            oUserTrackingEntity.ModifiedByID = oCurrentUser.AccountID;
                        }
                    }
                }

                if (isNew)
                {
                    _Repository.Add(oEntity);
                }

                var validationResult = oEntity.Validate();

                if (validationResult != null && validationResult.Any())
                {
                    oError.SetError(Error.ErrorCodeEnum.InvalidObjectState, string.Join("\n", validationResult.Select(v => v.ErrorMessage + ": " + string.Join(", ", v.MemberNames)).ToList()));
                }
                else
                {
                    await _Repository.SaveChangesAsync();
                    oError.SetError(Error.ErrorCodeEnum.Success);
                }
            }
            catch (Exception ex)
            {
                oError.SetError(Error.ErrorCodeEnum.InternalSystemError, ex);
            }

            return oError;
        }

        public virtual Error Delete(T oEntity)
        {
            Error oError = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                _Repository.Remove(oEntity);
                _Repository.SaveChanges();
            }
            catch (Exception ex)
            {
                oError.SetError(Error.ErrorCodeEnum.InternalSystemError, ex);
            }

            return oError;
        }

        public virtual async Task<Error> DeleteAsync(T oEntity)
        {
            Error oError = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                _Repository.Remove(oEntity);
                await _Repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                oError.SetError(Error.ErrorCodeEnum.InternalSystemError, ex);
            }

            return oError;
        }

        public virtual async Task<Error> ObsoleteAsync(T oEntity, DateTime? ObsoletionDate = null)
        {
            Error oError = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                IObsoletable oDateTrackingEntity = oEntity as IObsoletable;

                if (oDateTrackingEntity != null)
                {
                    oDateTrackingEntity.ObsoletionDate = ObsoletionDate.HasValue ? ObsoletionDate : DateTime.Now;
                    await _Repository.SaveChangesAsync();
                }
                else
                {
                    oError.SetError(Error.ErrorCodeEnum.InternalSystemError);
                }
            }
            catch (Exception ex)
            {
                oError.SetError(Error.ErrorCodeEnum.InternalSystemError, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return oError;
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _Repository.CountAsync(predicate);
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _Repository.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _Repository.AnyAsync(predicate);
        }

        public virtual async Task<M> MaxAsync<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector)
        {
            return await _Repository.MaxAsync(predicate, selector);
        }
        public virtual M Max<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector)
        {
            return _Repository.Max(predicate, selector);
        }
        public virtual async Task<M> MinAsync<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector)
        {
            return await _Repository.MinAsync(predicate, selector);
        }

    }
}
