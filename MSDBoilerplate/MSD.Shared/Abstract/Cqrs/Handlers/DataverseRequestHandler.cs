using Microsoft.Xrm.Sdk;
using MSD.Shared.Abstract.Cqrs.Requests;

namespace MSD.Shared.Abstract.Cqrs.Handlers
{
    public abstract class DataverseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected IServiceFactory ServiceFactory { get; }

        protected ITracingService Tracing => ServiceFactory.GetTracingService();

        protected DataverseRequestHandler(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }

        public TResponse Handle(TRequest request)
        {
            PreExecute(request);
            var response = Execute(request);
            return PostExecute(response);
        }

        protected virtual void PreExecute(TRequest request)
        {
            Tracing.Trace($"{GetType().Name} execution started");
        }

        protected virtual TResponse PostExecute(TResponse response)
        {
            Tracing.Trace($"{GetType().Name} execution finished");
            return response;
        }

        protected virtual IOrganizationService GetOrganizationService()
        {
            return ServiceFactory.GetAdminOrganizationService();
        }

        protected abstract TResponse Execute(TRequest query);
    }
}
