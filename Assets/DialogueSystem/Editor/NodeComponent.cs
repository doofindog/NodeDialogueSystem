using UnityEngine;

namespace DialogueSystem.Editor
{
    public class NodeComponent
    {
        protected BaseNodeEditor nodeEditor;
        protected Vector2 position;
        protected Vector2 size;
        protected Rect rect;
        
        protected virtual void GetPosition(){}
        protected virtual void GetSize(){}
        protected virtual void UpdatePosition() {}
        protected virtual void UpdateSize(int length) {}
    }
}
