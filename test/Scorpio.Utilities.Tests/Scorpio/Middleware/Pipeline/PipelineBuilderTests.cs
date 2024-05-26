﻿using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Shouldly;

using Xunit;

namespace Scorpio.Middleware.Pipeline
{
    public class PipelineBuilderTests
    {
        [Fact]
        public void Use()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.Throw<ArgumentNullException>(() => builder.Use((Func<TestPipelineContext, PipelineRequestDelegate<TestPipelineContext>,Task>)null));
            Should.NotThrow(() => builder.Use((context,next) => { context.PipelineInvoked = true; return next(context); }));
            var context = new TestPipelineContext();
            context.PipelineInvoked.ShouldBeFalse();
            Should.NotThrow(() => builder.Build()(context));
            context.PipelineInvoked.ShouldBeTrue();
        }
        [Fact]
        public void Use_S()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.Throw<ArgumentNullException>(() => builder.Use((Func<TestPipelineContext, Func<Task>, Task>)null));
            Should.NotThrow(() => builder.Use((TestPipelineContext context, Func<Task> next) => { context.PipelineInvoked = true; return next(); }));
            Should.NotThrow(() => builder.Use((TestPipelineContext context, Func<Task> next) => { context.PipelineInvoked = true; return next(); }));
            var context = new TestPipelineContext();
            context.PipelineInvoked.ShouldBeFalse();
            Should.NotThrow(() => builder.Build()(context));
            context.PipelineInvoked.ShouldBeTrue();
        }


        [Fact]
        public void UseMiddleware()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.Throw<ArgumentNullException>(() => builder.UseMiddleware(null));
            Should.NotThrow(() => builder.UseMiddleware(typeof(FuncResultMiddleware)));
            Should.NotThrow(() => builder.UseMiddleware(typeof(TestMiddleware)));
            var context = new TestPipelineContext();
            context.PipelineInvoked.ShouldBeFalse();
            Should.NotThrow(() => builder.Build()(context));
            context.PipelineInvoked.ShouldBeTrue();
        }

        [Fact]
        public void UseMiddleware_T()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, FuncResultMiddleware>());
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, TestMiddleware>());
            var context = new TestPipelineContext();
            context.PipelineInvoked.ShouldBeFalse();
            Should.NotThrow(() => builder.Build()(context));
            context.PipelineInvoked.ShouldBeTrue();
        }

        [Fact]
        public void UseMiddleware_T_NonServiceProvider()
        {
            var builder = new TestPipelineBuilder(null);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, ManyParametersMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<InvalidOperationException>(() => builder.Build()(context));
        }
        [Fact]
        public void UseMiddleware_T_Exception1()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, DoublyMethodMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<InvalidOperationException>(() => builder.Build()(context));
        }

        [Fact]
        public void UseMiddleware_T_Exception2()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, NonMethodMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<InvalidOperationException>(() => builder.Build()(context));
        }

        [Fact]
        public void UseMiddleware_T_Exception3()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, NotTaskMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<InvalidOperationException>(() => builder.Build()(context));
        }

        [Fact]
        public void UseMiddleware_T_Exception4()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, NonParameterMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<InvalidOperationException>(() => builder.Build()(context));
        }

        [Fact]
        public void UseMiddleware_T_Exception5()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, ByRefParametersMiddleware>());
            var context = new TestPipelineContext();
            Should.Throw<NotSupportedException>(() => builder.Build()(context));
        }

        [Fact]
        public void UseMiddleware_ManyParameters()
        {
            var descriptors = new ServiceCollection();
            var serviceProvider = descriptors.BuildServiceProvider();
            var builder = new TestPipelineBuilder(serviceProvider);
            builder.ApplicationServices.ShouldBe(serviceProvider);
            Should.NotThrow(() => builder.UseMiddleware<TestPipelineContext, ManyParametersMiddleware>());
            var context = new TestPipelineContext();
            context.PipelineInvoked.ShouldBeFalse();
            Should.NotThrow(() => builder.Build()(context));
            context.PipelineInvoked.ShouldBeTrue();
        }
    }
}
