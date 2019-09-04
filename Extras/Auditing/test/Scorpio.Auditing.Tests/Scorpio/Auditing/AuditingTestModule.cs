﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Scorpio.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Scorpio.Security;
using Scorpio.Timing;

namespace Scorpio.Auditing
{
    [DependsOn(typeof(AuditingModule))]
    public class AuditingTestModule : ScorpioModule
    {
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.ReplaceSingleton<ICurrentPrincipalAccessor, TestPrincipalAccessor>();
            context.Services.ReplaceSingleton<IClock, TestClock>();
            context.Services.Configure<AuditingOptions>(opt =>
            {
                opt.Contributors.Add(new TestContributor());
            });
            context.Services.RegisterAssemblyByConvention();
            context.Services.ReplaceSingleton<IAuditingStore, FackAuditingStore>();
        }

    }

    class TestPrincipalAccessor : ICurrentPrincipalAccessor
    {
        public IPrincipal Principal { get; }

        public TestPrincipalAccessor()
        {
            Principal = new GenericPrincipal(new GenericIdentity("TestUser"), new[] { "Admin" });
        }
    }

    class TestContributor : IAuditContributor
    {
        public void PostContribute(AuditContributionContext context)
        {
            context.AuditInfo.Comments.Add("PostContribute");
        }

        public void PreContribute(AuditContributionContext context)
        {
            context.AuditInfo.Comments.Add("PreContribute");
        }
    }

    class TestClock : IClock
    {
        public DateTime Now { get; }
        public DateTimeKind Kind { get; }
        public bool SupportsMultipleTimezone { get; }

        public TestClock()
        {
            Now = new DateTime(2019, 1, 1, 0, 0, 0);
            SupportsMultipleTimezone = true;
        }
        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime;
        }
    }
}
