namespace Editor.MessageMediatorGenerator
{
    using System.IO;
    using System.Text;
    using MessagePack;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class MessageMediatorGeneratorEditor : OdinEditorWindow
    {
        [MenuItem("Window/Message Mediator Generator")]
        public static void OpenWindow()
        {
            GetWindow<MessageMediatorGeneratorEditor>().Show();
        }

        [Sirenix.OdinInspector.FilePath] 
        public string mediatorContainerTemplatePath;
        
        [Sirenix.OdinInspector.FilePath]
        public string mediatorTemplatePath;

        [FolderPath] 
        public string outputPath;
        public string outputFileName = "MessageMediators";

        public string mediatorNameVariable = "$MEDIATOR_NAME$";
        public string namespaceVariable = "$NAMESPACE$";
        public string mediatorsVariable = "$MEDIATORS$";

        [Button("Generate")]
        public void Generate()
        {
            var absoluteDir = outputPath;
            Debug.Log($"Path: {absoluteDir}");
            
            var namespaceValue = GetNamespace(absoluteDir);
            Debug.Log($"Namespace: {namespaceValue}");
            
            var messageTypes = TypeCache.GetTypesWithAttribute<MessagePackObjectAttribute>();
            var sb = new StringBuilder();
            foreach (var type in messageTypes)
            {
                var mediatorCode = CreateMediatorCodeFromTemplate(mediatorTemplatePath, type.Name);
                sb.Append(mediatorCode);
                sb.AppendLine();
                sb.Append("\t");
            }

            var containerCode = File.ReadAllText(mediatorContainerTemplatePath);
            containerCode = containerCode.Replace(mediatorsVariable, sb.ToString());
            containerCode = containerCode.Replace(namespaceVariable, namespaceValue);

            File.WriteAllText($"{Path.Combine(outputPath, outputFileName)}.cs", containerCode);
        }

        private string CreateMediatorCodeFromTemplate(string templatePath, string mediatorName)
        {
            var templateString = File.ReadAllText(templatePath);
            templateString = templateString.Replace(mediatorNameVariable, mediatorName);
            return templateString;
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