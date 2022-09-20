using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class EventInfo
{
    [SerializeField] public SerializableType eventType;
    [SerializeField] public SerializableMethodInfo eventMethod;
    [SerializeField] public List<SerializableVariable> parametersObj;

    public EventInfo()
    {
        eventType = new SerializableType(null);
        eventMethod = new SerializableMethodInfo(null);
        parametersObj = new List<SerializableVariable>();
    }

    public void SetEventType(Type type)
    {
        eventType.SetType(type);
    }

    public SerializableMethodInfo SetMethodInfo(MethodInfo methodInfo)
    {
        eventMethod.SetMethodInfo(methodInfo);
        eventType.SetType(methodInfo.DeclaringType);
        UpdateParameters();
        return eventMethod;
    }
    public void UpdateParameters()
    {
        if(eventMethod.parameters==null) { return; }
        
        parametersObj.Clear();
        foreach (SerializableType param in eventMethod.parameters)
        {
            parametersObj.Add(new SerializableVariable(param.type));
        }
    }

    public List<SerializableVariable> GetParameters()
    {
        return parametersObj;
    }

    public void Invoke()
    { 
        if (eventMethod.methodInfo == null) { return; }

        eventMethod.methodInfo.Invoke(null, null);
    }

    public List<SerializableVariable> GetParameterObj()
    {
        return parametersObj;
    }
    
}
