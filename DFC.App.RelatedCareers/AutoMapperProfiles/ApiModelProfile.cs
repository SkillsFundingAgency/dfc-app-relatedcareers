using AutoMapper;
using DFC.App.RelatedCareers.ApiModels;
using DFC.App.RelatedCareers.Data.Models;

namespace DFC.App.RelatedCareers.AutoMapperProfiles
{
    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            CreateMap<RelatedCareerDataModel, RelatedCareerApiModel>()
                .ForMember(d => d.Url, s => s.MapFrom(a => $"https://api.nationalcareers.service.gov.uk/job-profiles/{a.CanonicalName}"))
                ;
        }
    }
}