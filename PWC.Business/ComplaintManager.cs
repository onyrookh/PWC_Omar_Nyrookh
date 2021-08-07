using PWC.Data.Base;
using PWC.Data.Model.POCOs;
using PWC.Entities.DTOs;
using PWC.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PWC.Common.Helpers;
using PWC.Entities;
using System.IO;
using System.Configuration;
using PWC.Entities.Filters;
using PWC.Entities.VMs.UI.Thesis;
using System.Linq.Expressions;

namespace PWC.Business
{
    public static class ComplaintManager
    {
        private static ILogger log = ApplicationLogging.CreateLogger("ComplaintManager");

        #region Mapper
        internal static IMapper<Complaint, int> _Mapper
        {
            get
            {
                return EFBaseMapper.ObjectFactory.GetInstance<IMapper<Complaint, int>>();
            }
        }
        #endregion

        public static Error AddEditComplaint(ComplaintDto oComplaintDto, int accountID, int userTypeID)
        {
            Error oError = new Error(Error.ErrorCodeEnum.Success);
            try
            {
                var compliant = _Mapper.FindSingle(s => s.ComplaintID == oComplaintDto.ComplaintID);
                if (compliant != null)
                {
                    if (userTypeID == (int)Enums.UserType.Admin)
                    {
                        compliant.StatusID = oComplaintDto.StatusID;
                    }
                    else if (userTypeID == (int)Enums.UserType.Customer)
                    {
                        compliant.Message = oComplaintDto.Message;
                        compliant.StatusID = (int)Enums.ComplaintStatus.Pending;

                    }
                    oError = _Mapper.Save(compliant, false);
                }
                else
                {
                    Complaint oComplaint = new Complaint
                    {
                        StatusID = (int)Enums.ComplaintStatus.Pending,
                        Message = oComplaintDto.Message,
                        UserID = accountID
                    };
                    oError = _Mapper.Save(oComplaint, true);
                }

            }
            catch (Exception)
            {
            }
            return oError;
        }
        public static ComplaintVM GetComplaintByID(int? complaintID)
        {
            ComplaintVM oComplaintVM = new ComplaintVM();
            try
            {
                var oComplaint = _Mapper.FindSingle(s => s.ComplaintID == complaintID);
                oComplaintVM = Mapper.Map<ComplaintVM>(oComplaint);

            }
            catch (Exception)
            {
            }
            return oComplaintVM;
        }
        public static ResponseEntity<PagerEntity<ComplaintVM>> GetComplaintList(PagingCriteria oPagingCriteria, ThesisListFilter oThesisListFilter, int accountid)
        {
            ResponseEntity<PagerEntity<ComplaintVM>> oResponseEntity = new ResponseEntity<PagerEntity<ComplaintVM>>();
            oResponseEntity.Status = new Error(Error.ErrorCodeEnum.Success);
            oResponseEntity.Data = new PagerEntity<ComplaintVM>(oPagingCriteria.PageSize, oPagingCriteria.PageNo);
            oResponseEntity.Data.Data = new List<ComplaintVM>();
            int totalCount = 0;
            try
            {
                Expression<Func<Complaint, bool>> oWhere = s => true;

                if (accountid > 0)
                {
                    oWhere=oWhere.And(s => s.UserID == accountid);
                }

                IEnumerable<Complaint> oComplaint = _Mapper.Find(oWhere, oPagingCriteria, out totalCount, s => s.ComplaintStatu, s => s.User);

                if (oComplaint != null && oComplaint.Count() > 0)
                {
                    oResponseEntity.Data.Data = Mapper.Map<List<ComplaintVM>>(oComplaint);
                    oResponseEntity.Data.TotalRows = totalCount;
                    oResponseEntity.Data.PageNumber = oPagingCriteria.PageNo;
                    oResponseEntity.Data.PageSize = oPagingCriteria.PageSize;
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on GetComplaintList Method: ");
                oResponseEntity.Status = new Error(Error.ErrorCodeEnum.InternalSystemError, ex);
            }
            return oResponseEntity;
        }

    }
}
