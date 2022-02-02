using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class Connection
    {
        public string ID;
        public PortEditor startPortEditor;
        public PortEditor endPortEditor;

        private Action<Connection> _onClickCallBack;

        public Connection(PortEditor startPortEditor, PortEditor endPortEditor, Action<Connection> OnClickCallBack, string id)
        {
            this.ID = id;
            this.startPortEditor = startPortEditor;
            this.endPortEditor = endPortEditor;
            _onClickCallBack = OnClickCallBack;
        }

        public void Draw()
        {

        }

    }
} 



