using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializedList<TValue> : List<TValue>, ISerializationCallbackReceiver
{
    [SerializeField]  private List<TValue> values = new List<TValue>();
    
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < this.values.Count; i++)
        {
            this[i] = values[i];
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        this.values.Clear();

        foreach (var item in this)
        {
            this.values.Add(item);
        }
    }
}
