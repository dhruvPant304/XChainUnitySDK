using Assets.Scripts.Features.Communication.Enums;
using Features.Communication.Enums;

namespace Assets.Scripts.Features.Communication.Messages
{
    public class XChainEventMessage
    {
        public XChainEvents eventType;
        public XChainEventMessage(XChainEvents eventType)
        {
            this.eventType = eventType;
        }
    }
}