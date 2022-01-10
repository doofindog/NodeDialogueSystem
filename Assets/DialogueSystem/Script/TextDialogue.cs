using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{

    [System.Serializable]
    public class TextDialogue : Dialogue
    {
        public string text;

        public TextDialogue()
        {
            type = TypeContrain.TEXT;
        }
    }
}
