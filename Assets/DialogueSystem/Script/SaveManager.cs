using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEditor;

public class SaveManager 
{
    public static void SaveData(UnityEngine.Object p_target)
    {
        EditorUtility.SetDirty(p_target);
    }
}
