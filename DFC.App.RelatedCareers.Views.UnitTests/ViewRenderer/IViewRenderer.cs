using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Views.UnitTests.ViewRenderer
{
    public interface IViewRenderer
    {
        string Render(string template, object model, IDictionary<string, object> viewBag);
    }
}