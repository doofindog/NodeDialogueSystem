using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{
    public string id;
    public string text;
    public List<Option> options;

    public Dialogue()
    {
        id = DialoguesManager.GenerateUniqueId();
        options = new List<Option>();
    }

    public Option CreateOption()
    {
        Option option = new Option();
        options.Add(option);
        return option;
    }

    public void RemoveOption(Option option)
    {
        options.Remove(option);
    }


}
