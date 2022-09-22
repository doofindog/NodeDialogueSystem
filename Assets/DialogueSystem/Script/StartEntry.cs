using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public class StartEntry : Entry
    {
        [SerializeField] private PointType _pointType;
        
        public PointType GetPointType()
        {
            return _pointType;
        }

        public void setPointType(PointType p_type)
        {
            _pointType = p_type;
        }
    }

    public enum PointType  // Feature not used for now.
    {
        START,
        END
    }
}


