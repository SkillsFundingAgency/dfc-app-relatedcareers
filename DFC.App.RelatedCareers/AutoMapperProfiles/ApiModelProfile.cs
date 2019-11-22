using AutoMapper;
using DFC.App.RelatedCareers.ApiModels;
using DFC.App.RelatedCareers.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.RelatedCareers.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            CreateMap<RelatedCareerDataModel, RelatedCareerApiModel>()
                .ForMember(d => d.Url, s => s.MapFrom(a => a.ProfileLink))
                ;
        }
    }
}