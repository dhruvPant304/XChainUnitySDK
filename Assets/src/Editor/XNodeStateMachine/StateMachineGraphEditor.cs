using UnityEditor.Graphs;
using UnityEngine;
using XNode;
using XNodeEditor;
using XNodeStateMachine;
using XNodeStateMachine.NodePorts;

namespace XChainSDK.XChain.Plugins.XNodeStateMachine.Editor
{
    [CustomNodeGraphEditor(typeof(StateMachineGraph))]
    public class StateMachineGraphEditor : NodeGraphEditor
    {
        public override Gradient GetNoodleGradient(NodePort output, NodePort input)
        {
            if (!Application.isPlaying) return base.GetNoodleGradient(output, input);
            if (output.node is not State outputState) return base.GetNoodleGradient(output, input);
            if (input.node is not State inputState) return base.GetNoodleGradient(output, input);
            if (outputState.ExitedFrom != output & outputState is not Parallel) return base.GetNoodleGradient(output, input);
            if (!(inputState.isCompleted || inputState is Parallel || inputState is ExitState || inputState.isRunning))
                return base.GetNoodleGradient(output, input);
            if (!(outputState.isCompleted || outputState is StartState || outputState is Parallel))
                return base.GetNoodleGradient(output, input);
            
            Gradient grad = new Gradient();
            Color a = Color.yellow;
            if (window.hoveredPort == output || window.hoveredPort == input) {
                    a = Color.Lerp(a, Color.white, 0.8f);
            }
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(a, 0f), new GradientColorKey(a, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
            return grad;
        }
    }
}
