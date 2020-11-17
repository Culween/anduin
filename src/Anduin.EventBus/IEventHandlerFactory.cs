using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public interface IEventHandlerFactory
    {
        IEventHandlerDisposeWrapper GetHandler();
    }
}
