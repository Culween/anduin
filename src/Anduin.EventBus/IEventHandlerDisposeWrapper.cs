using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public interface IEventHandlerDisposeWrapper : IDisposable
    {
        IEventHandler EventHandler { get; }
    }
}
