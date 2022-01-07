using System.IO;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class EditorManager : MonoBehaviour
    {
        public static NodeGraphWindow window;
        private static string FOLDER_NAME = "DialogueEditorData";
        private static string PATH = "/DialogueSystem/DialogueData";


        public static void OpenDialogueWindow(DialogueGraph dialogueGraph)
        {
            window = (NodeGraphWindow) EditorWindow.GetWindow(typeof(NodeGraphWindow));
            window.Initialise(dialogueGraph);
            window.Show();
        }
        
        [MenuItem("DialogueSystem/CreateDialogue")]
        public static void CreateData()
        {
            if (!Directory.Exists(Application.dataPath + "/DialogueSystem/Dialogues"))
            {
                Directory.CreateDirectory(Application.dataPath + "/DialogueSystem/Dialogues");
            }

            DialogueGraph dialogueScriptable = ScriptableObject.CreateInstance<DialogueGraph>();
            dialogueScriptable.Initalise(DeleteData);
            AssetDatabase.CreateAsset(dialogueScriptable,"Assets/DialogueSystem/Dialogues/"+dialogueScriptable.id+".asset");
            AssetDatabase.SaveAssets();
        
            Selection.activeObject = dialogueScriptable;
            EditorUtility.SetDirty(dialogueScriptable);
            
            
            
            string modifiedPath = Application.dataPath + PATH;

            if (!Directory.Exists(modifiedPath))
            {
                Directory.CreateDirectory(modifiedPath);
            }

            string filename = "/" + dialogueScriptable.id + ".json";
            System.IO.File.WriteAllText(modifiedPath + filename, string.Empty);
            AssetDatabase.Refresh();
        }
        
        public static void SaveData()
        {
            if (window != null)
            {
                string modifiedPath = Application.dataPath + PATH;

                if (!Directory.Exists(modifiedPath))
                {
                    Directory.CreateDirectory(modifiedPath);
                }

                EditorData data = new EditorData(window);

                string editorData = JsonUtility.ToJson(data, true);
                string filename = "/" + data.id + ".json";
                File.WriteAllText(modifiedPath + filename, editorData);
                EditorUtility.SetDirty(window.dialogueGraph);
            }
        }

        public static void LoadData(DialogueGraph dialogueScriptable)
        {
            string dataPath = Application.dataPath + PATH + "/" + dialogueScriptable.id + ".json";
            if (File.Exists(dataPath))
            {
                string rawData = File.ReadAllText(dataPath);
                EditorData data = JsonUtility.FromJson<EditorData>(rawData);
                if (data != null)
                {
                    //Loads Dialogues
                    foreach (NodeData nodeData in data.nodes)
                    {
                        foreach (TextDialogue dialogue in dialogueScriptable.dialogues)
                        {
                            if (nodeData.id == dialogue.id)
                            {
                                TextNode textNode = window.CreateNode(nodeData.rect.position, dialogue);
                                foreach (Option option in dialogue.options)
                                {
                                    textNode.AddOption(option);
                                }
                            }
                        }
                    }

                    //loadConnectors
                    foreach (TextDialogue dialogue in dialogueScriptable.dialogues)
                    {
                        global::Connection[] connectors = dialogueScriptable.GetConnections(dialogue);
                        if (connectors != null)
                        {
                            if (dialogue.options.Count > 0)
                            {
                                TextNode startTextNode = window.GetNode(dialogue);
                                foreach (global::Connection connector in connectors)
                                {
                                    ConnectionPort startPort = null;
                                    startPort = startTextNode.outPoints[connector.option.id ];
                                    
                                    TextNode endTextNode = window.GetNode(connector.endDialogue);
                                    ConnectionPort endPort = endTextNode.inPort;
                                    window.connections.Add(new Connection(startPort, endPort, window.RemoveConnection, connector.ID));
                                }
                            }
                            else
                            {
                                TextNode startTextNode = window.GetNode(dialogue);
                                TextNode endTextNode = window.GetNode(connectors[0].endDialogue);
                                ConnectionPort startPort = startTextNode.outPoint[0];
                                ConnectionPort endPort = endTextNode.inPort;
                                window.connections.Add(new Connection(startPort, endPort, window.RemoveConnection, connectors[0].ID));
                            }
                            
                        }
                        else
                        {
                            Debug.Log("connectors is null");
                        }
                    }
                }
                else
                {
                    Debug.Log("Json Data Not loaded");
                }
            }
            else
            {
                Debug.Log("File Does not Exist");
            }
        }

        public static void DeleteData(DialogueGraph dialogueScriptable)
        {
            string dataPath = Application.dataPath + PATH + "/" + dialogueScriptable.id + ".json";
            string dataPathMeta = Application.dataPath + PATH + "/" + dialogueScriptable.id + ".meta";
            if (File.Exists(dataPath))
            {
                File.Delete(dataPath);
                File.Delete(dataPathMeta);
            }
            
            AssetDatabase.Refresh();
        }

    }
}