using System;
using System.Threading.Tasks;
using Anduin.Dispatcher.GrainInterfaces;
using Orleans.EventSourcing;

namespace Anduin.Dispatcher.Grains
{
    public class JobDispatchGrain
    {
        public Task Start(string startWay)
        {
            return Task.FromResult<string>("");
        }
    }
}
