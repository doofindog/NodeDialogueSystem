using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    
    public class DecisionEntry : Entry
    {
        [SerializeField] public string text;
        [SerializeField] private List<Option> options;

        public override void Init(Vector2 position, ConversationGraph graph)
        {
            base.Init(position, graph);
            
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

        public void RemoveOption(Option option)
        {
            Option removeOption = null;
            for (int i = 0; i < options.Count; i++)
            {
                if (option.id == options[i].id)
                {
                    removeOption = options[i];
                    break;
                }
            }

            options.Remove(removeOption);
        }

        public override void Invoke()
        {
            DialogueManager.instance.ShowDiloague(text,options);
        }

        public Option[] GetOptions()
        {
            return options.ToArray();
        }
    }
}

