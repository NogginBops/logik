﻿using Cairo;
using LogikCore;
using LogikUI.Circuit;
using LogikUI.Util;
using System;
using System.Numerics;

namespace Logik.Gates
{
    class XorGate : IComponent
    {
        public string Name => "Xor Gate";
        public ComponentType Type => ComponentType.Xor;
        public int NumberOfPorts => 3;

        public Rect GetBounds(InstanceData data)
        {
            var size = new Vector2d(3, 3);
            var p = data.Position - new Vector2d(3, 1.5);
            return new Rect(
                p * CircuitEditor.DotSpacing,
                size * CircuitEditor.DotSpacing
                );
        }

        public void GetPorts(Span<Vector2i> ports)
        {
            ports[0] = new Vector2i(-3, 1);
            ports[1] = new Vector2i(-3, -1);
            ports[2] = new Vector2i(0, 0);
        }

        public void Draw(Context cr, InstanceData data)
        {
            using var transform = IComponent.ApplyComponentTransform(cr, data);

            //foreach (var gate in instances)
            {
                cr.MoveTo(-27.5, -15);
                cr.RelLineTo(7.5, 0);
                cr.RelCurveTo(10, 0, 15, 7.5, 20, 15);
                cr.RelCurveTo(-5, 7.5, -10, 15, -20, 15);
                cr.RelLineTo(-7.5, 0);
                cr.RelCurveTo(0, 0, 5, -7.5, 5, -15);
                cr.RelCurveTo(0, -7.5, -5, -15, -5, -15);
                cr.ClosePath();
                cr.RelMoveTo(-5, 30);
                cr.RelCurveTo(0, 0, 5, -7.5, 5, -15);
                cr.RelCurveTo(0, -7.5, -5, -15, -5, -15);
            }
            cr.SetSourceRGB(0.1, 0.1, 0.1);
            cr.LineWidth = Wires.WireWidth;
            cr.Stroke();

            //foreach (var gate in instances)
            {
                Span<Vector2i> points = stackalloc Vector2i[NumberOfPorts];
                GetPorts(points);

                for (int i = 0; i < NumberOfPorts; i++)
                {
                    IComponent.DrawRoundPort(cr, data, points, i);
                }
            }
        }
    }
}
