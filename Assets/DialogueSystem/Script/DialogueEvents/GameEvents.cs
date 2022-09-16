using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : DialogueEvents
{
    public static void GiveHealth()
    {
        Debug.Log("Thanks for healing me");
    }

    public static void GiveStamina()
    {
        Debug.Log("Much mana, Much wow");
    }

    public static void GiveBoth()
    {
        Debug.Log("I got both");
    }
}
