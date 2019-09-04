using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.RelatedCareers.AutoMapperProfiles
{
    public class RelatedCareersSegmentModelProfile : Profile
    {
        public RelatedCareersSegmentModelProfile()
        {
            CreateMap<RelatedCareersSegmentModel, DocumentViewModel>()
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)));

            CreateMap<RelatedCareersSegmentModel, IndexDocumentViewModel>();
        }
    }
}