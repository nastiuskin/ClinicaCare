﻿using MediatR;

namespace Application.Configuration.Queries
{
    public interface IQueryHandler<TQuery,TResult> : IRequestHandler<TQuery,TResult> where TQuery : IQuery<TResult> { }
}
