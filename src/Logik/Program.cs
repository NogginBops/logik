using Logik.Gates;
using LogikCore;
using LogikUI;
using LogikUI.Simulation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Logik
{
    class Program
    {
        static void Main()
        {
            IComponentGraphics[] comps =
            {
                new Constant(),
                new BufferGate(),
                new NotGate(),
                new AndGate(),
                new OrGate(),
                new XorGate(),
            };

            LogikUI.LogikUI.InitUI(comps);
        }
    }
}
