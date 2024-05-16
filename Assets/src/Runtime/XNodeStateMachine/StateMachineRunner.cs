using Cysharp.Threading.Tasks;
using UnityEngine;

#if ZENJECT_INJECT_IN_STATES
using Zenject;
#endif


namespace XNodeStateMachine
{
    public class StateMachineRunner : MonoBehaviour
    {
        [SerializeField] private StateMachineGraph stateMachineGraph;
        [SerializeField] private bool autoPlay;
#if ZENJECT_INJECT_IN_STATES
        [Inject] private DiContainer _container;  
#endif
        public void SetStateMachine(StateMachineGraph graph)
        {
            stateMachineGraph = graph;
        }
        private void Start()
        {
            if(autoPlay)
                StartCoroutine(StartStateMachineInternal().ToCoroutine());
        }

        public void StartStateMachine()
        {
            StartCoroutine(StartStateMachineInternal().ToCoroutine());
        }

        private async UniTask StartStateMachineInternal()
        {
#if ZENJECT_INJECT_IN_STATES
            await stateMachineGraph.RunStateMachine(_container);
#else
            await stateMachineGraph.RunStateMachine();
#endif
            
        }
    }
}
