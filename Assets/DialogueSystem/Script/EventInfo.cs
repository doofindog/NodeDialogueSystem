using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

[System.Serializable]
public class EventInfo
{
    [SerializeField] public SerializableType eventType;
    [SerializeField] public SerializableMethodInfo eventMethod;
    [SerializeField] public List<SerializableObjectVariable> parametersObj;

    public EventInfo()
    {
        eventType = new SerializableType(null);
        eventMethod = new SerializableMethodInfo(null);
        parametersObj = new List<SerializableObjectVariable>();
    }

    public void SetEventType(Type type)
    {
        eventType.SetType(type);
    }

    public SerializableMethodInfo SetMethodInfo(MethodInfo p_methodInfo)
    {
        eventMethod.SetMethodInfo(p_methodInfo);
        eventType.SetType(p_methodInfo.DeclaringType);
        
        UpdateParameters();
        
        return eventMethod;
    }
    
    public void UpdateParameters()
    {
        parametersObj.Clear();
        
        if(eventMethod.parameters==null) { return; }

        ParameterInfo[] info = eventMethod.methodInfo.GetParameters();
        for (int i = 0; i < eventMethod.parameters.Count; i++)
        {
            string paramName = info[i].Name;
            Type paramType = info[i].ParameterType;
            
            parametersObj.Add(new SerializableObjectVariable(paramType, paramName));
        }
    }

    public List<SerializableObjectVariable> GetParameters()
    {
        return parametersObj;
    }

    public void Invoke()
    { 
        if (eventMethod.methodInfo == null) { return; }

        List<object> parameters = new List<object>();
        foreach (SerializableObjectVariable variable in parametersObj)
        {
            object param = variable.GetObject();
            parameters.Add(param);
        }
         
        eventMethod.methodInfo.Invoke(null, parameters.ToArray());
    }

    public List<SerializableObjectVariable> GetParameterObj()
    {
        return parametersObj;
    }
    
}
