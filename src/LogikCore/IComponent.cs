using Cairo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace LogikCore
{
    public interface IComponent
    {
        public string Name { get; }
        public ComponentType Type { get; }
        public int NumberOfPorts { get; }
    }
}
