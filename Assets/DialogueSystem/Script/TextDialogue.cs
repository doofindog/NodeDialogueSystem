using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TextDialogue : Dialogue
{
    public string text;
    public List<Option> options;
    
    public TextDialogue()
    {
        options = new List<Option>();
        type = TypeContrain.TEXT;
    }

    public Option CreateOption()
    {
        Option option = new Option((options.Count+1).ToString());
        options.Add(option);
        return option;
    }
    
    public void RemoveOption(Option option)
    {
        options.Remove(option);
    }
}
