using DFC.App.RelatedCareers.Data.Models;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.DraftSegmentService
{
    public interface IDraftRelatedCareersSegmentService
    {
        Task<RelatedCareersSegmentModel> GetSitefinityData(string canonicalName);
    }
}