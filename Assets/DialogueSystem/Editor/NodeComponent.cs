using UnityEngine;

namespace DialogueSystem.Editor
{
    public class NodeComponent
    {
        protected Node node;
        protected Rect canvasRect;

        public Rect GetRect()
        {
            return canvasRect;
        }
    }
}
