using AutoMapper;
using DFC.App.RelatedCareers.Data.Models;
using DFC.App.RelatedCareers.Data.ServiceBusModels;
using DFC.App.RelatedCareers.MessageFunctionApp.AutomapperProfile;
using DFC.App.RelatedCareers.MessageFunctionApp.Services;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.RelatedCareers.MessageFunctionAppTests
{
    public class MappingServiceTests
    {
        private const int SequenceNumber = 123;

        private readonly IMappingService mappingService;
        private static readonly Guid JobProfileId = Guid.NewGuid();
        private readonly DateTime LastModified = DateTime.UtcNow.AddDays(-1);
        private readonly Guid RelatedCareerId1 = Guid.NewGuid();
        private readonly Guid RelatedCareerId2 = Guid.NewGuid();
        private readonly Guid RelatedCareerId3 = Guid.NewGuid();

        private const string SocCodeId = "99";
        private const string TestJobName = "Test Job name";
        private const string Title1 = "Job 1 Title";
        private const string Link1 = "job1-canonical";
        private const string Title2 = "Job 2 Title";
        private const string Link2 = "job2-canonical";
        private const string Title3 = "Job 3 Title";
        private const string Link3 = "job3-canonical";

        public MappingServiceTests()
        {
            var config = new MapperConfiguration(opts => { opts.AddProfile(new RelatedCareerProfile()); });

            var mapper = new Mapper(config);

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void MapToSegmentModelWhenJobProfileMessageSentThenItIsMappedCorrectly()
        {
            var fullJPMessage = BuildJobProfileMessage();
            var message = JsonConvert.SerializeObject(fullJPMessage);
            var expectedResponse = BuildExpectedResponse();

            // Act
            var actualMappedModel = mappingService.MapToSegmentModel(message, SequenceNumber);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualMappedModel);
        }

        private JobProfileMessage BuildJobProfileMessage()
        {
            return new JobProfileMessage
            {
                JobProfileId = JobProfileId,
                CanonicalName = TestJobName,
                LastModified = LastModified,

                SocLevelTwo = SocCodeId,
                RelatedCareersData = new List<RelatedCareersServiceBusModel>
                {
                    new RelatedCareersServiceBusModel
                    {
                        Id = RelatedCareerId1,
                        Title = Title1,
                        ProfileLink = Link1,
                    },
                    new RelatedCareersServiceBusModel
                    {
                        Id = RelatedCareerId2,
                        Title = Title2,
                        ProfileLink = Link2,
                    },
                    new RelatedCareersServiceBusModel
                    {
                        Id = RelatedCareerId3,
                        Title = Title3,
                        ProfileLink = Link3,
                    },
                },
            };
        }

        private RelatedCareersSegmentModel BuildExpectedResponse()
        {
            return new RelatedCareersSegmentModel
            {
                CanonicalName = TestJobName,
                SocLevelTwo = SocCodeId,
                Etag = null,
                DocumentId = JobProfileId,
                SequenceNumber = SequenceNumber,
                Data = new RelatedCareerSegmentDataModel
                {
                    LastReviewed = LastModified,
                    RelatedCareers = new List<RelatedCareerDataModel>
                    {
                        new RelatedCareerDataModel
                        {
                            Id = RelatedCareerId1,
                            ProfileLink = Link1,
                            Title = Title1,
                        },
                        new RelatedCareerDataModel
                        {
                            Id = RelatedCareerId2,
                            ProfileLink = Link2,
                            Title = Title2,
                        },
                        new RelatedCareerDataModel
                        {
                            Id = RelatedCareerId3,
                            ProfileLink = Link3,
                            Title = Title3,
                        },
                    },
                },
            };
        }
    }
}