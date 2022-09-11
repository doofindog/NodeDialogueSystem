using System;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class Linker
    {
        private Link _link;
        private Port _sourcePort;
        private Port _destinationPort;

        private Action<Linker> _onClickCallBack;

        public Linker(Link link,Port sourcePort, Port destinationPort)
        {
            _link = link;
            _sourcePort = sourcePort;
            _destinationPort = destinationPort;
        }

        public void Draw()
        {
            Handles.DrawBezier
            (
                _sourcePort.GetRect().center, _destinationPort.GetRect().center,
                _sourcePort.GetRect().center + Vector2.up * 50f , _destinationPort.GetRect().center + Vector2.down * 50f, 
                Color.white,null, 2f
            );
        }
    }
} 



