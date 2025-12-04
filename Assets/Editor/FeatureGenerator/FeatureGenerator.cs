namespace Editor
{
    using System.IO;
    using System.Text;
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

        [Sirenix.OdinInspector.FilePath]
        public string aspectTemplatePath;
        [Sirenix.OdinInspector.FilePath]
        public string featureTemplatePath;
        [Sirenix.OdinInspector.FilePath]
        public string asmdefTemplatePath;

        [FolderPath] 
        public string featurePath;
        public string featureName;
        public bool forceOverwrite = false;

        public string featureNameVariable = "$FEATURE_NAME$";
        public string namespaceVariable = "$NAMESPACE$";

        [Button("Generate")]
        public void Generate()
        {
            var absoluteDir = $"{Application.dataPath.Replace("Assets", string.Empty)}{featurePath}/{featureName}";
            Debug.Log($"Path: {absoluteDir}");
            if (Directory.Exists(absoluteDir) && !forceOverwrite)
            {
                Debug.LogError("Cannot overwrite existing files for safety reasons");
                return;
            }

            Directory.CreateDirectory(absoluteDir);
            Directory.CreateDirectory($"{absoluteDir}\\Aspects");
            Directory.CreateDirectory($"{absoluteDir}\\Components");
            Directory.CreateDirectory($"{absoluteDir}\\Systems");
            
            var namespaceValue = GetNamespace(absoluteDir);
            Debug.Log($"Namespace: {namespaceValue}");
            var asmdefName = namespaceValue;
            
            var aspectFilePath = Path.Combine(absoluteDir, $"Aspects/{featureName}Aspects.cs");
            CreateFileFromTemplate(aspectFilePath, aspectTemplatePath, $"{namespaceValue}.Aspects");

            var featureFilePath = Path.Combine(absoluteDir, $"{featureName}Feature.cs");
            CreateFileFromTemplate(featureFilePath, featureTemplatePath, namespaceValue);
            
            var asmdefFilePath = Path.Combine(absoluteDir, $"{asmdefName}.asmdef");
            CreateFileFromTemplate(asmdefFilePath, asmdefTemplatePath, namespaceValue);
        }

        private void CreateFileFromTemplate(string filePath, string templatePath, string namespaceValue)
        {
            var templateString = File.ReadAllText(templatePath);
            templateString = templateString.Replace(featureNameVariable, featureName);
            templateString = templateString.Replace(namespaceVariable, namespaceValue);
            File.WriteAllText(filePath, templateString);
        }

        private string GetNamespace(string absoluteDir)
        {
            var pathSplit = absoluteDir.Split('/');
            var pathDividerIndex = 0;
            for (int i = 0; i < pathSplit.Length; i++)
            {
                if (pathSplit[i] == "Runtime")
                {
                    pathDividerIndex = i;    
                    break;
                }
            }

            var sb = new StringBuilder();
            for (int i = pathDividerIndex; i < pathSplit.Length; i++)
            {
                sb.Append(pathSplit[i]);
                sb.Append('.');
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}