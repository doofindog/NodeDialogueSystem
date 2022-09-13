using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public abstract class Entry : ScriptableObject, IEquatable<Entry>, IComparable<Entry>
    {
        public Vector2 position;
        
        public ConversationGraph CommunicationGraph;
        public string id;

        public virtual void Init(Vector2 position)
        {
            this.id = NodeManager.GenerateUniqueId();
            this.position = position;
        }

        public virtual void Invoke()
        {
            
        }

        public bool Equals(Entry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (id != null ? id.GetHashCode() : 0);
            }
        }

        public int CompareTo(Entry other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(id, other.id, StringComparison.Ordinal);
        }

    }
}

