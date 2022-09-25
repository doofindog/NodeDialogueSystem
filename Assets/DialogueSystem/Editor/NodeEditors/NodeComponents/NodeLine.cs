using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

public class NodeLine : NodeComponent
{
    public NodeLine(Rect p_rect)
    {
        rect = p_rect;
    }

    public void Draw()
    {
        EditorGUI.DrawRect(rect,Color.gray);
    }
}
