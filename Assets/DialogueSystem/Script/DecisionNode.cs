using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    
    public class DecisionNode : Node
    {
        [SerializeField] public string text;

        [SerializeField] private List<Option> options;
        [SerializeField] private List<Port> optionPorts;

        public override void Init(Vector2 position)
        {
            base.Init(position);
            
            options = new List<Option>();
            optionPorts = new List<Port>();
            
            for (int i = 0; i < 2; i++)
            {
                AddOption();
            }
        }

        public void AddOption()
        {
            Option option = new Option((options.Count + 1).ToString());
            options.Add(option);
            Port port = new Port(this, DirectionFlow.OUT);
            optionPorts.Add(port);
        }

        public void RemoveOption(Option option)
        {
            Option removeOption = null;
            Port removePort = null;
            for (int i = 0; i < options.Count; i++)
            {
                if (option.id == options[i].id)
                {
                    removeOption = options[i];
                    removePort = optionPorts[i];
                    break;
                }
            }

            options.Remove(removeOption);
            optionPorts.Remove(removePort);
        }

        public override void Invoke()
        {
            DialogueManager.instance.ShowDiloague(text,options);
        }

        public Option[] GetOptions()
        {
            return options.ToArray();
        }

        public Port GetOptionPort(Option option)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (option.id == options[i].id)
                {
                    return optionPorts[i];
                }
            }

            return null;
        }


    }
}

