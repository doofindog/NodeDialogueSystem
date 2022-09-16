using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public abstract class Entry : ScriptableObject, IEqualityComparer<Entry>
    {
        [HideInInspector] public string id; 
        [SerializeField, HideInInspector] private Vector2 position;
        private ConversationGraph _conversationGraph;

        public virtual void Init(Vector2 position, ConversationGraph graph)
        {
            this.id = NodeManager.GenerateUniqueId();
            _conversationGraph = graph;
            this.position = position;
        }

        public string GetId()
        {
            return id;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void UpdatePosition(Vector2 newPositon)
        {
            position = newPositon;
        }

        public virtual void Invoke()
        {
            
        }
        
        #region IEqualityComparer
        
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
        
        #endregion

    }
}

