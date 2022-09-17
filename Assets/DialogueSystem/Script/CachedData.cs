using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class CachedData : MonoBehaviour
{
    public static Type[] eventTypes;
    public static string[] eventNames;
    
    public static Dictionary<Type, MethodInfo[]> methods;
    public static Dictionary<Type, string[]> methodNames;

    static CachedData()
    {
        methods = new Dictionary<Type, MethodInfo[]>();
        methodNames = new Dictionary<Type, string[]>();
        CacheEvents();
    }

    private static void CacheEvents()
    {
        eventTypes = ReflectionHandler.GetDerivedTypes(typeof(DialogueEvents));
        eventNames = (from type in eventTypes select type.Name).ToArray();

        for (int i = 0; i < eventTypes.Length; i++)
        {
            MethodInfo[] methodInfos = ReflectionHandler.GetMethods(eventTypes[i], BindingFlags.Static | BindingFlags.Public);
            string[] names = (from method in methodInfos select method.Name).ToArray();
            
            methods.Add(eventTypes[i], methodInfos);
            methodNames.Add(eventTypes[i], names);
        }
    }

    public static Type[] GetEventTypes()
    {
        return eventTypes;
    }

    public static Type GetEvent(string typeName)
    {
        for (int i = 0; i < eventTypes.Length; i++)
        {
            if (eventTypes[i].Name == typeName)
            {
                return eventTypes[i];
            }
        }

        return null;
    }
    
    public static Type GetEvent(int index)
    {
        return eventTypes[index];
    }
    

    public static string[] GetEventNames()
    {
        return eventNames;
    }

    public static MethodInfo[] GetMethods(Type type)
    {
        return methods[type];
    }

    public static string[] GetMethodNames(Type type)
    {
        return methodNames[type] != null ? methodNames[type] : null;
    }

    public static MethodInfo GetMethod(Type type, string methodName)
    {
        if (type == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(methodName))
        {
            return null;
        }

        MethodInfo[] methodInfos = GetMethods(type);
        for (int i = 0; i < methodInfos.Length; i++)
        {
            if (methodInfos[i].Name.Equals(methodName))
            {
                return methodInfos[i];
            }
        }

        return null;
    }
}
