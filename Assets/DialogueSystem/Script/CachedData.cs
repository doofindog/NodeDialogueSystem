using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class CachedData : MonoBehaviour
{
    public static Type[] eventTypes;
    public static Dictionary<Type, MethodInfo[]> methods;

    static CachedData()
    {
        CacheEvents();
    }

    private static void CacheEvents()
    {
        eventTypes = ReflectionHandler.GetDerivedTypes(typeof(DialogueEvents));
        foreach (Type type in eventTypes)
        {
            MethodInfo[] methodInfos = ReflectionHandler.GetMethods(type, BindingFlags.Static | BindingFlags.Public);
            methods.Add(type,methodInfos);
        }
    }

    public static Type[] GetEventTypes()
    {
        return eventTypes;
    }

    public static Type GetEvent(int index)
    {
        return eventTypes[index];
    }
}
