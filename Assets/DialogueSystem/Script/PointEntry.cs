using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public class PointEntry : Entry
    {
        [SerializeField] private PointType m_pointType;
        
        
        public PointType GetPointType()
        {
            return m_pointType;
        }

        public void setPointType(PointType type)
        {
            m_pointType = type;
        }
    }

    public enum PointType
    {
        START,
        END
    }
}


