using System;

namespace NetworkShared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerRegisterAttribute : Attribute
    {
        public HandlerRegisterAttribute(PacketType type)
        {
            PacketType= type;
        }

        public PacketType PacketType { get; set; }
    }
}
