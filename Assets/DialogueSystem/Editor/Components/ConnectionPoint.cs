using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEditor;
using UnityEngine;

public class ConnectionPoint
{
    public Rect rect;
    public ConnectionPointType type;
    public NodeEditor node;
    private Action<ConnectionPoint> callBack;
    private bool isclick;

    private GUIStyle unClickedStyle;
    private GUIStyle ClickedStyle;

    public ConnectionPoint(ConnectionPointType type, Action<ConnectionPoint> callBack)
    {
        this.type = type;
        this.callBack = callBack;
    }

    public void Draw(Rect rect)
    {
        if (GUI.Button(rect, string.Empty))
        {
            isclick = true;
            callBack(this);
        }

        this.rect = rect;
    }
}

public enum ConnectionPointType
{
    IN,
    OUT,
}
