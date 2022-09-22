using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class ConversationGraph : ScriptableObject, IEquatable<ConversationGraph>
    {
        [SerializeField] private string id;
        [SerializeField] private new string name;

        public Entry startEntry;
        public List<Entry> entries;
        public LinkDictionary links;

        public void Initialise()
        {
            id = NodeManager.GenerateUniqueId();
            name = "Conversation (" + id + ")";
            
            entries = new List<Entry>();
            links = new LinkDictionary();
            
            startEntry = CreateEntry<StartEntry>(new Vector2(100, 100));
        }
        
        public string GetName()
        {
            return name;
        }

        #region  entry

        public Entry GetStart()
        {
            return startEntry;
        }

        public Entry GetNext(Entry entry)
        {
            if (!links.Contains(entry))
            {
                return null;
            }
            Link link = links[entry][0];
            return link.destinationEntry;
        }

        public Entry GetNext(Entry entry, Option option)
        {
            if (!links.Contains(entry)) { return null; }
            
            List<Link> entryLinks = links[entry];
            for (int i = 0; i < entryLinks.Count; i++)
            {
                if (entryLinks[i].option == null) { return null; }

                if (entryLinks[i].option.Equals(option))
                {
                    return entryLinks[i].destinationEntry;
                }
            }

            return null;
        }

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
                entry.Init(position, this);
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
                entry.Init(position, this);
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
            links.Remove(entry);
            entries.Remove(entry);
            DestroyImmediate(entry,true);
        }

        #endregion

        #region Links

        public bool ContainsLinks()
        {
            return links.Keys.Count != 0;
        }

        public Link CreateLink(Entry sourceEntry, Entry destinationEntry, Option option = null)
        {
            if (!links.ContainsKey(sourceEntry))
            {
                links.Add(sourceEntry, new List<Link>());
            }

            links.TryGetValue(sourceEntry, out List<Link> adjLinks);
            Link link = new Link(sourceEntry, destinationEntry, option);
            adjLinks?.Add(link);
            
            Debug.Log("New link Created");
            return link;
        }

        public Link[] GetAdjLinks(Entry entry)
        {
            if(links.ContainsKey(entry))
            {
                this.links.TryGetValue(entry, out List<Link> adjLinks);
                if (adjLinks != null)
                {
                    return adjLinks.ToArray();
                }
            }
            
            Debug.Log("(GetAdjLink) : No entry found");
            return null;
        }

        public void RemoveLink(Link p_link)
        {
            List<Link> entryLinks = links[p_link.sourceEntry];

            foreach (Link link in entryLinks.ToList())
            {
                if (link.Equals(p_link))
                {
                    entryLinks.Remove(link);
                }
            }
        }

        public void RemoveLinks(Entry entry)
        {
            if (links.Contains(entry))
            {
                links.Remove(entry);
            }
        }

        #endregion

        #region IEquatable
        
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
        
        #endregion

    }
}
