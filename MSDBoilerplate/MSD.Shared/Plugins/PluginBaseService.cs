using MSD.Shared.Abstract;

namespace MSD.Shared.Plugins
{
    public abstract class PluginBaseService
    {
        public abstract void Execute(IServiceFactory serviceFactory);
    }
}
