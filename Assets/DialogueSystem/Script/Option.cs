using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;


namespace DialogueSystem
{

    [System.Serializable]
    public class Option : IEqualityComparer<Option>, IEquatable<Option>
    {
        public string id;
        public string text;

        public Option()
        {
            id = string.Empty;
        }

        public Option(string id)
        {
            this.id = id;
        }

        #region IEquialityComparer
        public bool Equals(Option x, Option y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.id == y.id;
        }

        public int GetHashCode(Option obj)
        {
            return (obj.id != null ? obj.id.GetHashCode() : 0);
        }
        #endregion


        #region IEqualable

        public bool Equals(Option other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Option) obj);
        }

        public override int GetHashCode()
        {
            return (id != null ? id.GetHashCode() : 0);
        }
        

        #endregion

    }
}
