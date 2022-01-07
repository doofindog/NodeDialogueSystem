
using UnityEngine;

namespace DialogueSystem.Editor
{
 [System.Serializable]
 public class EditorData
 {
  public string id;
  public DialogueGraph dialogueGraph;
  public NodeData[] nodes;


  public EditorData(NodeGraphWindow window)
  {
   if (window != null)
   {
    if (window.nodes != null)
    {
     this.id = window.id;

     nodes = new NodeData[window.nodes.Count];
     for (int i = 0; i < nodes.Length; i++)
     {
      nodes[i] = new NodeData(window.nodes[i]);
     }

     this.dialogueGraph = window.dialogueGraph;
    }
   }
  }
 }

 [System.Serializable]
 public class NodeData
 {
  public string id;
  public Rect rect;

  public NodeData(Node node)
  {
   id = node.id;
   rect = node.rect;
  }
 }

 [System.Serializable]
 public class ConnectionPointData
 {
  public string id;
  public Rect rect;
 }
}