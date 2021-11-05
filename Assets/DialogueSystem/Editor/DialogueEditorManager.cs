
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using DialogueEditor;
using Newtonsoft.Json;
using UnityEditor.Graphs;

namespace DialogueEditor
{
    public class DialogueEditorManager : MonoBehaviour
    {
        private static DialogueWindow window;
        private static string folderName = "DialogueEditorData";
        private static string path = "/DialogueSystem/DialogueData";


        public static void OpenDialogueWindow(DialoguesScriptable dialoguesScriptable)
        {

            window = (DialogueWindow) EditorWindow.GetWindow(typeof(DialogueWindow));
            window.Initialise(dialoguesScriptable);
            window.Show();
        }
        
        [MenuItem("DialogueSystem/CreateDialogue")]
        public static void CreateData()
        {
            if (!Directory.Exists(Application.dataPath + "/DialogueSystem/Dialogues"))
            {
                Directory.CreateDirectory(Application.dataPath + "/DialogueSystem/Dialogues");
            }

            DialoguesScriptable dialogueScriptable = ScriptableObject.CreateInstance<DialoguesScriptable>();
            dialogueScriptable.Initalise(DeleteData);
            AssetDatabase.CreateAsset(dialogueScriptable,"Assets/DialogueSystem/Dialogues/"+dialogueScriptable.id+".asset");
            AssetDatabase.SaveAssets();
        
            Selection.activeObject = dialogueScriptable;
            EditorUtility.SetDirty(dialogueScriptable);
            
            
            
            string modifiedPath = Application.dataPath + path;

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
            string modifiedPath = Application.dataPath + path;

            if (!Directory.Exists(modifiedPath))
            {
                Directory.CreateDirectory(modifiedPath);
            }

            EditorData data = new EditorData(window);

            string editorData = JsonUtility.ToJson(data, true);
            string filename = "/" + data.id + ".json";
            File.WriteAllText(modifiedPath + filename, editorData);
            EditorUtility.SetDirty(window.dialoguesScriptable);
        }

        public static void LoadData(DialoguesScriptable dialogueScriptable)
        {
            string dataPath = Application.dataPath + path + "/" + dialogueScriptable.id + ".json";
            if (File.Exists(dataPath))
            {
                string rawData = File.ReadAllText(dataPath);
                EditorData data = JsonUtility.FromJson<EditorData>(rawData);
                if (data != null)
                {
                    //Loads Dialogues
                    foreach (NodeEditorData nodeData in data.nodes)
                    {
                        foreach (Dialogue dialogue in dialogueScriptable.dialogues)
                        {
                            if (nodeData.id == dialogue.id)
                            {
                                NodeEditor node = window.CreateNode(nodeData.rect.position, dialogue);
                                foreach (Option option in dialogue.options)
                                {
                                    node.AddOption(option);
                                }
                            }
                        }
                    }

                    foreach (Dialogue dialogue in dialogueScriptable.dialogues)
                    {
                        Connector[] connectors = dialogueScriptable.GetConnections(dialogue);
                        NodeEditor startNode = window.GetNode(dialogue);

                        foreach (Connector connector in connectors)
                        {
                            NodeEditor endNode = window.GetNode(connector.toDialogue);
                            ConnectionPoint inConnectionPoint = startNode.optionConnectionPoints[connector.option];
                            ConnectionPoint outConnectionPoint = endNode.inPoint;

                            window.connections.Add(new ConnectionEditor(inConnectionPoint, outConnectionPoint));
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

        public static void DeleteData(DialoguesScriptable dialogueScriptable)
        {
            Debug.Log("called");
            string dataPath = Application.dataPath + path + "/" + dialogueScriptable.id + ".json";
            string dataPathMeta = Application.dataPath + path + "/" + dialogueScriptable.id + ".meta";
            if (File.Exists(dataPath))
            {
                File.Delete(dataPath);
                File.Delete(dataPathMeta);
            }
            
            AssetDatabase.Refresh();
        }

    }
}