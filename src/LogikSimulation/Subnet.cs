using LogikCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogikSimulation
{
    public class Subnet
    {
        public readonly SubnetID ID;
        public Value Value;

        public Subnet(SubnetID id)
        {
            ID = id;
            Value = Value.Floating;
        }

        public Subnet(SubnetID id, Value value)
        {
            ID = id;
            Value = value;
        }
    }
}
