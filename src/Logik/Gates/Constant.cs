using Cairo;
using LogikCore;
using LogikUI.Circuit;
using LogikUI.Util;
using System;
using System.Collections.Generic;
using System.Text;
using LogikUI;
using LogikSimulation;

namespace Logik.Gates
{
    class Constant : IComponentGraphics, ILogicComponent
    {
        public string Name => "Constant";
        public ComponentType Type => ComponentType.Constant;
        public int NumberOfPorts => 1;

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
            ports[0] = new Vector2i(0, 0);
        }

        public void Draw(Context cr, InstanceData data)
        {
            using var transform = IComponentGraphics.ApplyComponentTransform(cr, data);

            //foreach (var gate in instances)
            {
                cr.Rectangle(-30, -15, 30, 30);
                cr.ClosePath();
            }

            // FIXME: We probably shouldn't hardcode the color
            cr.SetSourceRGB(0.1, 0.1, 0.1);
            cr.LineWidth = Wires.WireWidth;
            cr.Stroke();

            //foreach (var gate in instances)
            {
                Span<Vector2i> points = stackalloc Vector2i[NumberOfPorts];
                GetPorts(points);

                for (int i = 0; i < NumberOfPorts; i++)
                {
                    IComponentGraphics.DrawRoundPort(cr, data, points, i);
                }
            }
        }

        public void Evaluate(Span<Value> State)
        {
            State[0] = Value.One;
        }
    }
}
