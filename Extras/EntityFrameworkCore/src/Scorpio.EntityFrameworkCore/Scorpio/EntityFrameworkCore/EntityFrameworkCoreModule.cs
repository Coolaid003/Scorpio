﻿using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.Modularity;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Scorpio.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Scorpio.EntityFrameworkCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Scorpio.EntityFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(DataModule))]
    public sealed class EntityFrameworkCoreModule : ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void PreConfigureServices(ConfigureServicesContext context)
        {
            context.Services.Configure<ScorpioDbContextOptions>(options =>
            {
                options.AddModelCreatingContributor<DataModelCreatingContributor>();
                options.PreConfigure(dbConfigContext => dbConfigContext.DbContextOptions.ConfigureWarnings(
                    warnings => warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning)
                    ));
            });
            context.Services.Configure<DbConnectionOptions>(context.Configuration);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.ScorpioDbContext(builder =>
            {
                builder.AddSaveChangeHandler<SoftDeleteSaveChangeHandler>();
                builder.AddSaveChangeHandler<HasExtraPropertiesSaveChangeHandler>();
            });
            context.Services.TryAddTransient<IOnSaveChangeHandlersFactory, OnSaveChangeHandlersFactory>();
            context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(DefaultDbContextProvider<>));
            context.Services.RegisterAssemblyByConvention();
            base.ConfigureServices(context);
        }
    }
}
