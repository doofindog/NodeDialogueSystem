using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CachedData : MonoBehaviour
{
    private static Type[] _eventTypes;
    private static string[] _eventNames;

    private static Dictionary<Type, MethodInfo[]> _methods;
    private static Dictionary<Type, string[]> _methodNames;

    static CachedData()
    {
        _methods = new Dictionary<Type, MethodInfo[]>();
        _methodNames = new Dictionary<Type, string[]>();
        
        CacheEvents();
    }

    private static void CacheEvents()
    {
        _eventTypes = ReflectionHandler.GetDerivedTypes(typeof(DialogueEvents));
        _eventNames = (from type in _eventTypes select type.Name).ToArray();

        for (int i = 0; i < _eventTypes.Length; i++)
        {
            MethodInfo[] methodInfos = ReflectionHandler.GetMethods(_eventTypes[i], BindingFlags.Static | BindingFlags.Public);
            string[] names = (from method in methodInfos select method.Name).ToArray();
            
            _methods.Add(_eventTypes[i], methodInfos);
            _methodNames.Add(_eventTypes[i], names);
        }
    }

    public static Type[] GetEventTypes()
    {
        return _eventTypes;
    }

    public static Type GetEvent(string p_typeName)
    {
        for (int i = 0; i < _eventTypes.Length; i++)
        {
            if (_eventTypes[i].Name == p_typeName)
            {
                return _eventTypes[i];
            }
        }

        return null;
    }
    
    public static Type GetEvent(int p_index)
    {
        return _eventTypes[p_index];
    }

    public static string[] GetEventNames()
    {
        return _eventNames;
    }

    public static MethodInfo[] GetMethods(Type p_type)
    {
        return _methods[p_type];
    }

    public static string[] GetMethodNames(Type p_type)
    {
        return _methodNames[p_type] != null ? _methodNames[p_type] : null;
    }

    public static MethodInfo GetMethod(Type p_type, string p_methodName)
    {
        if (p_type == null) { return null; }
        if (string.IsNullOrEmpty(p_methodName)) { return null; }

        MethodInfo[] methodInfos = GetMethods(p_type);
        for (int i = 0; i < methodInfos.Length; i++)
        {
            if (methodInfos[i].Name.Equals(p_methodName))
            {
                return methodInfos[i];
            }
        }

        return null;
    }
}
