using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.Models.PatchModels;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.Data.ServiceBusModels.PatchModels;

namespace DFC.App.RelatedCareers.MessageFunctionApp.AutomapperProfile
{
    public class RelatedCareerProfile : Profile
    {
        public RelatedCareerProfile()
        {
            CreateMap<JobProfileMessage, RelatedCareersSegmentModel>()
                .ForMember(d => d.Data, s => s.MapFrom(a => a))
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.JobProfileId))
                .ForMember(d => d.SocLevelTwo, s => s.MapFrom(a => a.SocCodeId))
                .ForMember(d => d.LastReviewed, s => s.MapFrom(a => a.LastModified))
                .ForMember(d => d.Etag, s => s.Ignore());

            CreateMap<JobProfileMessage, RelatedCareerSegmentDataModel>()
                .ForMember(d => d.LastReviewed, s => s.MapFrom(a => a.LastModified))
                .ForMember(d => d.RelatedCareers, s => s.MapFrom(a => a.RelatedCareersData));

            CreateMap<RelatedCareersServiceBusModel, RelatedCareerDataModel>()
                .ForMember(d => d.SocLevelTwo, s => s.Ignore())
                .ForMember(d => d.CanonicalName, s => s.MapFrom(a => a.ProfileLink));

            CreateMap<PatchRelatedCareerServiceBusModel, PatchRelatedCareersDataModel>()
                .ForMember(d => d.CanonicalName, s => s.MapFrom(a => a.ProfileLink))
                .ForMember(d => d.SocLevelTwo, s => s.Ignore());
        }
    }
}