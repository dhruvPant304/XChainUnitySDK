using System.Linq;
using UnityEngine;
using XNode;
using XNodeEditor;
using XNodeStateMachine;
using XNodeStateMachine.Editor;

namespace Plugins.XNodeStateMachine.Editor
{
    [CustomNodeEditor(typeof(Parallel))]
    public class ParallelNodeNodeEditor : StateNodeEditor
    {
        private Parallel _parallelNode;

        public override void OnBodyGUI()
        {
            if (_parallelNode == null)
            {
                _parallelNode = target as Parallel;
            }
            
            NodeEditorGUILayout.PortField(_parallelNode.GetInputPort("enter"));
            int branchCount = _parallelNode.DynamicOutputs.Count();
            
            if (GUILayout.Button("Add"))
            {
                _parallelNode.AddDynamicOutput(typeof(NodePort), fieldName: $"Branch_{branchCount}");
            }
            if (GUILayout.Button("Remove"))
            {
                if (branchCount > 0)
                {
                    _parallelNode.RemoveDynamicPort(_parallelNode.DynamicOutputs.Last());  
                }
            }
            foreach (var dynamicOutput in _parallelNode.DynamicOutputs)
            {
                NodeEditorGUILayout.PortField(dynamicOutput);
            }
            serializedObject.Update();
        }
    }
}
