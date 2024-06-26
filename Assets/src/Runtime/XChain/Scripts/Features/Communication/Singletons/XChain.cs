﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Features.Communication.Messages;
using Core.API;
using Core.API.APIResponse;
using Core.App;
using Cysharp.Threading.Tasks;
using Features.BuyXFlow.UIControllers;
using Features.Communication.Enums;
using Features.Communication.Types;
using RiskyTools.Messaging;
using RiskyTools.Messaging.Interfaces;
using RiskyTools.Messaging.Services;
using UnityEngine;
using XNodeStateMachine;

namespace Features.Communication.Singletons
{
    public class XChain : Singleton<XChain>
    {
        private AppConfig _appConfig;
        private readonly IMessagingService _messagingService = new MessagingService();
        private IAPIService _apiService;
        public IAPIService APIService => _apiService;
        public IMessagingService MessagingService => _messagingService;
        public AppConfig AppConfig => _appConfig;
        
        private float _xTokenBalance;
        private bool _loggedIn;

        public float XTokenBalance {get => _xTokenBalance; set {
            _xTokenBalance = value;
            OnXTokenBalanceChanged?.Invoke(_xTokenBalance);
        }}
        public bool LoggedIn {get =>  _loggedIn; set => _loggedIn = value;}



        protected override void Init(){
            //Load config data
            _appConfig = AppConfig.LoadData();
            _apiService = new APIService(_appConfig.networkSettings);
            _messagingService.Subscribe<XChainEventMessage>(OnXChainMessageEventReceived, SubscriptionType.Permanent);
            
            //Initialize the XChain Canvas
            LaunchStateMachine();
            InitializeXChainCanvas();
        }

        private void InitializeXChainCanvas(){
            var canvasPrefab = Resources.Load<XChainCanvas>("XChain/Prefabs/XChainCanvas");
            var canvas = Instantiate(canvasPrefab);
            canvas.transform.SetParent(transform);
        }

        private void LaunchStateMachine()
        {
            var stateMachineRunner = new GameObject("XChainStateMachine").AddComponent<StateMachineRunner>();
            DontDestroyOnLoad(stateMachineRunner.gameObject);
            try
            {
                var masterFlowGraph = Resources.Load<StateMachineGraph>("XChain/MasterFlow");
                stateMachineRunner.SetStateMachine(masterFlowGraph);
                stateMachineRunner.StartStateMachine();
            }
            catch
            {
                throw new Exception("Can't Find 'MasterFlow' State Machine graph in Resources folder, Try re-installing the XChain SDK again");
            }
        }

        public async UniTask<bool> Login(string accessToken, string accessKey, UserDetails userData){
            Context.SessionContext.AccessToken = accessToken;
            Context.Web3Context.AccessKey = accessKey;
            Context.Web3Context.UserData = userData;
            
            return await FetchXTokenBalance();
        }

        #region Data

        public XChainContext Context = new();
        public XChainContext GetContext(){
            return Context;
        }

        #endregion
        
        #region Events

        private class Subscription
        {
            public readonly Action<XChainContext> Callback;
            public bool ExecuteOnce;
            public Subscription(Action<XChainContext> callback, bool executeOnce)
            {
                this.Callback = callback;
                this.ExecuteOnce = executeOnce;
            }
        }
        
        private readonly Dictionary<XChainEvents, bool> _isEventWaitingToBeHandled = new Dictionary<XChainEvents, bool>();
        private readonly Dictionary<XChainEvents, List<Subscription>> _subscribers = new Dictionary<XChainEvents, List<Subscription>>();

        private void SendXChainEvent(XChainEvents eventType)
        {
            _isEventWaitingToBeHandled[eventType] = true;
        }
        
        public static async UniTask WaitXChainEvent(XChainEvents chainEvent, bool consumeSignal, CancellationToken token = new CancellationToken())
        {
            await Instance.WaitXChainEventInternal(chainEvent, consumeSignal, token);
        }

        private async UniTask WaitXChainEventInternal(XChainEvents chainEvent, bool consumeSignal, CancellationToken token = new CancellationToken())
        {
            var task = WaitEvent(chainEvent,token);
            if(_waitEventList.ContainsKey(chainEvent))
                _waitEventList[chainEvent].Add(task);
            else
            {
                _waitEventList.Add(chainEvent, new List<UniTask>(){task});
            }
            await UniTask.WaitUntil(() =>
            {
                bool allComplete = true;
                foreach (var uniTask in _waitEventList[chainEvent])
                {
                    allComplete &= uniTask.GetAwaiter().IsCompleted;
                }

                return allComplete;
            }, cancellationToken: token);
            if(token.IsCancellationRequested) return;
            if (consumeSignal) _isEventWaitingToBeHandled[chainEvent] = false;
            _waitEventList[chainEvent] = new List<UniTask>();
        }

        private readonly Dictionary<XChainEvents,List<UniTask>> _waitEventList = new Dictionary<XChainEvents, List<UniTask>>();

        private async UniTask WaitEvent(XChainEvents chainEvent, CancellationToken token = new CancellationToken())
        {
            await UniTask.WaitUntil(() => _isEventWaitingToBeHandled.ContainsKey(chainEvent), cancellationToken: token);
            await UniTask.WaitUntil(() => _isEventWaitingToBeHandled[chainEvent], cancellationToken: token);
        }
        
        
        public static async UniTask<List<XChainEvents>> WaitAnyEventExcept(XChainEvents eventType, bool consumeSignal, CancellationToken token = new CancellationToken())
        {
            var eventsReceived = new List<XChainEvents>();
            while (eventsReceived.Contains(eventType) == false)
            {
                eventsReceived = await Instance.WaitAnyEventInternal(token);
            }

            if (!consumeSignal) return eventsReceived;
            if (token.IsCancellationRequested) return eventsReceived;
            foreach (var received in eventsReceived)
            {
                Instance._isEventWaitingToBeHandled[received] = false;
            }
            return eventsReceived;
        }

        public static async UniTask<List<XChainEvents>> WaitAnyEvent(bool consumeSignal, CancellationToken token = new CancellationToken())
        {
            var queued  = await Instance.WaitAnyEventInternal(token);
            if (!consumeSignal) return queued;
            if (token.IsCancellationRequested) return queued;
            foreach (var e in queued)
            {
                Instance._isEventWaitingToBeHandled[e] = false;
            }
            return queued;
        }

        private async UniTask<List<XChainEvents>> WaitAnyEventInternal(CancellationToken token = new CancellationToken())
        {
            List<XChainEvents> queuedEvents = new List<XChainEvents>();
            await UniTask.WaitUntil(() =>
            {
                if (_isEventWaitingToBeHandled.Count == 0) return false;
                foreach (var queued in _isEventWaitingToBeHandled.Where((pair) => pair.Value))
                {
                    queuedEvents.Add(queued.Key);
                }

                return queuedEvents.Count != 0;
            }, cancellationToken: token);
            if(token.IsCancellationRequested)return new List<XChainEvents>();
            return queuedEvents;
        }
        
        private void OnXChainMessageEventReceived(Message<XChainEventMessage> message)
        {
            OnEventReceived(message.Payload.eventType);
        }

        private void OnEventReceived(XChainEvents eventType)
        {
            SendXChainEvent(eventType);
            if (!Instance._subscribers.ContainsKey(eventType)) return;
            foreach (var sub in Instance._subscribers[eventType])
            {
                sub.Callback?.Invoke(Instance.Context);
            }
            Instance._subscribers[eventType] = Instance._subscribers[eventType].Where((sub) => !sub.ExecuteOnce).ToList();
        }

        public static void OnEvent(XChainEvents eventType, Action<XChainContext> action)
        {
            SubscribeToEvent(eventType,action, true);
        }
        
        public static void SubscribeToEvent(XChainEvents eventType, Action<XChainContext> action, bool executeOnce = false)
        {
            var sub = new Subscription(action, executeOnce);
            if (Instance._subscribers.TryGetValue(eventType, out var subscriber))
            {
                subscriber.Add(sub);
            }
            else
            {
                Instance._subscribers.Add(eventType, new List<Subscription>(){sub});
            }
        }

        private static bool CheckIfAlreadySubscribed(XChainEvents eventType, Action<XChainContext> action)
        {
            if (!Instance._subscribers.ContainsKey(eventType)) return false;
            foreach (var del in Instance._subscribers[eventType])
            {
                if (del.GetHashCode() == action.GetHashCode()) return true;
            }
            return false;
        }

        public static void UnSubscribeToXChainEvent(XChainEvents eventType, Action<XChainContext> action)
        {
            if (Instance._subscribers.ContainsKey(eventType))
            {
                Instance._subscribers[eventType] = Instance._subscribers[eventType].Where((sub) => sub.Callback != action).ToList();
            }
        }
        
        public static bool IsEventWaiting(XChainEvents chainEvent)
        {
            if (Instance._isEventWaitingToBeHandled.ContainsKey(chainEvent) == false) return false;
            var result = Instance._isEventWaitingToBeHandled[chainEvent];
            Instance._isEventWaitingToBeHandled[chainEvent] = false;
            return result;
        }

        #endregion

        #region XToken Balance
        
        private static ExchangeNetwork GetExchangeNetworkByName(string networkName){
             try{
                var network = Instance
                    .Context
                    .BuyXContext
                    .exchangeNetworks.Find((e) => e.name == networkName);
                return network;
             }
             catch(Exception e){
                throw new Exception($"Failed to find network with name {networkName}: {e}");
             }
        }
        #endregion

        #region X CHAIN API
        public static void StartLoginFlow() => Instance.SendXChainEvent(XChainEvents.StartLogin);
        public static void StartBuyXFlow() => Instance.SendXChainEvent(XChainEvents.StartBuyXFlow);
        public static void EndBuyXFlow() => Instance.SendXChainEvent(XChainEvents.EndBuyXFlow);
        public static void StartAndWaitLoginCompletion() => Instance.SendXChainEvent(XChainEvents.StartAndWaitLoginCompletion);
        public static void CancelOperation() => Instance.OnEventReceived(XChainEvents.CancelOperation);
        /// <summary>
        /// Invoked when XToken Balance is update
        /// </summary>
        ///<value> new x token balance </value> 
        public event Action<float> OnXTokenBalanceChanged;
        
        /// <summary>
        /// Fetch X token Balance for the user from server, ensure that the method
        /// is called after login 
        /// </summary>
        public static async UniTask<bool> FetchXTokenBalance(){
            var xChainNetwork = GetExchangeNetworkByName("x-chain");
            var authToken = XChain.Instance.Context.SessionContext.AccessToken;
            var chainId = xChainNetwork.chainId;
            var tokenUUID = xChainNetwork.GetCurrency("X-Token").id;
            var response = await XChain.Instance.APIService.GetBalance(chainId, tokenUUID, authToken);
            if(response.IsSuccess){
                var successResponse = response.SuccessResponse;
                Instance.XTokenBalance = successResponse.balance;
            }
            return response.IsSuccess;
        }
        #endregion

    }
}