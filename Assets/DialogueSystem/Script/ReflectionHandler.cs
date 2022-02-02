using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReflectionHandler 
{
    public static Type[] GetDerivedTypes(Type baseType)
    {
        List<Type> types = new List<Type>();
        System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            try
            {
                types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return types.ToArray();
    }

    public static MethodInfo[] GetMethods(object target, BindingFlags flags)
    {
        MethodInfo[] infos = target.GetType().GetMethods(flags);
        return infos;
    }
}
