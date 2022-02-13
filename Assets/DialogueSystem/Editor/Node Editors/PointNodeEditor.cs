using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.PointNode))]
    public class PointNodeEditor : NodeEditor
    {
        private PointNode pointNode;
        public PointType pointType;

        public override void Init(Node node, GraphWindow graphWindow)
        {
            base.Init(node, graphWindow);
            pointNode = (PointNode) node;
        }

        public override void Draw()
        {
            base.Draw();

            DrawTypeSelection();

            if (pointNode.GetPointType() == PointType.START)
            {
                DrawOutPorts();
            }
            else if(pointNode.GetPointType() == PointType.END)
            {
                DrawInPorts();
            }
        }

        public void DrawTypeSelection()
        {
            GUILayout.BeginArea(rect);
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(pointNode.GetPointType().ToString(),style,GUILayout.ExpandHeight(true),GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
        }
        
    }
}

