namespace Runtime.Application.Server
{
    using Poli.Boot;
    using StateMachine;
    using StateMachine.States.Initialize;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/ApplicationServerLoader", fileName = "ApplicationServerLoader")]
    public sealed class ApplicationServerLoader : ScriptableLoader
    {
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            builder.Register<ServerDefaultStateProvider>(Lifetime.Singleton);
            builder.Register<ServerInitializeStateProvider>(Lifetime.Singleton);

            builder.RegisterEntryPoint<ServerStateMachine>();
        }
    }
}