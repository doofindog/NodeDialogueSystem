using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public enum TypeContrain
    {
        POINT,
        TEXT,
        EVENT
    }
        
    public string id;
    public TypeContrain type;
}


