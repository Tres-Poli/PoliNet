namespace Editor
{
    using System.IO;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class FeatureGeneratorEditor : OdinEditorWindow
    {
        [MenuItem("Window/Feature Generator")]
        public static void OpenWindow()
        {
            GetWindow<FeatureGeneratorEditor>().Show();
        }

        [FolderPath] 
        public string featurePath;

        public string featureName;

        [Button("Generate")]
        public void Generate()
        {
            var absoluteDir = Path.Combine(Application.dataPath.Replace("Assets", string.Empty), featurePath, featureName);
            Directory.CreateDirectory(absoluteDir);
        }
    }
}