using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


namespace DialogueSystem
{
    [Serializable]
    public class DialogueGraph : ScriptableObject
    {
        public string id;
        [SerializeField] public List<Dialogue> dialogues; //TODO : Could be changed to a HashTable
        [SerializeField] public List<Connection> connectors;

        private Action<DialogueGraph> OnDestoryCallBack;

        public void Initalise(Action<DialogueGraph> onDestoryCallBack)
        {
            id = DialoguesManager.GenerateUniqueId();
            dialogues = new List<Dialogue>();
            connectors = new List<Connection>();
            OnDestoryCallBack = onDestoryCallBack;
        }

        public Dialogue CreateDialogue(Type type)
        {
            Dialogue dialogue = ScriptableObject.CreateInstance(type) as Dialogue;
            dialogue.graph = this;
            AssetDatabase.AddObjectToAsset(dialogue,this);
            AssetDatabase.SaveAssets();
            AddDialogue(dialogue);
            return dialogue;
        }

        public void AddDialogue(Dialogue dialogue)
        {
            dialogues.Add(dialogue);
        }

        public void RemoveDialogue(Dialogue dialogue)
        {
            dialogues.Remove(dialogue);
        }

        public Connection CreateConnection(Dialogue startDialogue, Dialogue endDialogue, Option option = null) //Creates A dialogue Link
        {

            if (connectors != null)
            {
                if (option != null)
                {
                    Connection connection = new Connection(option, startDialogue, endDialogue);
                    connectors.Add(connection);
                    return connection;
                }
                else
                {
                    Connection connection = new Connection(startDialogue, endDialogue);
                    connectors.Add(connection);
                    return connection;
                }
            }
            else
            {
                Debug.Log("Connectors Does not exist");
                return null;
            }
        }

        public void RemoveConnection(Dialogue startDialogue, Dialogue endDialogue, Option option = null)
        {

            if (connectors != null)
            {
                foreach (Connection connector in connectors.ToList())
                {
                    if (option != null)
                    {
                        if (connector.startDialogue.id == startDialogue.id &&
                            connector.endDialogue.id == endDialogue.id &&
                            connector.option.id == option.id)
                        {
                            Debug.Log("Called Option to Remove");
                            connectors.Remove(connector);

                        }
                    }
                    else
                    {
                        if (connector.startDialogue.id == startDialogue.id &&
                            connector.endDialogue.id == endDialogue.id)
                        {
                            Debug.Log("Called No Option to Remove");
                            connectors.Remove(connector);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Connectors Does not exist");
            }
        }

        public Connection[] GetConnections(Dialogue dialogue)
        {
            List<Connection> connectorsList = new List<Connection>();
            foreach (Connection connector in connectors)
            {
                if (connector.startDialogue.id == dialogue.id)
                {
                    connectorsList.Add(connector);
                }
            }

            if (connectorsList.Count > 0)
            {
                return connectorsList.ToArray();
            }

            return null;
        }

        public void OnDestroy()
        {
            OnDestoryCallBack(this);
        }
    }
}
