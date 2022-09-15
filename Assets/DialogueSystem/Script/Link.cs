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
        

        public Link(Entry sourceEntry,Entry destinationEntry, Option option = null)
        {
            id = NodeManager.GenerateUniqueId();

            this.option = option;
            this.sourceEntry = sourceEntry; 
            this.destinationEntry = destinationEntry;
        } 
        

        #region IEqualityComparer
        
        public bool Equals(Link x, Link y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.id == y.id;
        }

        public int GetHashCode(Link obj)
        {
            return (obj.id != null ? obj.id.GetHashCode() : 0);
        }
        
        #endregion

    }
}


