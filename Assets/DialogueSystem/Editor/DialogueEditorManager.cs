
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
        System.IO.File.WriteAllText(modifiedPath + filename, editorData);
    }

    public static void LoadData(DialoguesScriptable dialogue)
    {
        string dataPath = Application.dataPath + path + "/"+ dialogue.id + ".json";
        if (File.Exists(dataPath))
        {
            string rawData = System.IO.File.ReadAllText(dataPath);
            Debug.Log(rawData);
            EditorData data = JsonUtility.FromJson<EditorData>(rawData);
            if (data != null)
            {
                foreach (NodeEditorData nodeData in data.nodes)
                {
                    window.AddNode(nodeData.rect.position);
                }
            }
            else
            {
                Debug.Log("Json Data Not loaded");
            }
        }
    }

}

public class RandomeTestClass
{
    public int _num;
    private string _name;
    public Vector2 _vector;

    public RandomeTestClass()
    {
        _num = 5;
        _name = "Aaron";
        _vector = new Vector2(0, 0);
    }
}