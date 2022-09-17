using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class EventInfo
{
    [SerializeField] public SerializableType eventType;
    [SerializeField] public SerializableMethodInfo eventMethod;


    public EventInfo()
    {
        eventType = new SerializableType(null);
        eventMethod = new SerializableMethodInfo(null);
    }

    public void SetEventType(Type type)
    {
        eventType.SetType(type);
    }

    public void SetMethodInfo(MethodInfo methodInfo)
    {
        eventMethod.SetMethodInfo(methodInfo);
        eventType.SetType(methodInfo.DeclaringType);
    }

}
