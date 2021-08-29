using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEditor
{

    public void Draw(Rect rect,Action callBack)
    {
        if (GUI.Button(rect,string.Empty))
        {
            callBack();
        }
    }
}
