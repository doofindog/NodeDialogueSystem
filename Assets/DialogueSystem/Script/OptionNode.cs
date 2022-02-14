using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    
    public class OptionNode : Node
    {
        public string text;

        private List<Option> options;
        private List<Port> optionPort;

        public virtual void Init(Vector2 position)
        {
            base.Init(position);
            
            options = new List<Option>();
            optionPort = new List<Port>();


            for (int i = 0; i < 2; i++)
            {
                optionPort[i] = new Port(this, DirectionFlow.IN);
            }
        }
    }
}

