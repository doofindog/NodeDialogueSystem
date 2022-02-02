using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Dialogue/Create Graph")]
    [Serializable]
    public class Graph : ScriptableObject
    {
        [SerializeField, HideInInspector] private bool isInitialised;
        
        public string id;
        [SerializeField] public List<Node> nodes; //TODO : Could be changed to a HashTable
        [SerializeField] public Linkers connections;

        private Action<Graph> OnDestoryCallBack;

        [SerializeField] private PointNode m_startNode;
        [SerializeField] private PointNode m_endNode;

        public Node[] GetNodes()
        {
            if (nodes != null)
            {
                return nodes.ToArray();
            }

            return null;
        }

        public void Initalise()
        {
            if (isInitialised == false)
            {
                isInitialised = true;
                id = NodeManager.GenerateUniqueId();
                nodes = new List<Node>();
                connections = new Linkers();

                m_startNode = (PointNode) CreateNode(typeof(PointNode), new Vector2(100,100));
                m_endNode = (PointNode) CreateNode(typeof(PointNode), new Vector2(200,200));
                m_startNode.setPointType(PointType.START);
                m_endNode.setPointType(PointType.END);
            }
        }

        public Node CreateNode(Type type, Vector2 position)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            if (node != null)
            {
                node.Init(position);
                node.graph = this;
                AssetDatabase.AddObjectToAsset(node, this);
                SaveManager.SaveData(node);
                AssetDatabase.SaveAssets();
                AddNode(node);
      
            }
            return node;
        }
        
        public void AddNode(Node node)
        {
            if (nodes == null)
            {
                nodes = new List<Node>();
            }

            nodes.Add(node);
        }

        public void RemoveDialogue(Node node)
        {
            nodes.Remove(node);
            DestroyImmediate(node,true);
        }
        
        public void CreateConnection(Port outPort, Port inPort)
        {
            if (connections == null)
            {
                connections = new Linkers();
            }
            
            connections.Add(outPort,inPort);
        }

        public void RemoveConnection(Port outPort)
        {
            foreach (Port port in connections.Keys.ToList())
            {
                if (port.id == outPort.id)
                {
                    connections.Remove(port);
                }
            }
            
        }

        public Port GetConnections(Port port)
        {
            if (connections.ContainsKey(port))
            {
                return connections[port];
            }

            return null;
        }

        public void OnDestroy()
        {
            OnDestoryCallBack(this);
        }

        public Node GetStarNode()
        {
            return m_startNode;
        }

        public Node GetNext(Node node)
        {
            Port nextInPort = null;
            foreach (Port port in connections.Keys)
            {
                if (port.id == node.outPort.id)
                {
                    connections.TryGetValue(port, out nextInPort);
                    break;
                }
            }
            if (nextInPort != null)
            {
                return nextInPort.GetNode();
            }

            return null;
        }
    }
}
