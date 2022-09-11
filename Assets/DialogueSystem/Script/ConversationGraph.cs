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
        
        public List<Entry> entries;
        public List<Link> links;

        public void Initialise()
        {
            id = NodeManager.GenerateUniqueId();
            name = "Conversation (" + id + ")";
            
            entries = new List<Entry>();
            links = new List<Link>();
            
            StartEntry startEntry = CreateEntry<StartEntry>(new Vector2(100, 100));
        }
        
        public string GetName()
        {
            return name;
        }

        #region  entry
        
        public Entry[] GetEntries()
        {
            if (entries != null)
            {
                return entries.ToArray();
            }

            return null;
        }

        public T CreateEntry<T>(Vector2 position) where T : Entry
        {
            T entry = ScriptableObject.CreateInstance<T>() as T;
            if (entry != null)
            {
                entry.hideFlags = HideFlags.HideInHierarchy;
                entry.Init(position);
                entry.CommunicationGraph = this;
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                AddEntry(entry);
      
            }
            return entry;
        }
        
        public Entry CreateEntry (Type type,Vector2 position)
        {
            Entry entry = ScriptableObject.CreateInstance(type) as Entry;
            if (entry != null)
            {
                entry.hideFlags = HideFlags.HideInHierarchy;
                entry.Init(position);
                entry.CommunicationGraph = this;
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                AddEntry(entry);
            }
            return entry;
        }
        
        public void AddEntry(Entry entry)
        {
            if (entries == null)
            {
                entries = new List<Entry>();
            }

            entries.Add(entry);
        }

        public void RemoveEntry(Entry entry)
        {
            entries.Remove(entry);
            DestroyImmediate(entry,true);
        }

        #endregion

        #region Links

        public Link CreateLink(Entry sourceEntry, Entry destinationEntry)
        {
            Link link = new Link(sourceEntry, destinationEntry);
            links.Add(link);

            return link;
        }
        
        #endregion
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
