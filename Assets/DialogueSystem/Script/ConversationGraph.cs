using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


namespace DialogueSystem
{
    [System.Serializable]
    public class ConversationGraph : ScriptableObject, IEquatable<ConversationGraph>
    {
        [SerializeField] private string id;
        [SerializeField] private new string name;
        [SerializeField] public List<Entry> entries; //TODO : Could be changed to a HashTable

        public string GetName()
        {
            return name;
        }
        public Entry[] GetEntries()
        {
            if (entries != null)
            {
                return entries.ToArray();
            }

            return null;
        }

        public void Initialise()
        {
            id = NodeManager.GenerateUniqueId();
            name = "Conversation (" + id + ")";
            
            entries = new List<Entry>();
        }

        public T CreateEntry<T>(Vector2 position) where T : Entry
        {
            T entry = ScriptableObject.CreateInstance<T>() as T;
            if (entry != null)
            {
                entry.Init(position);
                entry.CommunicationGraph = this;
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                AddNode(entry);
      
            }
            return entry;
        }
        
        public Entry CreateEntry (Type type,Vector2 position)
        {
            Entry entry = ScriptableObject.CreateInstance(type) as Entry;
            if (entry != null)
            {
                entry.Init(position);
                entry.CommunicationGraph = this;
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                AddNode(entry);
            }
            return entry;
        }
        
        public void AddNode(Entry entry)
        {
            if (entries == null)
            {
                entries = new List<Entry>();
            }

            entries.Add(entry);
        }

        public void RemoveDialogue(Entry entry)
        {
            entries.Remove(entry);
            DestroyImmediate(entry,true);
        }

        public bool Equals(ConversationGraph other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && id == other.id && name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConversationGraph) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (id != null ? id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
