using MSD.Shared.Abstract;
using System;

namespace MSD.Shared.Model
{
    public class PluginEvent
    {
        public EventPipeline Stage { get; set; }

        public string EntityName { get; set; }

        public string MessageName { get; set; }

        public Func<IPluginService> PluginService { get; set; }
    }

    public enum EventPipeline
    {
        PreValidation = 10,
        PreOperation = 20,
        MainOperation = 30,
        PostOperation = 40
    }
}
