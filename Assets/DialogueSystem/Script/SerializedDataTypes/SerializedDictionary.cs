using System.Collections.Generic;
using DialogueSystem;

[System.Serializable]
public class LinkDictionary : SerializableDictionary<Entry, List<Link>, LinkStorage>, IEqualityComparer<Entry>
{
    public bool Equals(Entry x, Entry y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        return x.id == y.id;
    }

    public int GetHashCode(Entry obj)
    {
        return (obj.id != null ? obj.id.GetHashCode() : 0);
    }
    
}

[System.Serializable]
public class LinkStorage : SerializableDictionary.Storage<List<Link>>
{
    
}


public class MethodDictionary : SerializableDictionary<SerializableType, List<SerializableMethodInfo>>, IEqualityComparer<SerializableType>
{
    public bool Equals(SerializableType x, SerializableType y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.type == y.type;
    }

    public int GetHashCode(SerializableType obj)
    {
        return (obj.type != null ? obj.type.GetHashCode() : 0);
    }
}

