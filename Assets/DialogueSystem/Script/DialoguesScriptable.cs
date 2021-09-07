using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/CreateDialogue")]
public class DialoguesScriptable : ScriptableObject
{
    public string id;
    //public List<Node> dialogues;

    DialoguesScriptable()
    {
        id = DialoguesManager.GenerateUniqueId();
    }
    
}
