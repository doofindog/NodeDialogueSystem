using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/CreateDialogue")]
public class DialoguesScriptable : ScriptableObject
{
    public int id;
    [SerializeField] public List<Node> dialogues;
}
