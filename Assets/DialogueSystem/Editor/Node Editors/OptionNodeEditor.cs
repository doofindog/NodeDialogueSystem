using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Editor;
using UnityEngine;

public class OptionNodeEditor : NodeEditor
{
    public OptionNode optionNode;

    public override void Init(Node node, GraphWindow graphWindow)
    {
        base.Init(node, graphWindow);
        optionNode = (OptionNode) node;
    }

    public override void Draw()
    {
        base.Draw();
        spacing = new Vector2(0, 0);

        DrawDialogueText(45);
        DrawOptions(20);
        DrawInPorts();
    }

    private void DrawDialogueText(float length)
    {
        Vector2 position = rect.position + padding + spacing;
        Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
        optionNode.text = GUI.TextArea(new Rect(position, size), optionNode.text);
        spacing.y += length + 10;
    }

    private void DrawOptions(float length)
    {
        Option[] options = optionNode.GetOptions();
        foreach (Option option in options)
        {
            Vector2 position = rect.position + spacing + padding;
            Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
            Rect optionRect = new Rect(position, size);

            option.text = GUI.TextArea(optionRect, option.text);

            spacing.y += length + 5;
        }
    }


}
