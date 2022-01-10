using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public class Dialogue : ScriptableObject
    {
        public enum TypeContrain
        {
            POINT,
            TEXT,
            EVENT
        }

        public DialogueGraph graph;
        public string id;
        public TypeContrain type;
        private List<ConnectionPort> ports;


        public void AddInputPort()
        {

        }

        public void AddOutputPort()
        {

        }
    }
}

