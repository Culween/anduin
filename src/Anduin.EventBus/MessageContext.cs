using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public class MessageContext
    {
        /// <summary>
        /// 消息组
        /// </summary>
        public string Group { get; set; }

        public string Topic { get; set; }

        public string RouteKey { get; set; }

        public string MessageTypeName { get; set; }

        public byte[] Content { get; set; }
    }
}
