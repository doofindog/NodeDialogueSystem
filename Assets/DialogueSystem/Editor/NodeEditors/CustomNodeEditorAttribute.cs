using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNodeEditorAttribute : Attribute
{
    private Type _targetType;

    public CustomNodeEditorAttribute(Type p_type)
    {
        _targetType = p_type;
    }

    public Type GetInspectedType()
    {
        return _targetType;
    }
}
