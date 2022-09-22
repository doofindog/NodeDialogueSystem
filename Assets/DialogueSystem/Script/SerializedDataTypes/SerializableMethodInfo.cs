using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class SerializableMethodInfo : ISerializationCallbackReceiver
{
    public MethodInfo methodInfo;
    public SerializableType type;
    public List<SerializableType> parameters = null;
    public string methodName;
    public int flags = 0;

    public SerializableMethodInfo(MethodInfo p_methodInfo)
    {
        methodInfo = p_methodInfo;
    }

    public void SetMethodInfo(MethodInfo p_methodInfo)
    {
        methodInfo = p_methodInfo;
        OnBeforeSerialize();
    }

    public void OnBeforeSerialize()
    {
        if (methodInfo == null) {return;}
        
        type = new SerializableType(methodInfo.DeclaringType);
        methodName = methodInfo.Name;
        
        if (methodInfo.IsPrivate)
            flags |= (int)BindingFlags.NonPublic;
        else
            flags |= (int)BindingFlags.Public;
        
        if (methodInfo.IsStatic)
            flags |= (int)BindingFlags.Static;
        else
            flags |= (int)BindingFlags.Instance;
        
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        
        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            parameters = new List<SerializableType>(parameterInfos.Length);
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameters.Add(new SerializableType(parameterInfos[i].ParameterType));
            }
        }
        else
            parameters = null;
    }

    public void OnAfterDeserialize()
    {
        if (type == null || string.IsNullOrEmpty(methodName)) {return;}
        
        System.Type[] param = null;
        
        Type t = type.type;
        if (parameters != null && parameters.Count>0)
        {
            param = new System.Type[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                param[i] = parameters[i].type;
            }
        }
        
        if (param == null)
            methodInfo = t.GetMethod(methodName, (BindingFlags)flags);
        else
            methodInfo = t.GetMethod(methodName, (BindingFlags)flags,null, param, null);
    }
}
