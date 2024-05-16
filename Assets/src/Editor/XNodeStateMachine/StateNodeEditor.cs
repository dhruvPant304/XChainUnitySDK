using System;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using XNodeEditor;

namespace XNodeStateMachine.Editor
{
    [CustomNodeEditor(typeof(State))]
    public class StateNodeEditor : NodeEditor
    {
        private State _state;
        public override void OnBodyGUI()
        {
            if (_state == null) _state = target as State;
            serializedObject.Update();
            base.OnBodyGUI();
        }

        public override Color GetTint()
        {
            if (_state == null) _state = target as State;
            if (Application.isPlaying)
            {
                if (_state is ExitState || _state is Parallel)
                {
                    if (_state.GetPort("enter").Connection.node is State state)
                    {
                        var outPutPort = _state.GetPort("enter").Connection;
                        var isConnectedToExit = outPutPort == (outPutPort.node as State)?.ExitedFrom;
                        if (state.isCompleted && isConnectedToExit) return new Color(0f,0.5f,0f);
                    }
                }
                if(_state.isRunning) return new Color(0.58f, 0f, 0.83f);
                if (_state.isCompleted) return new Color(0f,0.5f,0f);
                else return new Color32(90, 97, 105, 255);
            }
            return base.GetTint();
        }
    }
}
