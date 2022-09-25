using System;
using System.Collections.Generic;


namespace DialogueSystem
{
    [System.Serializable]
    public class Link : IEqualityComparer<Link>
    {
        public string id;
        public Option option;
        public DialogueSystem.Entry sourceEntry;  
        public DialogueSystem.Entry destinationEntry;
        

        public Link(Entry p_sourceEntry,Entry p_destinationEntry, Option p_option = null)
        {
            id = NodeManager.GenerateUniqueId();

            option = p_option;
            sourceEntry = p_sourceEntry; 
            destinationEntry = p_destinationEntry;
        }

        #region ----> IEqualityComparer <----
        
        public bool Equals(Link p_x, Link p_y)
        {
            if (ReferenceEquals(p_x, p_y)) return true;
            if (ReferenceEquals(p_x, null)) return false;
            if (ReferenceEquals(p_y, null)) return false;
            if (p_x.GetType() != p_y.GetType()) return false;
            return p_x.id == p_y.id;
        }

        public int GetHashCode(Link p_obj)
        {
            return (p_obj.id != null ? p_obj.id.GetHashCode() : 0);
        }
        
        #endregion
    }
}


