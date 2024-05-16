using System;
using RiskyTools.Messaging.Services;

namespace RiskyTools.Messaging.Interfaces
{
    /// \ingroup Messaging
    public interface IMessagingService
    {
        void Publish<T>(object source, T messageData);
        void Subscribe<T>(Action<Message<T>> subscription, SubscriptionType subscriptionType = SubscriptionType.Disposable);
        void Unsubscribe<T>(Action<Message<T>> subscription);
    }
    
    public enum SubscriptionType
    {
        Permanent,
        Disposable
    }
}