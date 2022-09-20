using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEngine;

public class NodeBoolean : NodeComponent
{
    private bool _isTrue;
    
    private Rect _labelRect;
    private Rect _toggleRect;

    public NodeBoolean(Rect rect)
    {
        canvasRect = rect;

        Vector2 labelPosition = rect.position;
        Vector2 labelSize = new Vector2()
        {
            x = rect.size.x * 0.3f,
            y = rect.size.y
        };
        _labelRect = new Rect(labelPosition, labelSize);

        Vector2 textPosition = new Vector2()
        {
            x = labelPosition.x + labelSize.x + 10,
            y = rect.position.y
        };
        Vector2 textSize = new Vector2()
        {
            x = rect.size.x * 0.65f,
            y = rect.size.y
        };
        _toggleRect = new Rect(textPosition, textSize);
    }

    public bool Draw(bool isTrue = false)
    {

        GUI.skin.textArea.wordWrap = true;
        GUI.Label(_labelRect, "boolean");
        _isTrue = GUI.Toggle(_toggleRect, isTrue, string.Empty);
        GUI.skin.textArea.wordWrap = false;

        return _isTrue;
    }
}
