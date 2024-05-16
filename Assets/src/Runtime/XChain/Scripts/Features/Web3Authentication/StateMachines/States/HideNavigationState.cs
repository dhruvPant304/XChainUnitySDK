using Features.Web3Authentication.UIControllers;
using UnityEngine;
using XNode;
using XNodeStateMachine;
using static XNode.Node;

namespace Features.Web3Authentication.StateMachines.States
{
    public class HideNavigationState : State
    {
        [Output] public NodePort exit;

        NavigationUIController _navigationUIController;


        protected override void Enter()
        {
            _navigationUIController.Hide();
            ExitThroughNodePort("exit");
        }

        protected override void Exit()
        {
            
        }

        protected override void UpdateState()
        {
            
        }
    }
}
