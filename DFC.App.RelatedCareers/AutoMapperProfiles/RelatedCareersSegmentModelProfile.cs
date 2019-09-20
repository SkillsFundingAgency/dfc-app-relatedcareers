using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;

namespace DFC.App.RelatedCareers.AutoMapperProfiles
{
    public class RelatedCareersSegmentModelProfile : Profile
    {
        public RelatedCareersSegmentModelProfile()
        {
            CreateMap<RelatedCareerDataModel, RelatedCareerDataViewModel>();
            CreateMap<RelatedCareerSegmentDataModel, DocumentDataViewModel>();
            CreateMap<RelatedCareersSegmentModel, IndexDocumentViewModel>();

            CreateMap<RelatedCareersSegmentModel, DocumentViewModel>()
                .ForMember(d => d.RoutePrefix, s => s.Ignore())
                ;
        }
    }
}