using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNodeEditorAttribute : Attribute
{
    private Type m_targetType;

    public CustomNodeEditorAttribute(Type type)
    {
        m_targetType = type;
    }

    public Type GetInspectedType()
    {
        return m_targetType;
    }
}
