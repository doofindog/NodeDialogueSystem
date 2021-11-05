using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


public class DialoguesScriptable : ScriptableObject
{
    public string id;
    public List<Dialogue> dialogues; //TODO : Could be changed to a HashTable
    private Dictionary<Dialogue, List<Connector>> m_connectors;

    private Action<DialoguesScriptable> OnDestoryCallBack;
    

    public void Initalise(Action<DialoguesScriptable> onDestoryCallBack)
    {
        id = DialoguesManager.GenerateUniqueId();
        dialogues = new List<Dialogue>();
        m_connectors = new Dictionary<Dialogue, List<Connector>>();
        OnDestoryCallBack = onDestoryCallBack;
    }

    public Dialogue CreateDialogue() //Creates a Dialogue
    {
        Dialogue dialogue = new Dialogue();
        dialogues.Add(dialogue);
        List<Connector> connectorsList = new List<Connector>();
        m_connectors.Add(dialogue, connectorsList);
        return dialogue;
    }

    public void RemoveDialogue(Dialogue dialogue)
    {
        dialogues.Remove(dialogue);
    }

    public void CreateConnection(Dialogue startDialogue, Dialogue endDialogue, Option startOption) //Creates A dialogue Link
    {
        List<Connector> connectorsList;
        m_connectors.TryGetValue(startDialogue, out connectorsList);
        if (connectorsList != null)
        {
            Connector connector = new Connector(startOption, endDialogue);
            connectorsList.Add(connector);
        }
        else
        {
            Debug.LogError("Connection List was null");
        }
    }

    public Connector[] GetConnections(Dialogue dialogue)
    {
        List<Connector> connectorsList;
        m_connectors.TryGetValue(dialogue, out connectorsList);
        if (connectorsList != null)
        {
            return connectorsList.ToArray();
        }
        return null;
    }

    public void OnDestroy()
    {
        Debug.Log("Called");
        OnDestoryCallBack(this);
    }
}
