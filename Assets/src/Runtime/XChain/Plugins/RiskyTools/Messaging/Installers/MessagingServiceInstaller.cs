using System.Runtime.CompilerServices;
using RiskyTools.Messaging.Awaiters;
using RiskyTools.Messaging.Interfaces;
using RiskyTools.Messaging.Services;
//using Zenject;

[assembly: InternalsVisibleTo("RiskyTools.Tests.Messaging.PlayMode")]
[assembly: InternalsVisibleTo("RiskyTools.Tests.Messaging.EditMode")]

namespace RiskyTools.Messaging.Installers
{
    /*public class MessagingServiceInstaller : Installer<MessagingServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessagingService>()
                .To<MessagingService>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<IThreadBlockMessageAwaiter>()
                .To<ThreadBlockMessageAwaiter>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<IWaitUntilMessageAwaiter>()
                .To<WaitUntilMessageAwaiter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        public static void Install(DiContainer Container)
        {
            Container.Bind<IMessagingService>()
                .To<MessagingService>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<IThreadBlockMessageAwaiter>()
                .To<ThreadBlockMessageAwaiter>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<IWaitUntilMessageAwaiter>()
                .To<WaitUntilMessageAwaiter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }*/
}