using PWC.Data.Model.POCOs;
using PWC.Entities.DTOs;
using PWC.Entities.VMs.UI.Thesis;
using AutoMapper;

namespace PWC.Business.Helpers
{
    public static class AutoMapperHelper
    {
        public static void InitializeAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<ComplaintProfile>();

            });

            Mapper.AssertConfigurationIsValid();
        }
    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllOtherMembers(opt => opt.Ignore());
            return expression;
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, AccountSessionInfoDto>().IgnoreAllUnmapped()
                .ForMember(d => d.AccountID, o => o.MapFrom(s => s.UserID))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.UserTypeID, o => o.MapFrom(s => s.UserTypeID));
        }
    }

    public class ComplaintProfile : Profile
    {
        public ComplaintProfile()
        {
            CreateMap<Complaint, ComplaintVM>().IgnoreAllUnmapped()
                .ForMember(d => d.ComplaintID, o => o.MapFrom(s => s.ComplaintID))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User.Username))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.ComplaintStatu.Name))
                .ForMember(d => d.StatusID, o => o.MapFrom(s => s.StatusID))
                .ForMember(d => d.Message, o => o.MapFrom(s => s.Message));
        }
    }

}
