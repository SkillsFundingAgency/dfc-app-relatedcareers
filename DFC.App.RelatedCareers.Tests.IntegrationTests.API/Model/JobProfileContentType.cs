using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model
{
    public class JobProfileContentType
    {
        public string JobProfileId { get; set; }
        public string Title { get; set; }
        public string DynamicTitlePrefix { get; set; }
        public string AlternativeTitle { get; set; }
        public string Overview { get; set; }
        public string SocLevelTwo { get; set; }
        public string UrlName { get; set; }
        public string DigitalSkillsLevel { get; set; }
        public List<Restriction> Restrictions { get; set; }
        public string OtherRequirements { get; set; }
        public string CareerPathAndProgression { get; set; }
        public string CourseKeywords { get; set; }
        public double MinimumHours { get; set; }
        public double MaximumHours { get; set; }
        public double SalaryStarter { get; set; }
        public double SalaryExperienced { get; set; }
        public List<WorkingPattern> WorkingPattern { get; set; }
        public List<WorkingPatternDetail> WorkingPatternDetails { get; set; }
        public List<WorkingHoursDetail> WorkingHoursDetails { get; set; }
        public List<HiddenAlternativeTitle> HiddenAlternativeTitle { get; set; }
        public List<JobProfileSpecialism> JobProfileSpecialism { get; set; }
        public bool IsImported { get; set; }
        public HowToBecomeData HowToBecomeData { get; set; }
        public WhatYouWillDoData WhatYouWillDoData { get; set; }
        public SocCodeData SocCodeData { get; set; }
        public List<RelatedCareersData> RelatedCareersData { get; set; }
        public List<SocSkillsMatrixData> SocSkillsMatrixData { get; set; }
        public List<JobProfileCategory> JobProfileCategories { get; set; }
        public DateTime LastModified { get; set; }
        public string CanonicalName { get; set; }
        public string WidgetContentTitle { get; set; }
        public bool IncludeInSitemap { get; set; }
    }

    public class Restriction
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
    }

    public class WorkingPattern
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class WorkingPatternDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class WorkingHoursDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class HiddenAlternativeTitle
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class JobProfileSpecialism
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class EntryRequirement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
    }

    public class MoreInformationLink
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
    }

    public class RouteEntry
    {
        public int RouteName { get; set; }
        public List<EntryRequirement> EntryRequirements { get; set; }
        public List<MoreInformationLink> MoreInformationLinks { get; set; }
        public string RouteSubjects { get; set; }
        public string FurtherRouteInformation { get; set; }
        public string RouteRequirement { get; set; }
    }

    public class FurtherInformationContentType
    {
        public string CareerTips { get; set; }
        public string ProfessionalAndIndustryBodies { get; set; }
        public string FurtherInformation { get; set; }
    }

    public class FurtherRoutes
    {
        public string Work { get; set; }
        public string Volunteering { get; set; }
        public string DirectApplication { get; set; }
        public string OtherRoutes { get; set; }
    }

    public class Registration
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
    }

    public class HowToBecomeData
    {
        public List<RouteEntry> RouteEntries { get; set; }
        public FurtherInformationContentType FurtherInformation { get; set; }
        public FurtherRoutes FurtherRoutes { get; set; }
        public string IntroText { get; set; }
        public List<Registration> Registrations { get; set; }
    }

    public class Location
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsNegative { get; set; }
    }

    public class Uniform
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsNegative { get; set; }
    }

    public class Environment
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsNegative { get; set; }
    }

    public class WhatYouWillDoData
    {
        public string DailyTasks { get; set; }
        public List<Location> Locations { get; set; }
        public List<Uniform> Uniforms { get; set; }
        public List<Environment> Environments { get; set; }
        public string Introduction { get; set; }
    }

    public class ApprenticeshipStandard
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public object Description { get; set; }
    }

    public class ApprenticeshipFrameworkContentType
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class SocCodeData
    {
        public string Id { get; set; }
        public string SOCCode { get; set; }
        public string Description { get; set; }
        public string ONetOccupationalCode { get; set; }
        public string UrlName { get; set; }
        public List<ApprenticeshipFrameworkContentType> ApprenticeshipFramework { get; set; }
        public List<ApprenticeshipStandard> ApprenticeshipStandards { get; set; }
    }

    public class RelatedCareersData
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ProfileLink { get; set; }
    }

    public class RelatedSkill
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ONetElementId { get; set; }
        public string Description { get; set; }
    }

    public class RelatedSOC
    {
        public string Id { get; set; }
        public string SOCCode { get; set; }
    }

    public class SocSkillsMatrixData
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Contextualised { get; set; }
        public string ONetAttributeType { get; set; }
        public double Rank { get; set; }
        public double ONetRank { get; set; }
        public List<RelatedSkill> RelatedSkill { get; set; }
        public List<RelatedSOC> RelatedSOC { get; set; }
    }

    public class JobProfileCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
