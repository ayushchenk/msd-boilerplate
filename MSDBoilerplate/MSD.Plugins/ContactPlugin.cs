using MSD.Plugins.QueryHandlers;
using MSD.Plugins.Services;
using MSD.Shared.Abstract;
using MSD.Shared.Definitions;
using MSD.Shared.Model;
using MSD.Shared.Plugins;
using System;

namespace MSD.Plugins
{
    public class ContactPlugin : MessageAwarePluginBase
    {
        private IPluginService _populateAccountService;

        public ContactPlugin()
        {
            PluginEvents.Add(new PluginEvent()
            {
                EntityName = "contact",
                MessageName = MessageNames.Create,
                Stage = EventPipeline.PreOperation,
                PluginService = new Func<IPluginService>(() => _populateAccountService)
            });
        }

        protected override void InitializeDependencies(IServiceFactory serviceFactory)
        {
            var accountHandler = new AccountByNameQueryHandler(serviceFactory);
            _populateAccountService = new PopulateContactAccountService(accountHandler);
        }
    }
}
