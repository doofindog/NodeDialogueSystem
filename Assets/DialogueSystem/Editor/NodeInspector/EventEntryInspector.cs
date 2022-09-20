using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventEntry))]
public class EventEntryInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
