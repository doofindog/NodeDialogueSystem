using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class SerializableType : ISerializationCallbackReceiver, IEqualityComparer<SerializableType>
{
    public System.Type type;
    public string typeName;
    public byte[] data;
    public SerializableType(System.Type aType)
    {
        if (aType != null)
        {
            type = aType;
            typeName = aType.Name;
        }
    }

    public void SetType(Type p_type)
    {
        this.type = p_type;
        typeName = p_type.Name;
        OnBeforeSerialize();
    }

    public static System.Type Read(BinaryReader aReader)
    {
        var paramCount = aReader.ReadByte();
        if (paramCount == 0xFF)
            return null;
        var typeName = aReader.ReadString();
        var type = System.Type.GetType(typeName);
        if (type == null)
            throw new System.Exception("Can't find type; '" + typeName + "'");
        if (type.IsGenericTypeDefinition && paramCount > 0)
        {
            var p = new System.Type[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                p[i] = Read(aReader);
            }
            type = type.MakeGenericType(p);
        }
        return type;
    }

    public static void Write(BinaryWriter aWriter, System.Type aType)
    {
        if (aType == null)
        {
            aWriter.Write((byte)0xFF);
            return;
        }
        if (aType.IsGenericType)
        {
            var t = aType.GetGenericTypeDefinition();
            var p = aType.GetGenericArguments();
            aWriter.Write((byte)p.Length);
            aWriter.Write(t.AssemblyQualifiedName);
            for (int i = 0; i < p.Length; i++)
            {
                Write(aWriter, p[i]);
            }
            return;
        }
        aWriter.Write((byte)0);
        aWriter.Write(aType.AssemblyQualifiedName);
    }


    public void OnBeforeSerialize()
    {
        using (var stream = new MemoryStream())
        using (var w = new BinaryWriter(stream))
        {
            Write(w, type);
            data = stream.ToArray();
        }
    }

    public void OnAfterDeserialize()
    {
        using (var stream = new MemoryStream(data))
        using (var r = new BinaryReader(stream))
        {
            type = Read(r);
        }
    }

    public bool Equals(SerializableType x, SerializableType y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.typeName == y.typeName;
    }

    public int GetHashCode(SerializableType obj)
    {
        return (obj.typeName != null ? obj.typeName.GetHashCode() : 0);
    }
}
