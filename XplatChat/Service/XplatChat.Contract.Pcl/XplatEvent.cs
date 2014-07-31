using System;

namespace XplatChat.Contract.Pcl
{
    public class XplatEvent
    {
        public DateTime WhenOccurred { get; set; }
        public XplatEventTypeEnum EventType { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

    }
}