using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    public class Error
    {
        #region Properties
        public int ID { get; set; }
        public int RowID { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionMessage { set; get; }

        public bool IsSuccess
        {
            get
            {
                return (ID == (int)ErrorCodeEnum.Success);
            }
        }

        public bool IsDuplication
        {
            get
            {
                return (ID == (int)ErrorCodeEnum.Duplication);
            }
        }

        public bool IsInvalidLoginIDorPassword
        {
            get
            {
                return (ID == (int)ErrorCodeEnum.InvalidLoginIDorPassword);
            }
        }

        #endregion

        #region Enums

        public enum ErrorCodeEnum : int
        {
            [Description("Success")]
            Success = 0,
            [Description("Internal System Error")]
            InternalSystemError = -101,
            [Description("Duplication")]
            Duplication = -102,
            [Description("Record(s) does not exist(s)")]
            RecordsDoesNotExist = -103,
            [Description("No Records Found")]
            NoRecordsFound = -104,
            [Description("Invalid LoginID or Password")]
            InvalidLoginIDorPassword = -105,
            [Description("Account is Inactive")]
            AccountIsInactive = -106,
            [Description("Record Has Related Data")]
            RecordHasRelatedData = -107,
            [Description("Authentication Error")]
            InvalidAuthentication = -108,
            [Description("Invalid Object State")]
            InvalidObjectState = -109,
            [Description("Invalid Email")]
            InvalidEmail= -110,
            [Description("Subscription Expired")]
            SubscriptionExpired = -111,
        }

        #endregion

        #region Constructors

        public Error()
        {
            ID = -1;
            RowID = -1;
        }

        public Error(ErrorCodeEnum errCode)
        {
            SetError(errCode);
        }

        public Error(ErrorCodeEnum errCode, string exceptionMsg = null)
        {
            SetError(errCode, exceptionMsg);
        }

        public Error(ErrorCodeEnum errCode, Exception exception)
        {
            SetError(errCode, exception);
        }

        #endregion

        #region Methods

        public void SetError(ErrorCodeEnum errCode, Exception exception)
        {
            ID = (int)errCode;
            ErrorMessage = exception.InnerException != null ? (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) : (exception.Message);
            ExceptionMessage = exception.InnerException != null ? (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) : (exception.Message);
        }

        public void SetError(ErrorCodeEnum errCode, string exceptionMsg = null)
        {
            ID = (int)errCode;
            ErrorMessage = errCode.GetDescription();
            ExceptionMessage = exceptionMsg;
        }

        public void SetError(ErrorCodeEnum errCode, int rowID, string exceptionMsg = null)
        {
            ID = (int)errCode;
            ErrorMessage = errCode.GetDescription();
            RowID = rowID;
            ExceptionMessage = exceptionMsg;
        }

        public static implicit operator Task<object>(Error v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
