using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class ConnectionPort
    {
        public string ID;
        public Rect rect;
        private ConnectionPointType _type;
        public Node node;
        private Action<ConnectionPort> _callBack;
        private bool _isclick;

        private GUIStyle _unClickedStyle;
        private GUIStyle _clickedStyle;

        public ConnectionPort(Node node,ConnectionPointType type, Action<ConnectionPort> callBack)
        {
            ID = DialoguesManager.GenerateUniqueId();
            this.node = node;
            _type = type;
            _callBack = callBack;
        }

        public void Draw(Rect rect)
        {
            if (GUI.Button(rect, string.Empty))
            {
                _isclick = true;
                _callBack(this);
            }

            this.rect = rect;
        }
    
    }

    public enum ConnectionPointType
    {
        IN,
        OUT,
    }
}