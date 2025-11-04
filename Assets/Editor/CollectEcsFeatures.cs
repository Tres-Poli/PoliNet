using System.Linq;
using Runtime.EcsSource;
using Runtime.LeoEcs;
using Runtime.Shared;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CollectEcsFeatures
{
    [MenuItem("PoliNet/Collect Ecs Systems", false, 0)]
    public static void Collect()
    {
        var featureCache = AssetDatabase.LoadAssetAtPath<Object>("Assets/Data/Cache/EcsFeatures.asset") as EcsFeaturesCached;
        if (!featureCache)
        {
            Debug.LogError("Missing cache file LeoEcsSystems");
            return;
        }

        featureCache.aspects.Clear();
        featureCache.features.Clear();

        var ecsAspectTypes = TypeCache.GetTypesDerivedFrom<IEcsAspect>().Where(t => t.IsClass);
        var ecsFeatureTypes = TypeCache.GetTypesDerivedFrom<IEcsFeature>().Where(t => t.IsClass);
        foreach (var type in ecsAspectTypes)
        {
            featureCache.aspects.Add(new SerializableType(type));
            Debug.Log($"Aspect {type.Name} added");
        }
        
        foreach (var type in ecsFeatureTypes)
        {
            featureCache.features.Add(new SerializableType(type));
            Debug.Log($"Feature {type.Name} added");
        }

        AssetDatabase.SaveAssets();
    }
}