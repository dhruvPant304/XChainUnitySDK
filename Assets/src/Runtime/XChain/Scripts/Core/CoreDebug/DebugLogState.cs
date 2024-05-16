using System;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Core.CoreDebug
{
    public class DebugLogState : State
    {
        public enum LogType
        {
            Log,
            Error,
            Warning
        }
        [Output] public NodePort exit;

        [SerializeField] public LogType type = LogType.Log;
        [SerializeField] public string message;
        protected override void Enter()
        {
            switch (type)
            {
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                default:
                    Debug.Log(message);
                    break;
            }
            
            ExitThroughNodePort("exit");
        }

        protected override void Exit()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
           // throw new System.NotImplementedException();
        }
    }
}