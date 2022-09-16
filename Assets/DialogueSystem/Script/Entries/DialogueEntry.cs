using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueEntry : Entry
    {
        [SerializeField] public string text;

        public override void Invoke()
        {
            DialogueManager.instance.ShowDiloague(text);
        }
    }
}
