using System;
using System.Collections;

namespace RiskyTools.Messaging.Awaiters
{
    /// \ingroup Messaging
    internal interface IWaitUntilMessageAwaiter
    {
        IEnumerator WaitForMessage<T>();
        IEnumerator WaitForMessage<T>(object source);
        IEnumerator WaitForMessage<T>(Func<Message<T>, bool> filter);
    }
}