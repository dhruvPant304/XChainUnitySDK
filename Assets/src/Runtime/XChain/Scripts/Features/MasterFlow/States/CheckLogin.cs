using Core.DataProviders.SessionDataProvider;
using XNode;
using XNodeStateMachine;

namespace Features.MasterFlow.States
{
    public class CheckLogin : State
    {
        [Output] public NodePort yes;
        [Output] public NodePort no;

        private ISessionDataProvider _sessionDataProvider;
        protected override void Enter()
        {
            if (_sessionDataProvider.IsLoggedIn)
            {
                ExitThroughNodePort("yes");
            }
            else
            {
                ExitThroughNodePort("no");
            }
        }

        protected override void Exit()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }
    }
}