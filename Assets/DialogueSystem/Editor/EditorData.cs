 using System.Collections;
using System.Collections.Generic;
 using DialogueEditor;
 using UnityEngine;

 [System.Serializable]
public class EditorData
{
    public string id;
    public DialoguesScriptable dialoguesScriptable;
    public NodeEditorData[] nodes;


    public EditorData(DialogueWindow window)
    {
        this.id = window.id;
        nodes = new NodeEditorData[window.nodes.Count];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new NodeEditorData(window.nodes[i]);
        }

        this.dialoguesScriptable = window.dialoguesScriptable;
    }
}

 [System.Serializable]
 public class NodeEditorData
 {
     public int id;
     public Rect rect;

     public NodeEditorData(NodeEditor node)
     {
         id = node.id;
         rect = node.rect;
     }
 }
 
 
 
 
 
