using System.ComponentModel;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    internal class EnumLibrary
    {
        public enum RequirementType
        {
            University = 2,
            College = 1,
            Apprenticeship = 0
        }

        public enum CType
        {
            JobProfile,
            JobProfileSoc,
            WorkingHoursDetails,
            WorkingPattern,
            WorkingPatternDetails,
            JobProfileSpecialism
        }

        public enum ActionType
        {
            Published,
            Deleted
        }

        public enum ContentType
        {
            [Description("application/json")]
            JSON,
            [Description("text/html")]
            HTML
        }
    }
}
