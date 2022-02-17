using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    
    public class OptionNode : Node
    {
        [SerializeField] public string text;

        [SerializeField] private List<Option> options;
        [SerializeField] private List<Port> optionPort;

        public virtual void Init(Vector2 position)
        {
            base.Init(position);
            
            options = new List<Option>();
            optionPort = new List<Port>();
            
            for (int i = 0; i < 2; i++)
            {
                optionPort[i] = new Port(this, DirectionFlow.OUT);
            }
        }

        public void AddOption()
        {
            options.Add(new Option((options.Count + 1).ToString()));
            optionPort.Add(new Port(this, DirectionFlow.OUT));
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

