using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

[System.Serializable]
public class SerializableObjectVariable : ISerializationCallbackReceiver
{
    public object variableValue;
    public string variableName;
    public byte[] data;
    public SerializableType type;
    
    public SerializableObjectVariable(Type p_type, string p_varialbleName)
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
        if (data == null) {return;} 
        if (data.Length == 0) {return;}

        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        memoryStream.Write(data,0,data.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        variableValue = binaryFormatter.Deserialize(memoryStream);
    }

    public  void OnBeforeSerialize()
    {
        WriteData(variableValue);
    }

    public void OnAfterDeserialize()
    {
        ReadData();
    }
    
    public void SetObject(object p_obj)
    {
        variableValue = p_obj;
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


