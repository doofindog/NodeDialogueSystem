using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    [System.Serializable]
    public class EventEntry : Entry
    {
        [SerializeField] public List<EventInfo> staticEvents;
        [SerializeField] private UnityEvent unityEvent;

        public override void Init(Vector2 position, ConversationGraph graph)
        {
            base.Init(position, graph);
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

