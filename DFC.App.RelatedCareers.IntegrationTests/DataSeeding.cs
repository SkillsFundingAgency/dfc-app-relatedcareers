using DFC.App.RelatedCareers.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.RelatedCareers.IntegrationTests
{
    public static class DataSeeding
    {
        private const string Job2CanonicalName = "relatedJob2";
        private const string Job2Title = "Related Job 2 Title";
        private const string Job3CanonicalName = "relatedJob3";
        private const string Job3Title = "Related Job 3 Title";

        private static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");
        private static readonly Guid Job2ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-26f999f68661");
        private static readonly Guid Job3ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-36f999f68661");

        public static void SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory, string article)
        {
            const string url = "/segment";

            var models = new List<RelatedCareersSegmentModel>()
            {
                new RelatedCareersSegmentModel
                {
                    DocumentId = MainArticleGuid,
                    CanonicalName = article,
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
                    Data = new RelatedCareerSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                DocumentId = MainArticleGuid,
                                CanonicalName = article,
                                Title = article,
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
                    Data = new RelatedCareerSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                DocumentId = MainArticleGuid,
                                CanonicalName = article,
                                Title = article,
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

            models.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()));
        }
    }
}