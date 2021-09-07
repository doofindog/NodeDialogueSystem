using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesManager : MonoBehaviour
{

    public static string GenerateUniqueId()
    {
        Guid g = Guid.NewGuid();
        string id = Convert.ToBase64String(g.ToByteArray());
        id = id.Replace("=","");
        id = id.Replace("+", "");
        id = id.Replace("/", "");
        return id;
    }
}
