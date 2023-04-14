using MSD.Shared.Abstract.Cqrs.Requests;

namespace MSD.Shared.Abstract.Cqrs.Handlers
{
    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }
}
