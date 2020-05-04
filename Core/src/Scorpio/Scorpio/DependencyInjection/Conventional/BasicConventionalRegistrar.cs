﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scorpio.Conventional;
using Scorpio.DependencyInjection.Conventional;
using System.Linq.Expressions;

namespace Scorpio.DependencyInjection.Conventional
{
    internal class BasicConventionalRegistrar : IConventionalRegistrar
    {
        public void Register(IConventionalRegistrationContext context)
        {
            context.DoConventionalAction<ConventionalDependencyAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<ISingletonDependency>()).AsDefault().AsSelf().Lifetime(ServiceLifetime.Singleton);
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<ITransientDependency>()).AsDefault().AsSelf().Lifetime(ServiceLifetime.Transient);
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IScopedDependency>()).AsDefault().AsSelf().Lifetime(ServiceLifetime.Scoped);
                config.Where(t => t.IsStandardType()).Where(t => t.AttributeExists<ExposeServicesAttribute>(false)).AsExposeService();
            });
        }
    }
}
