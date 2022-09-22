using System.Collections.Generic;

using UnityEngine;

namespace DialogueSystem
{
    
    public class DecisionEntry : Entry
    {
        [SerializeField] public List<Option> options;

        public override void Init(Vector2 p_position, ConversationGraph p_graph)
        {
            base.Init(p_position, p_graph);
            
            options = new List<Option>();
            for (int i = 0; i < 2; i++)
            {
                AddOption();
            }
        }

        public void AddOption()
        {
            Option option = new Option((options.Count + 1).ToString());
            options.Add(option);
        }

        public void RemoveOption(Option p_option)
        {
            Option removeOption = null;
            for (int i = 0; i < options.Count; i++)
            {
                if (p_option.id == options[i].id)
                {
                    removeOption = options[i];
                    break;
                }
            }

            options.Remove(removeOption);
        }

        public Option[] GetOptions()
        {
            return options.ToArray();
        }

        public Option GetOptionAtIndex(int i)
        {
            if (i < options.Count)
            {
                return options[i];
            }

            return null;
        }
    }
}

