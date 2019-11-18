using System;

namespace DFC.App.RelatedCareers.MessageFunctionApp.Models
{
    public class SegmentClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
    }
}