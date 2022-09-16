using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

public class NodeLine : NodeComponent
{
    public NodeLine(Rect rect)
    {
        canvasRect = rect;
    }

    public void Draw()
    {
        EditorGUI.DrawRect(canvasRect,Color.gray);
    }
}
