namespace Runtime.Messaging
{
    using MessagePipe;
    using Messages.Application;
    using Poli.Boot;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/MessagingLoader", fileName = "MessagingLoader")]
    public sealed class MessagingLoader : ScriptableLoader
    {
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

            builder.RegisterMessageBroker<RequestUIScreenMessage>(options);
        }
    }
}