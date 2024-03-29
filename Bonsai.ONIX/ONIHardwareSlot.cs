﻿using System;

namespace Bonsai.ONIX
{
    public class ONIHardwareSlot : IEquatable<ONIHardwareSlot>
    {
        public string Driver = "";

        public int Index = 0;

        internal string MakeKey()
        {
            return string.Format("({0},{1})", Driver, Index);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Driver) ? "" : string.Format("{0}/{1}", Driver, Index);
        }

        public bool Equals(ONIHardwareSlot other) => MakeKey().Equals(other.MakeKey());

        public override int GetHashCode() => MakeKey().GetHashCode();

        public override bool Equals(object obj) => obj is ONIHardwareSlot objHS && Equals(objHS);
    }
}
