using Microsoft.Xrm.Sdk;
using MSD.Shared.Abstract;
using MSD.Shared.Exceptions;
using MSD.Shared.Extensions;
using MSD.Shared.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSD.Shared.Plugins
{
    public abstract class MessageAwarePluginBase : IPlugin
    {
        protected Func<PluginBaseService> PluginServiceFactory { get; set; }

        protected List<PluginEvent> PluginEvents { get; set; }

        protected MessageAwarePluginBase() 
        {
            PluginEvents = new List<PluginEvent>();
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            var serviceFactory = new ServiceFactory(serviceProvider);
            var context = serviceFactory.GetExecutionContext();

            serviceFactory.Trace($"Entered {GetType().Name}.Execute()");

            var filteredActions = PluginEvents
                .Where(action => 
                    (int)action.Stage == context.Stage 
                    && string.Equals(action.MessageName, context.MessageName, StringComparison.InvariantCultureIgnoreCase) 
                    && (string.IsNullOrWhiteSpace(action.EntityName) || action.EntityName == context.PrimaryEntityName) 
                    && action.PluginService != null)
                .Select(a => a.PluginService);

            if (!filteredActions.Any())
            {
                serviceFactory.Trace("Cannot find any action that satisfies events criteria");
                return;
            }

            try
            {
                serviceFactory.Trace($"{GetType().Name} is firing for Entity {context.PrimaryEntityName}, Message {context.MessageName}");
                InitializeDependencies(serviceFactory);
                foreach (var action in filteredActions)
                {
                    action().Execute(serviceFactory);
                    PluginServiceFactory = null;
                }
            }
            catch (ValidationException validationException)
            {
                serviceFactory.Trace(validationException.SerializedErrors);
                throw new InvalidPluginExecutionException("Data processed by plugin are invalid: " + validationException.SerializedErrors);
            }
            catch (Exception exception)
            {
                serviceFactory.Trace(exception.ToString());
                throw;
            }
            finally
            {
                serviceFactory.Trace($"Exiting {GetType().Name}.Execute()");
            }
        }

        protected abstract void InitializeDependencies(IServiceFactory serviceFactory);
    }
}