using MSD.Shared.Abstract.Cqrs.Requests;

namespace MSD.Shared.Abstract.Cqrs.Handlers
{
    public abstract class DataverseCommandHandler<TCommand, TResponse> : DataverseRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        protected DataverseCommandHandler(IServiceFactory serviceFactory) : base(serviceFactory)
        {
        }
    }
}
