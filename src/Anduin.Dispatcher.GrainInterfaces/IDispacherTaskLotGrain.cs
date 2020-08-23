using Orleans;
using Orleans.CodeGeneration;
using System;
using System.Threading.Tasks;

namespace Anduin.Dispatcher.GrainInterfaces
{
    [Version(1)]
    public interface IDispacherTaskLotGrain : IGrainWithGuidCompoundKey
    {
        Task Start(string startWay);
    }
}
