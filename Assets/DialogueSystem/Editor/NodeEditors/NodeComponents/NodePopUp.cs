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

    public NodePopUp(Rect p_rect,int index, string[] content)
    {
        _index = index;
        _content = content;
        rect = p_rect;
    }

    public int Draw()
    {
        return EditorGUI.Popup(rect, _index, _content);
    }
}