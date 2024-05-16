namespace RiskyTools.Messaging.Interfaces
{
    public interface IMessage
    {
        /// <summary>
        ///     source of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        ///     the message data object that is being passed
        /// </summary>
        public object Payload { get; }
    }

    public interface IMessage<T> : IMessage
    {
        /// <summary>
        ///     the message data object that is being passed
        /// </summary>
        public new T Payload { get; }
    }
}