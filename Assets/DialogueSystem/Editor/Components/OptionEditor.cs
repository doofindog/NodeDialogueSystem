using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionEditor
{
    public Rect rect;
    public DialogueEditor.TextEditor optionText;


    public OptionEditor()
    {
        optionText = new DialogueEditor.TextEditor();
    }

    public void Draw(Rect rect)
    {
        this.rect = rect;
        optionText.Draw(rect);
    }

    public string GetText()
    {
        return optionText.GetText();
    }
}
