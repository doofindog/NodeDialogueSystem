using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class SerializableType : ISerializationCallbackReceiver, IEqualityComparer<SerializableType>
{
    public System.Type type;
    public string typeName;
    public byte[] data;
    
    public SerializableType(System.Type p_type)
    {
        if (p_type == null)
        {
            return;
        }

        type = p_type;
        typeName = p_type.Name;
    }

    public void SetType(Type p_type)
    {
        type = p_type;
        typeName = p_type.Name;
        
        OnBeforeSerialize();
    }

    public static System.Type Read(BinaryReader p_reader)
    {
        byte paramCount = p_reader.ReadByte();
        if (paramCount == 0xFF)
        {
            return null;
        }
        
        string typeName = p_reader.ReadString();
        Type type = System.Type.GetType(typeName);
        
        if (type == null)
        {
            Debug.LogError("Can't find type; '" + typeName + "'");
        }

        if (type.IsGenericTypeDefinition && paramCount > 0)
        {
            Type[] p = new System.Type[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                p[i] = Read(p_reader);
            }
            
            type = type.MakeGenericType(p);
        }
        return type;
    }

    public static void Write(BinaryWriter p_writer, System.Type p_type)
    {
        if (p_type == null)
        {
            p_writer.Write((byte)0xFF);
            return;
        }
        
        if (p_type.IsGenericType)
        {
            Type t = p_type.GetGenericTypeDefinition();
            Type[] p = p_type.GetGenericArguments();
            
            p_writer.Write((byte)p.Length);
            p_writer.Write(t.AssemblyQualifiedName);
            
            for (int i = 0; i < p.Length; i++)
            {
                Write(p_writer, p[i]);
            }
            return;
        }
        p_writer.Write((byte)0);
        p_writer.Write(p_type.AssemblyQualifiedName);
    }
    
    public void OnBeforeSerialize()
    {
        using (MemoryStream stream = new MemoryStream())
        using (BinaryWriter w = new BinaryWriter(stream))
        {
            Write(w, type);
            data = stream.ToArray();
        }
    }

    public void OnAfterDeserialize()
    {
        using (MemoryStream stream = new MemoryStream(data))
        using (BinaryReader r = new BinaryReader(stream))
        {
            type = Read(r);
        }
    }

    #region ----> IEqualityComparer <----

    public bool Equals(SerializableType p_x, SerializableType p_y)
    {
        if (ReferenceEquals(p_x, p_y)) return true;
        if (ReferenceEquals(p_x, null)) return false;
        if (ReferenceEquals(p_y, null)) return false;
        if (p_x.GetType() != p_y.GetType()) return false;
        
        return p_x.typeName == p_y.typeName;
    }

    public int GetHashCode(SerializableType p_obj)
    {
        return (p_obj.typeName != null ? p_obj.typeName.GetHashCode() : 0);
    }

    #endregion

}
