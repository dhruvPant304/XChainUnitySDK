namespace RiskyTools.Messaging
{
    /// \ingroup Messaging
    /// <summary>
    ///     wrapper class used to pass messages to the MessasgeBroker
    /// </summary>
    /// <typeparam name="T">payload Type, used to filter messasges in the message broker.</typeparam>
    public class Message<T>
    {
        /// <summary>
        ///     the message data object that is being passed
        /// </summary>
        public T Payload;

        /// <summary>
        ///     source of the message
        /// </summary>
        public object Sender;

        /// <summary>
        ///     Constructs a new message
        /// </summary>
        /// <param name="sender">the object that is sending the message</param>
        /// <param name="payload">the T data being passed in the message</param>
        public Message(object sender, T payload)
        {
            Sender = sender;
            Payload = payload;
        }
    }
}