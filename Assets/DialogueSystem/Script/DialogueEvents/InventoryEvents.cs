using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents : DialogueEvents
{
    public static void GiveItem()
    {
        Debug.Log("Debug Item given");
    }

    public static void GiveItemOfInt(int number)
    {
        Debug.Log("Debug number : " + number);
    }

    public static void GiveBoolean(bool isTrue)
    {
        Debug.Log("Debug boolean : " + isTrue);
    }
    
    public static void GiveMultiple(int number, string something, bool condition)
    {
        Debug.Log("Debug int : " + number + "\n Debug string : " +something + "\n Debug Bool : " + condition);
    }
}
