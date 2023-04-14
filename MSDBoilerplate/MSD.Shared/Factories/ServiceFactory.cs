using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using MSD.Shared.Abstract;
using System;

namespace MSD.Shared.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IOrganizationServiceFactory _orgServiceFactory;
        private readonly Lazy<IPluginExecutionContext> _context;
        private readonly Lazy<IOrganizationService> _userOrgService;
        private readonly Lazy<IOrganizationService> _adminOrgService;
        private readonly Lazy<ITracingService> _tracingService;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _orgServiceFactory = serviceProvider.Get<IOrganizationServiceFactory>();
            _context = new Lazy<IPluginExecutionContext>(() => serviceProvider.Get<IPluginExecutionContext>());
            _tracingService = new Lazy<ITracingService>(() => serviceProvider.Get<ITracingService>());
            _userOrgService = new Lazy<IOrganizationService>(() => _orgServiceFactory.CreateOrganizationService(_context.Value.UserId));
            _adminOrgService = new Lazy<IOrganizationService>(() => _orgServiceFactory.CreateOrganizationService(null));
        }

        public IOrganizationService GetUserOrganizationService()
        {
            return _userOrgService.Value;
        }

        public IOrganizationService GetAdminOrganizationService()
        {
            return _adminOrgService.Value;
        }

        public ITracingService GetTracingService()
        {
            return _tracingService.Value;
        }

        public IPluginExecutionContext GetExecutionContext()
        {
            return _context.Value;
        }

        public T GetInputParameter<T>(string name)
        {
            _context.Value.InputParameters.TryGetValue<T>(name, out var param);
            return param;
        }

        public void SetOutputParameter(object value, string name)
        {
            _context.Value.OutputParameters[name] = value;
        }
    }
}
