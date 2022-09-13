using System;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class LinkEditor
    {
        private Link _link;
        private Node _sourceNode;
        private Node _destinationNode;

        private Action<LinkEditor> _onClickCallBack;

        public LinkEditor(Link link,Node sourceNode, Node destinationNode)
        {
            _link = link;
            _sourceNode = sourceNode;
            _destinationNode = destinationNode;
        }

        public void Draw()
        {
            Handles.DrawBezier
            (
                _sourceNode.GetRect().center, _destinationNode.GetRect().center,
                _sourceNode.GetRect().center + Vector2.up * 50f , _destinationNode.GetRect().center + Vector2.down * 50f, 
                Color.white,null, 2f
            );
        }
    }
} 



