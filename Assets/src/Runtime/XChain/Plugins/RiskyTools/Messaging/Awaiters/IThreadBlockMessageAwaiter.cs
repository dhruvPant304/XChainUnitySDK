using System;

namespace RiskyTools.Messaging.Awaiters
{
    /// \ingroup Messaging
    internal interface IThreadBlockMessageAwaiter
    {
        void BlockWaitForMessage<T>();
        void BlockWaitForMessage<T>(object source);
        void BlockWaitForMessage<T>(Func<Message<T>, bool> filter);
    }
}