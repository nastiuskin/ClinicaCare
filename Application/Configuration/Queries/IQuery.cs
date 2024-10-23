using MediatR;

namespace Application.Configuration.Queries
{
    public interface IQuery<TResult> : IRequest<TResult> { }
}
