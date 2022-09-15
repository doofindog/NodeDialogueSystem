using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Database", menuName = "Dialogue/Create Dialogue DataBase")]
[System.Serializable]
public class DialogueDatabase : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private List<ConversationGraph> conversations;


    public bool ContainsConversations()
    {
        if (conversations == null) { return false; }
        if (conversations.Count == 0) { return false; }
        
        return true;
    }

    public List<ConversationGraph> GetAllConversations()
    {
        conversations ??= new List<ConversationGraph>();
        return conversations;
    }

    public ConversationGraph GetConversationGraphAtIndex(int index)
    {
        if (index < conversations.Count)
        {
            return conversations[index];
        }

        return null;
    }

    public ConversationGraph CreateNewConversation()
    {
        conversations ??= new List<ConversationGraph>();
        
        AssetDatabase.StartAssetEditing();
        
        ConversationGraph graph = ScriptableObject.CreateInstance<ConversationGraph>();
        graph.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(graph, this);
        SaveManager.SaveData(graph);
        AssetDatabase.SaveAssets();
        
        graph.Initialise();
        
        AssetDatabase.StopAssetEditing();
        
        conversations.Add(graph);

        return graph;
    }

    public void DeleteDialogue(ConversationGraph conv)
    {
        if (conversations.Contains(conv))
        {
            conversations.Remove(conv);
            DestroyImmediate(conv, this);
        }
    }
}
