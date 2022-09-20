using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class SerializableVariable : ISerializationCallbackReceiver
{
    public object variableValue;
    public string variableName;
    public SerializableType type;
    public byte[] data;


    public SerializableVariable(Type p_type, string p_varialbleName)
    {
        type = new SerializableType(p_type);
        variableName = p_varialbleName;
    }

    public void WriteData(object p_object)
    {
        if (p_object == null)
        {
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, p_object);

        data = memoryStream.ToArray();
    }

    public void ReadData()
    {
        if (data == null)
        {
            return;
        }

        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        memoryStream.Write(data,0,data.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        variableValue = (object) binaryFormatter.Deserialize(memoryStream);
    }

    public  void OnBeforeSerialize()
    {
        WriteData(variableValue);
    }

    public void OnAfterDeserialize()
    {
        ReadData();
    }
    
    public void SetObject(object obj)
    {
        variableValue = obj;
        OnBeforeSerialize();
    }

    public object GetObject()
    {
        return variableValue;
    }

    public Type GetObjType()
    {
        return type.type;
    }
}


