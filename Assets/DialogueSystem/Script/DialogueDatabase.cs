using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using DialogueSystem;

[CreateAssetMenu(fileName = "Dialogue Database", menuName = "Dialogue/Create Dialogue DataBase")]
[System.Serializable]
public class DialogueDatabase : ScriptableObject
{
    [SerializeField] private List<ConversationGraph> _conversations;

    public bool ContainsConversations()
    {
        if (_conversations == null) { return false; }
        if (_conversations.Count == 0) { return false; }
        
        return true;
    }

    public List<ConversationGraph> GetAllConversations()
    {
        _conversations ??= new List<ConversationGraph>();
        return _conversations;
    }

    public ConversationGraph GetConversationGraphAtIndex(int p_index)
    {
        if (p_index < _conversations.Count)
        {
            return _conversations[p_index];
        }

        return null;
    }

    public ConversationGraph CreateNewConversation()
    {
        _conversations ??= new List<ConversationGraph>();
        
        AssetDatabase.StartAssetEditing();
        
        ConversationGraph graph = ScriptableObject.CreateInstance<ConversationGraph>();
        graph.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(graph, this);
        SaveManager.SaveData(graph);
        AssetDatabase.SaveAssets();
        
        graph.Initialise();
        
        AssetDatabase.StopAssetEditing();
        
        _conversations.Add(graph);

        return graph;
    }

    public void DeleteDialogue(ConversationGraph p_conv)
    {
        if (!_conversations.Contains(p_conv))
        {
            return;
        }
        
        _conversations.Remove(p_conv);
        DestroyImmediate(p_conv, this);
    }
}
