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
        [SerializeField] private string _id;
        [SerializeField] private new string _name;

        public Entry startEntry;
        public List<Entry> entries;
        public LinkDictionary links;

        public void Initialise()
        {
            _id = NodeManager.GenerateUniqueId();
            _name = "Conversation (" + _id + ")";
            
            entries = new List<Entry>();
            links = new LinkDictionary();
            
            startEntry = CreateEntry<StartEntry>(new Vector2(100, 100));
        }
        
        public string GetName()
        {
            return _name;
        }

        #region  ----> Entry <----

        public Entry GetStart()
        {
            return startEntry;
        }

        public Entry GetNext(Entry p_entry)
        {
            if (!links.Contains(p_entry))
            {
                return null;
            }
            Link link = links[p_entry][0];
            return link.destinationEntry;
        }

        public Entry GetNext(Entry p_entry, Option p_option)
        {
            if (!links.Contains(p_entry)) { return null; }
            
            List<Link> entryLinks = links[p_entry];
            for (int i = 0; i < entryLinks.Count; i++)
            {
                if (entryLinks[i].option == null) { return null; }

                if (entryLinks[i].option.Equals(p_option))
                {
                    return entryLinks[i].destinationEntry;
                }
            }

            return null;
        }

        public Entry[] GetEntries()
        {
            if (entries == null)
            {
                return null;
            }

            return entries.ToArray();
        }

        public T CreateEntry<T>(Vector2 p_position) where T : Entry
        {
            T entry = ScriptableObject.CreateInstance<T>() as T;
            
            if (entry != null)
            {
                entry.hideFlags = HideFlags.HideInHierarchy;
                entry.Init(p_position, this);
                
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                
                AddEntry(entry);
            }
            
            return entry;
        }
        
        public Entry CreateEntry (Type p_type,Vector2 p_position)
        {
            Entry entry = ScriptableObject.CreateInstance(p_type) as Entry;
            
            if (entry != null)
            {
                entry.hideFlags = HideFlags.HideInHierarchy;
                entry.Init(p_position, this);
                
                AssetDatabase.AddObjectToAsset(entry, this);
                AssetDatabase.SaveAssets();
                
                AddEntry(entry);
            }
            
            return entry;
        }
        
        public void AddEntry(Entry p_entry)
        {
            if (entries == null)
            {
                entries = new List<Entry>();
            }

            entries.Add(p_entry);
        }

        public void RemoveEntry(Entry p_entry)
        {
            links.Remove(p_entry);
            entries.Remove(p_entry);
            DestroyImmediate(p_entry,true);
        }

        #endregion

        #region ----> Links <----

        public bool ContainsLinks()
        {
            return links.Keys.Count != 0;
        }

        public Link CreateLink(Entry p_sourceEntry, Entry p_destinationEntry, Option p_option = null)
        {
            if (!links.ContainsKey(p_sourceEntry))
            {
                links.Add(p_sourceEntry, new List<Link>());
            }

            links.TryGetValue(p_sourceEntry, out List<Link> adjLinks);
            Link link = new Link(p_sourceEntry, p_destinationEntry, p_option);
            adjLinks?.Add(link);
            
            Debug.Log("New link Created");
            return link;
        }

        public Link[] GetAdjLinks(Entry p_entry)
        {
            if(links.ContainsKey(p_entry))
            {
                links.TryGetValue(p_entry, out List<Link> adjLinks);
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

        public void RemoveLinks(Entry p_entry)
        {
            if (links.Contains(p_entry))
            {
                links.Remove(p_entry);
            }
        }

        #endregion

        #region ----> IEquatable <----
        
        public bool Equals(ConversationGraph p_other)
        {
            if (ReferenceEquals(null, p_other)) return false;
            if (ReferenceEquals(this, p_other)) return true;
            return base.Equals(p_other) && _id == p_other._id && _name == p_other._name;
        }

        public override bool Equals(object p_obj)
        {
            if (ReferenceEquals(null, p_obj)) return false;
            if (ReferenceEquals(this, p_obj)) return true;
            if (p_obj.GetType() != this.GetType()) return false;
            return Equals((ConversationGraph) p_obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_id != null ? _id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                return hashCode;
            }
        }
        
        #endregion

    }
}
