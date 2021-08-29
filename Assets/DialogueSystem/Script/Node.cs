using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Node
{
    public int id;
    public string text;
    public List<Option> options;
    public List<Connector> connectors;
}
