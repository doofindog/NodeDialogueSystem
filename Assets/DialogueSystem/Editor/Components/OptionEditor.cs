using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionEditor
{
    public DialogueEditorConfig.OptionConfig config; 
    
    public OptionEditor()
    {
        config = new DialogueEditorConfig.OptionConfig();
    }

    public void Draw(Rect rect)
    {
        config.rect = rect;
        config.textEditor.Draw(config.rect);
    }

    public string GetText()
    {
        return config.textEditor.GetText();
    }
}
