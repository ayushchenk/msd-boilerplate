using MSD.Shared.Abstract;

namespace MSD.Shared.Extensions
{
    public static class ServiceFactoryExtensions
    {
        public static void Trace(this IServiceFactory factory, string message)
        {
            factory.GetTracingService().Trace(message);
        }
    }
}
