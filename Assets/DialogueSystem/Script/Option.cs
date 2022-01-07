using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class Option
{
    public string id;
    public string text;

    public Option()
    {
        id = "-1";
    }

    public Option(string id)
    {
        this.id = id;
    }
}
