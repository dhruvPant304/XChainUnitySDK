using System;
using System.Collections.Generic;
using System.Linq;
using RiskyTools.Messaging.Interfaces;
using UnityEngine.SceneManagement;

namespace RiskyTools.Messaging.Services
{
    /// \ingroup Messaging
    /// <summary>
    ///     Initial implementation based on https://www.codeproject.com/Tips/1169118/Message-Broker-Pattern-using-Csharp
    ///     Provides a messaging system that can be used to invoke Action
    ///     <T>
    ///         s on other objects without requiring a
    ///         direct refference.
    /// </summary>
    public class MessagingService : IMessagingService, IDisposable
    {
        internal class Subscription
        {
            public readonly Delegate SubscribedEvent;
            public readonly SubscriptionType SubscriptionType;

            public Subscription(Delegate subscribedEvent, SubscriptionType type)
            {
                SubscribedEvent = subscribedEvent;
                SubscriptionType = type;
            }
        }

        /// <summary>
        ///     datastructure to store lists of actions that are to be invoked for
        ///     each message Type.
        /// </summary>
        internal readonly Dictionary<Type, List<Subscription>> _subscribers;

        public MessagingService()
        {
            _subscribers = new Dictionary<Type, List<Subscription>>();
            SceneManager.sceneUnloaded += OnSceneUnLoad;
        }

        /// <summary>
        ///     IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            _subscribers?.Clear();
        }

        /// <summary>
        ///     publishes a message, effectively calling Invoke on all the
        ///     subscribed Action<Message
        ///     <T>
        ///         > that other objects have added to the _subscribers
        ///         data structure
        /// </summary>
        /// <param name="source">the source of the published message</param>
        /// <param name="messageData">the message data, used to pass information in the message.</param>
        /// <typeparam name="T">
        ///     The type of message data that is being sent, used to filter the _subscribers list to only Invoke
        ///     subscriptions of that message type.
        /// </typeparam>
        public void Publish<T>(object source, T messageData)
        {
            // if the message or source object is null, skip execution
            // TODO: review if this should throw an exception
            if (messageData == null || source == null) return;

            // of no subscriber has been added for this type of message, then
            // skip execution.
            if (!_subscribers.ContainsKey(typeof(T))) return;

            // get a list of all the delegates that are registered for this
            // type of message.
            var subscriptions = _subscribers[typeof(T)];
            var delegates = new List<Delegate>();
            foreach (var s in subscriptions) delegates.Add(s.SubscribedEvent);

            // possibly reduntant null check here, as the if(!_subscribers.ContainsKey(typeof(T)))
            // check above should catch both these code paths.
            // TODO: review removing this after adding tests (so we know it's not
            //       breaking it.
            if (delegates == null || delegates.Count == 0) return;

            // create a new Message<T> object wrapping the message data.
            var payload = new Message<T>(source, messageData);

            // lock the delegates list here, as it is a reference to the element in the _subscribers list, 
            // which can be modified outside of this thread.
            lock (delegates)
            {
                // get a list of all the Actions that have been subscribed
                var handlers = delegates.Select(item => item as Action<Message<T>>).ToList();

                // invoke all those actions.
                for (var i = 0; i < handlers.Count; i++)
                {
                    try
                    {
                        handlers[i]?.Invoke(payload);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Adds a message handler to the _subscriptions list, so that it can be invoked by other
        ///     processes.
        ///     As a note, T can be any object, but recommend creating a new [WhatItsFor]Message named object to use,
        ///     as this is what is used to index into the _subscribers list during publishing.
        /// </summary>
        /// <param name="subscription">An Action<Message<T>> to add to the subscription list</param>
        /// <param name="subscriptionType">Disposable subscriptions are cleared when scene unloads permanent subscriptions stay</param>
        /// <typeparam name="T">The message data object</typeparam>
        public void Subscribe<T>(Action<Message<T>> subscription, SubscriptionType subscriptionType = SubscriptionType.Disposable)
        {
            var subscriptionPackage = new Subscription(subscription, subscriptionType);

            // if the _subscribers dictionarry already contains T as a key, return that list,
            // otherwise create a new list that will be added to _subscribers later
            var subscriptions = _subscribers.ContainsKey(typeof(T)) ? _subscribers[typeof(T)] : new List<Subscription>();

            // if the delegates for this message type already contains THIS instance of
            // subscription, then we dont want to add a second one, so only add the 
            // subscription if it does not allready exist in the list.
            bool search = false;
            foreach (var s in subscriptions)
            {
                if (s.SubscribedEvent.GetHashCode() == subscription.GetHashCode())
                {
                    search = true;
                    break;
                }
            }
            if (search == false)
                subscriptions.Add(subscriptionPackage);

            // put the list into the dictionary (either creating a new key, or 
            // replacing the existing reference with itself
            _subscribers[typeof(T)] = subscriptions;
        }

        /// <summary>
        ///     Removes an Action<Message<T>> from the _subscribers dictionary.
        /// </summary>
        /// <param name="subscription">the Action to remove</param>
        /// <typeparam name="T">message data type to remove for</typeparam>
        public void Unsubscribe<T>(Action<Message<T>> subscription)
        {
            // if there are no subscriptions for this type of message, then
            // return here.
            // TODO: should this be an exception, or remain as a silent failure?
            if (!_subscribers.ContainsKey(typeof(T))) return;

            // get all the delegates for this message type
            var subscriptions = _subscribers[typeof(T)];

            // if the delegates contains the action we want to remove, then
            // remove it.
            List<Subscription> toRemove = new List<Subscription>();
            foreach (var s in subscriptions)
            {
                if (s.SubscribedEvent.GetHashCode() == subscription.GetHashCode())
                {
                    toRemove.Add(s);
                }
            }
            foreach (var s in toRemove)
            {
                subscriptions.Remove(s);
            }

            // if this was the last action in the list, then remove the list
            // and key from the _subscribers dictionarry.
            if (subscriptions.Count == 0) _subscribers.Remove(typeof(T));
        }

        private void UnsubscribeAllDisposable()
        {
            foreach (var subscriber in _subscribers)
            {
                var subscriptions = subscriber.Value;
                subscriptions.RemoveAll(item => item.SubscriptionType.Equals(SubscriptionType.Disposable));
            }
        }

        private void OnSceneUnLoad(Scene s)
        {
            UnsubscribeAllDisposable();
        }
    }
}