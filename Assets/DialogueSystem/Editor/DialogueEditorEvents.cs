using System;
using DialogueSystem;
using DialogueSystem.Editor;
using UnityEngine;

public class DialogueEditorEvents : MonoBehaviour
{
    public static Action<LinkEditor> removeLink;

    public static void RemoveLink(LinkEditor p_link)
    {
        removeLink?.Invoke(p_link);
    }
}
