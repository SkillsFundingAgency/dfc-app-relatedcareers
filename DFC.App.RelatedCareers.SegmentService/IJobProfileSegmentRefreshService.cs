using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.SegmentService
{
    public interface IJobProfileSegmentRefreshService<in TModel>
    {
        Task SendMessageAsync(TModel model);
    }
}