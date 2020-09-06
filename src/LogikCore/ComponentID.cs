using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace LogikCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ComponentID : IEquatable<ComponentID>, IComparable<ComponentID>
    {
        public static readonly ComponentID Invalid = (ComponentID)0;

        public int ID;

        public ComponentID(int id)
        {
            ID = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is ComponentID iD && Equals(iD);
        }

        public bool Equals(ComponentID other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public int CompareTo([AllowNull] ComponentID other)
        {
            return ID.CompareTo(other);
        }

        public override string? ToString()
        {
            return ID.ToString();
        }

        public static bool operator ==(ComponentID left, ComponentID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComponentID left, ComponentID right)
        {
            return !(left == right);
        }

        public static bool operator ==(ComponentID left, int right)
        {
            return left.ID == right;
        }

        public static bool operator !=(ComponentID left, int right)
        {
            return !(left == right);
        }

        public static bool operator ==(int left, ComponentID right)
        {
            return left == right.ID;
        }

        public static bool operator !=(int left, ComponentID right)
        {
            return !(left == right);
        }

        public static explicit operator ComponentID(int i) => new ComponentID(i);

        public static explicit operator int(ComponentID id) => id.ID;
    }
}
