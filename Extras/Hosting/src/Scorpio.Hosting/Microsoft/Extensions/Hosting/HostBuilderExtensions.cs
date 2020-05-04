﻿using Scorpio;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    public static class HostBuilderExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartupModule"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder AddScorpio<TStartupModule>(this IHostBuilder hostBuilder)
                 where TStartupModule : Scorpio.Modularity.IScorpioModule
        {
            return AddScorpio<TStartupModule>(hostBuilder, o => { });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartupModule"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IHostBuilder AddScorpio<TStartupModule>(this IHostBuilder hostBuilder, Action<BootstrapperCreationOptions> optionsAction)
            where TStartupModule : Scorpio.Modularity.IScorpioModule
        {
            return AddScorpio(hostBuilder, typeof(TStartupModule), optionsAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="startupModuleType"></param>
        /// <returns></returns>
        public static IHostBuilder AddScorpio(this IHostBuilder builder, Type startupModuleType)
        {
            return AddScorpio(builder, startupModuleType, o => { });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="startupModuleType"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IHostBuilder AddScorpio(this IHostBuilder builder, Type startupModuleType, Action<BootstrapperCreationOptions> optionsAction)
        {
            builder.UseServiceProviderFactory(context=>new ServiceProviderFactory(context,startupModuleType,optionsAction));
            return builder;
        }
    }
}
