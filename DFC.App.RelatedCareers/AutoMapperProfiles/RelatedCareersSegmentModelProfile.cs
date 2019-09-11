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
            CreateMap<RelatedCareersSegmentModel, DocumentViewModel>()
                .ForMember(d => d.Updated, s => s.MapFrom(a => a.Updated));
            CreateMap<RelatedCareersSegmentModel, IndexDocumentViewModel>();
        }
    }
}
