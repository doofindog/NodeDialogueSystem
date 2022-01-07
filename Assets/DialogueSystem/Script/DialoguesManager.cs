using System;

public class DialoguesManager 
{

    public static string GenerateUniqueId()
    {
        Guid g = Guid.NewGuid();
        string longID = Convert.ToBase64String(g.ToByteArray());
        longID = longID.Replace("=","");
        longID = longID.Replace("+", "");
        longID = longID.Replace("/", "");

        string id = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            id += longID[i];
        }
        
        return id;
    }
}
