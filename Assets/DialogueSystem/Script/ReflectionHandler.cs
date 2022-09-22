using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReflectionHandler 
{
    public static Type[] GetDerivedTypes(Type p_baseType)
    {
        List<Type> types = new List<Type>();
        
        System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            try
            {
                types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && p_baseType.IsAssignableFrom(t)).ToArray());
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        return types.ToArray();
    }

    public static MethodInfo[] GetMethods(Type p_target, BindingFlags p_flags)
    {
        MethodInfo[] infos = p_target.GetMethods(p_flags);
        return infos;
    }
}
