using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Features.Web3Authentication.StateMachines;
using Features.Web3Authentication.StateMachines.Interfaces;
using Features.Web3Authentication.UIControllers;
using UnityEngine;

/*public class Web3AuthInstaller : MonoInstaller<Web3AuthInstaller>
{
    public override void InstallBindings()
    {
        InstallUIControllers();
        InstallStateMachine();
    }

    private void InstallUIControllers()
    {
        Container.Bind<LandingPageUIController>().FromComponentInHierarchy()
            .AsCached()
            .NonLazy();

        Container.Bind<SignUpPageUIController>().FromComponentInHierarchy()
            .AsCached()
            .NonLazy();

        Container.Bind<WaitAuthenticationUIController>().FromComponentInHierarchy()
            .AsCached()
            .NonLazy();

        Container.Bind<NavigationUIController>().FromComponentInHierarchy()
            .AsCached()
            .NonLazy();

        Container.Bind<MessagePopUpUIController>().FromComponentInHierarchy()
            .AsCached()
            .NonLazy();
    }

    private void InstallStateMachine()
    {
        Container.Bind<IWeb3AuthFlowBlackBoard>().To<Web3AuthFlowBlackBoard>().FromNew()
            .AsCached()
            .NonLazy();
    }
}*/