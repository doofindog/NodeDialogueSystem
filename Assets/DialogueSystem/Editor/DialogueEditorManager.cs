
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using DialogueEditor;
using Newtonsoft.Json;


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

    public static void CreateData(DialoguesScriptable dialoguesScriptable)
    {
        string modifiedPath = Application.dataPath + path;

        if (!Directory.Exists(modifiedPath))
        {
            Directory.CreateDirectory(modifiedPath);
        }
        string filename = "/" + dialoguesScriptable.id + ".json";
        System.IO.File.WriteAllText(modifiedPath + filename,string.Empty);
    }


    public static void SaveData()
    {
        string modifiedPath = Application.dataPath + path;

        if (!Directory.Exists(modifiedPath))
        {
            Directory.CreateDirectory(modifiedPath);
        }

        EditorData data = new EditorData(window);

        string editorData = JsonUtility.ToJson(data,true);
        string filename = "/" + data.id + ".json";
        File.WriteAllText(modifiedPath + filename, editorData);
        EditorUtility.SetDirty(window.dialoguesScriptable);
    }

    public static void LoadData(DialoguesScriptable dialogueScriptable)
    {
        string dataPath = Application.dataPath + path + "/"+ dialogueScriptable.id + ".json";
        if (File.Exists(dataPath))
        {
            string rawData = File.ReadAllText(dataPath);
            EditorData data = JsonUtility.FromJson<EditorData>(rawData);
            if (data != null)
            {
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
        string dataPath = Application.dataPath + path + "/"+ dialogueScriptable.id + ".json";
        string dataPathMeta = Application.dataPath + path + "/" + dialogueScriptable.id + ".meta";
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            File.Delete(dataPathMeta);
        }
    }

}