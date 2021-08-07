using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using PWC.Business;
using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using PWC.Entities.Filters;
using PWC.Entities.VMs;
using PWC.Entities.VMs.UI.Thesis;
using PWC.Infrastructure;
using PWC.UI.Filters;
using PWC.UI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PWC.UI.Controllers.api
{
    [Produces("application/json")]
    [NoCache]
    public class ComplaintApiController : Base.BaseApiController
    {
        #region Log

        private static ILogger log = ApplicationLogging.CreateLogger("ComplaintApiController");

        #endregion

        #region Actions

        [HttpPost]
        public ResponseEntity<PagerEntity<ComplaintVM>> GetComplaintList([FromBody] JObject filters)
        {
            ResponseEntity<PagerEntity<ComplaintVM>> oResponseEntity = new ResponseEntity<PagerEntity<ComplaintVM>>();
            oResponseEntity.Status = new Error(Error.ErrorCodeEnum.Success);
            oResponseEntity.Data = new PagerEntity<ComplaintVM>();
            oResponseEntity.Data.Data = new List<ComplaintVM>();
            try
            {
                AccountSessionInfoDto oAccountSessionInfoDto = AuthenticationHelper.GetAuthenticatedAccount();
                if (oAccountSessionInfoDto != null)
                {
                    ThesisListFilter oThesisListFilter = JsonConvert.DeserializeObject<ThesisListFilter>(filters.ToString());
                    oResponseEntity.Data = new PagerEntity<ComplaintVM>(oThesisListFilter.PageSize, oThesisListFilter.PageNumber);
                    string sortingCriteria = oThesisListFilter.FieldSort.Any() ? string.Format("{0} {1}", oThesisListFilter.FieldSort.First().field, oThesisListFilter.FieldSort.First().dir) : string.Empty;
                    PagingCriteria oPagingCriteria = new PagingCriteria(oThesisListFilter.PageNumber, oThesisListFilter.PageSize, sortingCriteria);
                    
                    oResponseEntity = ComplaintManager.GetComplaintList(oPagingCriteria, oThesisListFilter, (oAccountSessionInfoDto.UserTypeID == (int)Enums.UserType.Customer? oAccountSessionInfoDto.AccountID:0));
                }
                else
                {
                    oResponseEntity.Status = new Error(Error.ErrorCodeEnum.InvalidAuthentication);
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on GetComplaintList Method");
                oResponseEntity.Status = new Error(Error.ErrorCodeEnum.InternalSystemError, ex);
            }

            return oResponseEntity;
        }

        [HttpPost]
        public Error AddEditComplaint([FromBody] JObject data)
        {
            Error oError = new Error(Error.ErrorCodeEnum.InvalidAuthentication);

            try
            {
                AccountSessionInfoDto oAccountSessionInfoDto = AuthenticationHelper.GetAuthenticatedAccount();
                if (oAccountSessionInfoDto != null && oAccountSessionInfoDto.UserTypeID >0)
                {
                    ComplaintDto oComplaintDto = JsonConvert.DeserializeObject<ComplaintDto>(data.ToString());
                    if (oComplaintDto != null)
                    {
                        oError = ComplaintManager.AddEditComplaint(oComplaintDto, oAccountSessionInfoDto.AccountID, oAccountSessionInfoDto.UserTypeID); 
                    }
                    else
                    {
                        oError = new Error(Error.ErrorCodeEnum.NoRecordsFound, new Exception(Resources.UI.ReviewThesis.Review.PleaseFillAllRequiredFields));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on AddEditComplaint Method");
            }

            return oError;
        }
        #endregion
    }
}