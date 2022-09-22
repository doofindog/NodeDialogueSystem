using System.Collections.Generic;

using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public abstract class Entry : ScriptableObject, IEqualityComparer<Entry>
    {
        private ConversationGraph _conversationGraph;
        [SerializeField, HideInInspector] private Vector2 position;
        
        public string text;
        [HideInInspector] public string id;
        
        public virtual void Init(Vector2 p_position, ConversationGraph p_graph)
        {
            this.id = NodeManager.GenerateUniqueId();
            _conversationGraph = p_graph;
            position = p_position;
        }

        public string GetId()
        {
            return id;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void UpdatePosition(Vector2 p_newPositon)
        {
            position = p_newPositon;
        }

        public virtual string GetDialogueText()
        {
            return text;
        }

        public virtual void Invoke()
        {
            
        }
        
        #region ----> IEqualityComparer <----
        
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

