using MSD.Shared.Abstract.Cqrs.Requests;

namespace MSD.Shared.Abstract.Cqrs.Handlers
{
    public abstract class DataverseQueryHandler<TQuery, TResponse> : DataverseRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        protected DataverseQueryHandler(IServiceFactory serviceFactory) : base(serviceFactory)
        {
        }
    }
}
