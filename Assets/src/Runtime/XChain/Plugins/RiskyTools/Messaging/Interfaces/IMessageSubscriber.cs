using System;

namespace RiskyTools.Messaging.Interfaces
{
    public interface IMessageSubscriber : IDisposable
    {
        void Subscribe();
        void Unsubscribe();
    }
}