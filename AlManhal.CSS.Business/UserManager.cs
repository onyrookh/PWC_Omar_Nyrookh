using PWC.Common.Helpers;
using PWC.Data.Base;
using PWC.Data.Model.POCOs;
using PWC.Entities.DTOs;
using PWC.Infrastructure;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PWC.Business
{
    public static class UserManager
    {
        private static ILogger log = ApplicationLogging.CreateLogger("UserManager");

        #region Mapper
        internal static IMapper<User, int> _Mapper
        {
            get
            {
                return EFBaseMapper.ObjectFactory.GetInstance<IMapper<User, int>>();
            }
        }
        #endregion

        public static async Task<ResponseEntity<AccountSessionInfoDto>> GetAccountForLogin(string username, string password)
        {
            ResponseEntity<AccountSessionInfoDto> oResponseEntity = new ResponseEntity<AccountSessionInfoDto>();
            oResponseEntity.Status = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                User oUser = await _Mapper.FindSingleAsync(s => s.Username.ToLower() == username && s.Password == password);

                if (oUser != null)
                {
                        oResponseEntity.Data = Mapper.Map<AccountSessionInfoDto>(oUser);
                }
                else
                {
                    oResponseEntity.Data = null;
                    oResponseEntity.Status = new Error(Error.ErrorCodeEnum.InvalidLoginIDorPassword, new Exception("Invalid Username and/or Password"));
                }
            }
            catch (Exception ex)
            {
                oResponseEntity.Data = null;
                oResponseEntity.Status = new Error(Error.ErrorCodeEnum.InternalSystemError, ex);
                log.LogTrace(ex, "Exception on GetAccountForLogin Method: ");
            }
            return oResponseEntity;
        }

        public static Error AddUser(string username, string password, int userTypeID)
        {
            Error oError = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                if (!_Mapper.Any(s=>s.Username.ToLower().Trim() == username.ToLower().Trim()))
                {
                    User User = new User
                    {
                        Username = username.Trim(),
                        Password = password.Trim(),
                        UserTypeID = userTypeID
                    };
                    oError= _Mapper.Save(User, true);

                }
                else
                {
                    oError = new Error(Error.ErrorCodeEnum.Duplication);
                }
            }
            catch (Exception)
            {
            }

            return oError;
        }
    }
}
