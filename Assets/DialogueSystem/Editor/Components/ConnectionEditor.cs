using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEditor;
using UnityEngine;


namespace DialogueEditor
{
    public class ConnectionEditor
    {
        private ConnectionPoint startpoint;
        private ConnectionPoint endPoint;
    
        public ConnectionEditor(ConnectionPoint start, ConnectionPoint end)
        {
            startpoint = start;
            endPoint = end;
        }

        public void Draw()
        {
            Handles.DrawBezier(startpoint.rect.center, endPoint.rect.center,startpoint.rect.center + Vector2.up * 50f , endPoint.rect.center + Vector2.down * 50f, Color.white,null, 2f);
        }

    } 
}



