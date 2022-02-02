using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogueSystem;
using UnityEditor;
using UnityEngine;

public class GraphImporter : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (int i = 0; i < importedAssets.Length; i++)
        {
            string path = importedAssets[i];
            if (Path.GetExtension(path) != ".asset")
            {
                continue;
            }

            Graph graph = AssetDatabase.LoadAssetAtPath<Graph>(path);
            graph.Initalise();
        }
    }
}
