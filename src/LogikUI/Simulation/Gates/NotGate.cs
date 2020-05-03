﻿using Atk;
using Cairo;
using LogikUI.Circuit;
using LogikUI.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogikUI.Simulation.Gates
{
    class NotGate : IComponent
    {
        public string Name => "Not Gate";
        public ComponentType Type => ComponentType.Not;
        public int NumberOfPorts => 2;

        public void GetPorts(Span<Vector2i> ports)
        {
            ports[0] = new Vector2i(0, 0);
            ports[0] = new Vector2i(-3, 0);
        }
        
        public void Draw(Context cr, InstanceData data)
        {
            double height = CircuitEditor.DotSpacing * 1.5;
            double width = CircuitEditor.DotSpacing * 3;

            var gate = data;

            cr.LineJoin = LineJoin.Bevel;
            //foreach (var gate in instances)
            {
                double horiz = gate.Orientation == Orientation.East ?
                    -1 : gate.Orientation == Orientation.West ?
                    1 : 0;
                double vert = gate.Orientation == Orientation.South ?
                    -1 : gate.Orientation == Orientation.North ?
                    1 : 0;

                var pos = gate.Position * CircuitEditor.DotSpacing;
                var p1 = pos + horiz * new Vector2d(width, height / 2) + vert * new Vector2d(-width / 2, height);
                var p2 = pos + horiz * new Vector2d(width, -height / 2) + vert * new Vector2d(width / 2, height);
                var p3 = pos + horiz * new Vector2d(width * 0.3, 0) + vert * new Vector2d(0, height * 0.3);
                var p4 = pos + horiz * new Vector2d(width * 0.15, 0) + vert * new Vector2d(0, height * 0.15);

                cr.MoveTo(p1);
                cr.LineTo(p2);
                cr.LineTo(p3);
                cr.ClosePath();

                const float r = 5;
                cr.MoveTo(p4 + new Vector2d(r, 0));
                cr.Arc(p4.X, p4.Y, r, 0, Math.PI * 2);
                cr.ClosePath();
            }
            // FIXME: We probably shouldn't hardcode the color
            cr.SetSourceRGB(0.1, 0.1, 0.1);

            cr.LineWidth = Wires.WireWidth;
            cr.Stroke();

            //foreach (var gate in instances)
            {
                // FIXME: We kind of don't want to do this again?
                int horiz = gate.Orientation == Orientation.East ?
                    -1 : gate.Orientation == Orientation.West ?
                    1 : 0;
                int vert = gate.Orientation == Orientation.South ?
                    -1 : gate.Orientation == Orientation.North ?
                    1 : 0;

                var p1 = horiz * new Vector2i(3, 0) + vert * new Vector2i(0, 3);

                var in1 = (gate.Position + p1) * CircuitEditor.DotSpacing;
                var out1 = gate.Position * CircuitEditor.DotSpacing;

                // FIXME: Magic number radius...
                cr.Arc(in1.X, in1.Y, 2, 0, Math.PI * 2);
                cr.ClosePath();
                cr.Arc(out1.X, out1.Y, 2, 0, Math.PI * 2);
                cr.ClosePath();
            }
            cr.SetSourceRGB(0.2, 0.9, 0.2);
            cr.Fill();
        }
    }
}
