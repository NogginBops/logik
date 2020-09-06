using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace LogikCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubnetID : IEquatable<SubnetID>, IComparable<SubnetID>
    {
        public static readonly SubnetID Invalid = (SubnetID)0;

        public int ID;

        public SubnetID(int id)
        {
            ID = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is SubnetID iD && Equals(iD);
        }

        public bool Equals(SubnetID other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public override string? ToString()
        {
            return ID.ToString();
        }

        public int CompareTo([AllowNull] SubnetID other)
        {
            return ID.CompareTo(other.ID);
        }

        public static bool operator ==(SubnetID left, SubnetID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubnetID left, SubnetID right)
        {
            return !(left == right);
        }

        public static bool operator ==(SubnetID left, int right)
        {
            return left.ID == right;
        }

        public static bool operator !=(SubnetID left, int right)
        {
            return !(left == right);
        }

        public static bool operator ==(int left, SubnetID right)
        {
            return left == right.ID;
        }

        public static bool operator !=(int left, SubnetID right)
        {
            return !(left == right);
        }

        public static explicit operator SubnetID(int i) => new SubnetID(i);

        public static explicit operator int(SubnetID id) => id.ID;
    }
}
