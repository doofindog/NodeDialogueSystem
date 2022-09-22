using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : DialogueEvents
{
    public static void GetGold()
    {
        Debug.Log("You received a gold coin");
    }

    public static void IncreaseSkill(string skillName, int amount)
    {
        Debug.Log(skillName + "increased by " + amount);
    }
}
