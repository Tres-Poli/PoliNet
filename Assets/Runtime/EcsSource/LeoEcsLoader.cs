namespace Runtime.LeoEcs
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Boot;
    using Cysharp.Threading.Tasks;
    using EcsSource;
    using Leopotam.EcsLite;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "PoliNet/Loaders/LeoEcsLoader", fileName = "LeoEcsLoader")]
    public sealed class LeoEcsLoader : ScriptableLoader
    {
        [SerializeField]
        public EcsFeaturesCached ecsFeaturesCached;
        
        private EcsWorld _world;
        private EcsSystemsShell _systems;

        private List<Type> _aspectTypes;
        
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            _world = new EcsWorld();
            _systems = new EcsSystemsShell(new EcsSystems(_world), scope);

            _aspectTypes = new List<Type>(64);

            builder.RegisterInstance(_world).AsSelf();
            
            foreach (var serializedAspect in ecsFeaturesCached.aspects)
            {
                var type = serializedAspect.GetType();
                builder.Register(type, Lifetime.Singleton).AsSelf();
                _aspectTypes.Add(type);
            }
            
            builder.RegisterBuildCallback(resolver =>
            {
                foreach (var serializedFeature in ecsFeaturesCached.features)
                {
                    var type =  serializedFeature.GetType(); 
                    var feature = Activator.CreateInstance(type) as IEcsFeature;
                    if (feature == null)
                    {
                        Debug.LogWarning($"{type} is not IEcsFeature");
                        continue;
                    }
                    
                    feature.InitializeFeature(_systems);
                }
                
                _systems.Init();
                foreach (var aspectType in _aspectTypes)
                {
                    var aspect = resolver.Resolve(aspectType) as IEcsAspect;
                    if (aspect == null)
                    {
                        Debug.LogWarning($"Cannot resolve aspect of type {aspectType}");
                        continue;
                    }

                    var fields = aspectType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (!typeof(IEcsPool).IsAssignableFrom(field.FieldType))
                        {
                            continue;
                        }
                        
                        var genericArgs = field.FieldType.GetGenericArguments();
                        var genericMethod = _world.GetType().GetMethod("GetPool")?.MakeGenericMethod(genericArgs[0]);
                        if (genericMethod == null)
                        {
                            continue;
                        }
                        
                        var pool = genericMethod.Invoke(_world, Array.Empty<object>());
                        field.SetValue(aspect, pool);
                    }
                }
                
                UpdateAsync().Forget();
            });
        }

        private async UniTaskVoid UpdateAsync()
        {
            await UniTask.Yield();
            
            while (_world != null && _world.IsAlive())
            {
                _systems.Run();
                await UniTask.Yield();
            }
        }
    }
}