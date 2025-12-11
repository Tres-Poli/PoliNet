namespace Runtime.Application.Client
{
    using Poli.Boot;
    using StateMachine;
    using StateMachine.States;
    using StateMachine.States.Initialize;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/ApplicationClientLoader", fileName = "ApplicationClientLoader")]
    public sealed class ApplicationClientLoader : ScriptableLoader
    {
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            builder.Register<ClientDefaultStateProvider>(Lifetime.Singleton);
            builder.Register<ClientInitializeStateProvider>(Lifetime.Singleton);

            builder.Register<ClientStateProvider>(Lifetime.Singleton);

            // Startable
            
            // PostStartable
            builder.RegisterEntryPoint<ClientStateMachine>();
        }
    }
}