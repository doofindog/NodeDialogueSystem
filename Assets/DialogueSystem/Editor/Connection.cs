using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class Connection
    {
        public string ID;
        public ConnectionPort startPort;
        public ConnectionPort endPort;

        private Action<Connection> _onClickCallBack;

        public Connection(ConnectionPort startPort, ConnectionPort endPort, Action<Connection> OnClickCallBack, string id)
        {
            this.ID = id;
            this.startPort = startPort;
            this.endPort = endPort;
            _onClickCallBack = OnClickCallBack;
        }

        public void Draw()
        {
            Handles.DrawBezier(startPort.rect.center, endPort.rect.center,startPort.rect.center + Vector2.up * 50f , endPort.rect.center + Vector2.down * 50f, Color.white,null, 2f);
            if (Handles.Button((startPort.rect.center + endPort.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                _onClickCallBack.Invoke(this);
            }
        }

    }
} 



