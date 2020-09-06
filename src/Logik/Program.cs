using Logik.Gates;
using LogikCore;
using LogikSimulation;
using LogikUI;
using LogikUI.Simulation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

            // FIXME: Don't use Select casting!
            ISimulation simulation = new CSharpSimulation(new ILogicComponent[] { new AndGate(), new Constant(), });

            LogikUI.LogikUI.InitUI(simulation, comps);
        }
    }
}
