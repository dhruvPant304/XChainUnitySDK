using System;
using System.Collections;
using RiskyTools.Messaging.Interfaces;
using UnityEngine;

namespace RiskyTools.Messaging.Awaiters
{
    /// \ingroup Messaging
    internal class WaitUntilMessageAwaiter : IWaitUntilMessageAwaiter
    {
        private readonly IMessagingService _messagingService;
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();

        public WaitUntilMessageAwaiter(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        public IEnumerator WaitForMessage<T>()
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message => { blocking = false; });

            while (blocking) yield return _waitForEndOfFrame;
        }

        public IEnumerator WaitForMessage<T>(object source)
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message =>
            {
                if (message.Sender == source) blocking = false;
            });

            while (blocking) yield return _waitForEndOfFrame;
        }

        public IEnumerator WaitForMessage<T>(Func<Message<T>, bool> filter)
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message =>
            {
                if (filter(message)) blocking = false;
            });

            while (blocking) yield return _waitForEndOfFrame;
        }
    }
}