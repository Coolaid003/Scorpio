﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Scorpio.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class DataFilterDescriptor:IDataFilterDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type FilterType { get; }

        /// <summary>
        /// 
        /// </summary>
        public IFilterContext FilterContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterType"></param>
        internal DataFilterDescriptor(Type filterType)
        {
            FilterType = filterType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal DataFilterState GetState()
        {
            return new DataFilterState(IsEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> BuildFilterExpression<TEntity>(IDataFilter dataFilter, IFilterContext context) where TEntity : class
        {
            FilterContext = context;
            var filterexpression = BuildFilterExpression<TEntity>(context);
            var expression = filterexpression.OrElse(filterexpression.Equal(expr2 => dataFilter.IsEnabled(FilterType)));
            return expression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected abstract Expression<Func<TEntity, bool>> BuildFilterExpression<TEntity>(IFilterContext context) where TEntity : class;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    internal sealed class DataFilterDescriptor<TFilter> : DataFilterDescriptor,IDataFilterDescriptor<TFilter>
    {
        /// <summary>
        /// 
        /// </summary>
        internal DataFilterDescriptor() : base(typeof(TFilter))
        {
            IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public Expression<Func<TFilter, bool>> FilterExpression { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected override Expression<Func<TEntity, bool>> BuildFilterExpression<TEntity>(IFilterContext context)
        {
            var filterexpression = FilterExpression.Translate().To<TEntity>();
            return filterexpression;
        }
    }

}
