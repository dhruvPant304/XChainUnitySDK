using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using XNodeEditor;
using XNodeStateMachine;
using XNodeStateMachine.Editor;

namespace Plugins.XNodeStateMachine.Editor
{
    [CustomNodeEditor(typeof(SubStateMachineState))]
    public class SubStateNodeMachineEditor : StateNodeEditor
    {
        private SubStateMachineState _subStateMachine;
        public override void OnBodyGUI()
        {
            if (_subStateMachine == null)
            {
                _subStateMachine = target as SubStateMachineState;
            }
            DrawNodes();
            serializedObject.Update();
            base.OnBodyGUI();
        }

        private void DrawNodes()
        {
            if(_subStateMachine.subState == null) return;
            HashSet<string> nodeSet = new HashSet<string>();
            foreach (var node in _subStateMachine.subState.nodes)
            {
                if (node is ExitState exitState)
                {
                    var exitNodeName = exitState.exitName;
                    if (string.IsNullOrWhiteSpace(exitNodeName)) exitNodeName = "exit";
                    nodeSet.Add(exitNodeName);
                    if (!NodeExists(exitNodeName))
                    {
                        _subStateMachine.AddDynamicOutput(typeof(NodePort), fieldName: exitNodeName);
                    }
                }
            }

            var dynamicOutputs = _subStateMachine.DynamicOutputs;
            foreach (var node in dynamicOutputs)
            {
                if (nodeSet.Contains(node.fieldName) == false)
                {
                    _subStateMachine.RemoveDynamicPort(node);
                }
            }
        }

        private bool NodeExists(string name)
        {
            var outputs = _subStateMachine.DynamicOutputs;
            foreach (var nodePort in outputs)
            {
                if (nodePort.fieldName == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}