using Microsoft.Xrm.Sdk;

namespace MSD.Shared.Abstract
{
    public interface IServiceFactory
    {
        IOrganizationService GetUserOrganizationService();
        IOrganizationService GetAdminOrganizationService();
        IPluginExecutionContext GetExecutionContext();
        ITracingService GetTracingService();
        T GetInputParameter<T>(string name);
        void SetOutputParameter(object value, string name);
    }
}
