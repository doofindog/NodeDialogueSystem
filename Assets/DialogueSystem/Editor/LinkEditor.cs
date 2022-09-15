using System;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class LinkEditor
    {
        private Action<LinkEditor> _onClickCallBack;
        

        public void Draw(Vector2 startPos, Vector2 endPos)
        {

            Handles.DrawBezier
            (
                startPos, endPos,
                startPos + Vector2.up * 50f , endPos + Vector2.down * 50f, 
                Color.white,null, 2f
            );
        }
    }
} 



