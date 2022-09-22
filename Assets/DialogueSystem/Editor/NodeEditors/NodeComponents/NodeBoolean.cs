using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEngine;

public class NodeBoolean : NodeComponent
{
    private bool _isTrue;
    private Rect _labelRect;
    private Rect _toggleRect;

    public NodeBoolean(Rect p_rect)
    {
        rect = p_rect;

        Vector2 labelPosition = p_rect.position;
        Vector2 labelSize = new Vector2()
        {
            x = p_rect.size.x * 0.3f,
            y = p_rect.size.y
        };
        _labelRect = new Rect(labelPosition, labelSize);

        Vector2 textPosition = new Vector2()
        {
            x = labelPosition.x + labelSize.x + 10,
            y = p_rect.position.y
        };
        Vector2 textSize = new Vector2()
        {
            x = p_rect.size.x * 0.65f,
            y = p_rect.size.y
        };
        _toggleRect = new Rect(textPosition, textSize);
    }

    public bool Draw(string p_lable,bool p_isTrue = false)
    {
        GUI.Label(_labelRect, p_lable);
        _isTrue = GUI.Toggle(_toggleRect, p_isTrue, string.Empty);

        return _isTrue;
    }
}
