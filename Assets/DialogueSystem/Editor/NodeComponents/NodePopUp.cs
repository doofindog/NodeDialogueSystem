using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

public class NodePopUp : NodeComponent
{
    private int label;
    private int _index;
    private string[] _content;

    public NodePopUp(Rect rect,int index, string[] content)
    {
        _index = index;
        _content = content;
        canvasRect = rect;
    }

    public int Draw()
    {
        return EditorGUI.Popup(canvasRect, _index, _content);
    }
}
