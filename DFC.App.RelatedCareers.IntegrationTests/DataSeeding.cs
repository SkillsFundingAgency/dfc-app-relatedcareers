using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.IntegrationTests
{
    public class DataSeeding
    {
        private const string SegmentUrl = "/segment";

        internal const string Job1CanonicalName = "nurse";
        private const string Job1Title = "Nurse Title";
        private const string Job2CanonicalName = "relatedJob2";
        private const string Job2Title = "Related Job 2 Title";
        private const string Job3CanonicalName = "relatedJob3";
        private const string Job3Title = "Related Job 3 Title";

        internal static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");
        private static readonly Guid Job2ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-26f999f68661");
        private static readonly Guid Job3ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-36f999f68661");

        internal static readonly DateTime MainJobDatetime = new DateTime(2019, 1, 15, 15, 30, 11);
        private static readonly DateTime Job2Datetime = new DateTime(2019, 1, 15, 15, 30, 21);
        private static readonly DateTime Job3Datetime = new DateTime(2019, 1, 15, 15, 30, 31);

        public async Task SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory)
        {
            var models = new List<RelatedCareersSegmentModel>()
            {
                new RelatedCareersSegmentModel
                {
                    DocumentId = MainArticleGuid,
                    CanonicalName = Job1CanonicalName,
                    Created = MainJobDatetime,
                    Updated = DateTime.UtcNow,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                DocumentId = Job2ArticleGuid,
                                CanonicalName = Job2CanonicalName,
                                Title = Job2Title,
                                Updated = DateTime.UtcNow,
                            },
                            new RelatedCareerDataModel
                            {
                                DocumentId = Job3ArticleGuid,
                                CanonicalName = Job3CanonicalName,
                                Title = Job3Title,
                                Updated = DateTime.UtcNow,
                            },
                        },
                    },
                },

                new RelatedCareersSegmentModel
                {
                    DocumentId = Job2ArticleGuid,
                    CanonicalName = Job2CanonicalName,
                    Created = Job2Datetime,
                    Updated = DateTime.UtcNow,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                DocumentId = MainArticleGuid,
                                CanonicalName = Job1CanonicalName,
                                Title = Job1Title,
                                Updated = DateTime.UtcNow,
                            },
                            new RelatedCareerDataModel
                            {
                                DocumentId = Job3ArticleGuid,
                                CanonicalName = Job3CanonicalName,
                                Title = Job3Title,
                                Updated = DateTime.UtcNow,
                            },
                        },
                    },
                },

                new RelatedCareersSegmentModel
                {
                    DocumentId = Job3ArticleGuid,
                    CanonicalName = Job3CanonicalName,
                    Created = Job3Datetime,
                    Updated = DateTime.UtcNow,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                DocumentId = MainArticleGuid,
                                CanonicalName = Job1CanonicalName,
                                Title = Job1Title,
                                Updated = DateTime.UtcNow,
                            },
                            new RelatedCareerDataModel
                            {
                                DocumentId = Job2ArticleGuid,
                                CanonicalName = Job2CanonicalName,
                                Title = Job2Title,
                                Updated = DateTime.UtcNow,
                            },
                        },
                    },
                },
            };

            var client = factory?.CreateClient();

            client?.DefaultRequestHeaders.Accept.Clear();

            foreach (var relatedCareersSegmentModel in models)
            {
                await client.PostAsync(SegmentUrl, relatedCareersSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);
            }
        }
    }
}