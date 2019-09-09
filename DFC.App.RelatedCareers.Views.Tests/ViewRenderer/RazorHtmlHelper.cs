using RazorEngine.Text;

namespace DFC.App.RelatedCareers.Views.Tests.ViewRenderer
{
    public class RazorHtmlHelper
    {
        public IEncodedString Raw(string rawString)
        {
            return new RawString(rawString);
        }
    }
}