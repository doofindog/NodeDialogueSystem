using System.Collections.Generic;
using DialogueSystem;

[System.Serializable]
public class LinkDictionary : SerializableDictionary<Entry, List<Link>, LinkStorage>, IEqualityComparer<Entry>
{
    public bool Equals(Entry p_x, Entry p_y)
    {
        if (ReferenceEquals(p_x, p_y)) return true;
        if (ReferenceEquals(p_x, null)) return false;
        if (ReferenceEquals(p_y, null)) return false;
        
        return p_x.id == p_y.id;
    }

    public int GetHashCode(Entry p_obj)
    {
        return (p_obj.id != null ? p_obj.id.GetHashCode() : 0);
    }
    
}

[System.Serializable]
public class LinkStorage : SerializableDictionary.Storage<List<Link>>
{
    
}


public class MethodDictionary : SerializableDictionary<SerializableType, List<SerializableMethodInfo>>, IEqualityComparer<SerializableType>
{
    public bool Equals(SerializableType p_x, SerializableType p_y)
    {
        if (ReferenceEquals(p_x, p_y)) return true;
        if (ReferenceEquals(p_x, null)) return false;
        if (ReferenceEquals(p_y, null)) return false;
        if (p_x.GetType() != p_y.GetType()) return false;
        
        return p_x.type == p_y.type;
    }

    public int GetHashCode(SerializableType p_obj)
    {
        return (p_obj.type != null ? p_obj.type.GetHashCode() : 0);
    }
}

