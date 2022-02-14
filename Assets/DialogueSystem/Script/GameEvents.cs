using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : DialogueEvents
{
    public static void GiveHealth()
    {
        Debug.Log("Thanks for healing me");
    }
}
