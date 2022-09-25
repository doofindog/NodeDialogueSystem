using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    [System.Serializable]
    public class EventEntry : Entry
    { 
        public List<EventInfo> staticEvents;
        
        [SerializeField] private UnityEvent _unityEvent;

        public override void Init(Vector2 p_position, ConversationGraph p_graph)
        {
            base.Init(p_position, p_graph);
            
            staticEvents = new List<EventInfo>();
            AddNewEvent();
        }

        public void AddNewEvent()
        {
            EventInfo eventInfo = new EventInfo();
            staticEvents.Add(eventInfo);
        }

        public override void Invoke()
        {
            foreach (EventInfo info in staticEvents)
            {
                info.Invoke();
            }
        }
    }
}

