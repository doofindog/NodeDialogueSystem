using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connector 
{
    public Option option;
    public Dialogue toDialogue;
    
    public Connector(Option option, Dialogue toDialogue)
    {
        this.option = option;
        this.toDialogue = toDialogue;
    }
}
