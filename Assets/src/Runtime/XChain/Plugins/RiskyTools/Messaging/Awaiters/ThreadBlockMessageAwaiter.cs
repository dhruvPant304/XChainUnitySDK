using System;
using System.Threading;
using RiskyTools.Messaging.Interfaces;

namespace RiskyTools.Messaging.Awaiters
{
    /// \ingroup Messaging
    internal class ThreadBlockMessageAwaiter : IThreadBlockMessageAwaiter
    {
        private readonly IMessagingService _messagingService;

        public ThreadBlockMessageAwaiter(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        public void BlockWaitForMessage<T>()
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message => { blocking = false; });

            while (blocking) Thread.Sleep(10);
        }

        public void BlockWaitForMessage<T>(object source)
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message =>
            {
                if (message.Sender == source) blocking = false;
            });

            while (blocking) Thread.Sleep(10);
        }

        public void BlockWaitForMessage<T>(Func<Message<T>, bool> filter)
        {
            var blocking = true;

            _messagingService.Subscribe<T>(message =>
            {
                if (filter(message)) blocking = false;
            });

            while (blocking) Thread.Sleep(10);
        }
    }
}