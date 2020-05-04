﻿using Scorpio.Application.Dtos;
using System;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Scorpio.Repositories;
using Scorpio.Entities;
using Scorpio.Linq;
using Microsoft.Extensions.DependencyInjection;
namespace Scorpio.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AsyncCrudApplicationService<TEntity, TEntityDto, TKey>
        : AsyncCrudApplicationService<TEntity, TEntityDto, TKey, ListRequest<TEntityDto>>,
        IAsyncCrudApplicationService<TEntityDto, TKey>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudApplicationService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TGetListInput"></typeparam>
    public abstract class AsyncCrudApplicationService<TEntity, TEntityDto, TKey, TGetListInput>
        : AsyncCrudApplicationService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>,
        IAsyncCrudApplicationService<TEntityDto, TKey, TGetListInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudApplicationService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TGetListInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    public abstract class AsyncCrudApplicationService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
        : AsyncCrudApplicationService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>,
        IAsyncCrudApplicationService<TEntityDto, TKey, TGetListInput, TCreateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudApplicationService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
        }
    }
    public abstract class AsyncCrudApplicationService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        : CrudApplicationServiceBase<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        IAsyncCrudApplicationService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get;  }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="repository"></param>
        protected AsyncCrudApplicationService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
            AsyncQueryableExecuter = serviceProvider.GetService<IAsyncQueryableExecuter>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<TEntity>(input);
            await Repository.InsertAsync(entity, cancellationToken: cancellationToken);
            return Mapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteAsync(id, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntityDto Get(TKey id)
        {
            return Mapper.Map<TEntityDto>(Repository.Get(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Mapper.Map<TEntityDto>(await Repository.GetAsync(id, cancellationToken: cancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<IPagedResult<TEntityDto>> GetListAsync(TGetListInput input, CancellationToken cancellationToken = default)
        {
            var query = GetQuery(Repository);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query, cancellationToken);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            return new PagedResult<TEntityDto>(await AsyncQueryableExecuter.ToListAsync(query.ProjectTo<TEntityDto>(Configuration), cancellationToken)) { TotalCount = totalCount };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input, CancellationToken cancellationToken = default)
        {
            var entity = Repository.Get(id);
            Mapper.Map(input, entity);
            await Repository.UpdateAsync(entity, cancellationToken: cancellationToken);
            return Mapper.Map<TEntityDto>(entity);
        }
    }
}
