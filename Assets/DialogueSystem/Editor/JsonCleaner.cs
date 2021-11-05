using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



//Files that adds in create
public class JsonCleaner : UnityEditor.AssetModificationProcessor
{
    private const string JSON_SAVE_PATH = "/DialogueSystem/DialogueData/";
    private const string ASSET_SAVE_PATH = "/DialogueSystem/Dialogues/";
    
    static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
    {
        string extension = Path.GetFileNameWithoutExtension(path) + ".json";
        string metaExtension = Path.GetFileNameWithoutExtension(path) + ".meta";
        string jsonPath = Application.dataPath + JSON_SAVE_PATH + extension;
        string jsonMetaPath = Application.dataPath + JSON_SAVE_PATH + metaExtension;

        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
            File.Delete(jsonMetaPath);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("Json file does not exist");
        }

        return AssetDeleteResult.DidNotDelete;
    }
    
}
