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

        internal static readonly string MainJobSocLevelTwo = "12345Soc";
        private static readonly string Job2SocLevelTwo = "23456Soc";
        private static readonly string Job3SocLevelTwo = "34567Soc";

        internal static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");
        private static readonly Guid Job2ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-26f999f68661");
        private static readonly Guid Job3ArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-36f999f68661");

        public async Task SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory)
        {
            var models = new List<RelatedCareersSegmentModel>()
            {
                new RelatedCareersSegmentModel
                {
                    DocumentId = MainArticleGuid,
                    CanonicalName = Job1CanonicalName,
                    LastReviewed = DateTime.UtcNow,
                    SocLevelTwo = MainJobSocLevelTwo,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                Id = Job2ArticleGuid,
                                ProfileLink = Job2CanonicalName,
                                Title = Job2Title,
                            },
                            new RelatedCareerDataModel
                            {
                                Id = Job3ArticleGuid,
                                ProfileLink = Job3CanonicalName,
                                Title = Job3Title,
                            },
                        },
                    },
                },

                new RelatedCareersSegmentModel
                {
                    DocumentId = Job2ArticleGuid,
                    CanonicalName = Job2CanonicalName,
                    LastReviewed = DateTime.UtcNow,
                    SocLevelTwo = Job2SocLevelTwo,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                Id = MainArticleGuid,
                                ProfileLink = Job1CanonicalName,
                                Title = Job1Title,
                            },
                            new RelatedCareerDataModel
                            {
                                Id = Job3ArticleGuid,
                                ProfileLink = Job3CanonicalName,
                                Title = Job3Title,
                            },
                        },
                    },
                },

                new RelatedCareersSegmentModel
                {
                    DocumentId = Job3ArticleGuid,
                    CanonicalName = Job3CanonicalName,
                    LastReviewed = DateTime.UtcNow,
                    SocLevelTwo = Job3SocLevelTwo,
                    Data = new RelatedCareerSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        RelatedCareers = new List<RelatedCareerDataModel>
                        {
                            new RelatedCareerDataModel
                            {
                                Id = MainArticleGuid,
                                ProfileLink = Job1CanonicalName,
                                Title = Job1Title,
                            },
                            new RelatedCareerDataModel
                            {
                                Id = Job2ArticleGuid,
                                ProfileLink = Job2CanonicalName,
                                Title = Job2Title,
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