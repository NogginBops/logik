using Cairo;
using LogikCore;
using LogikUI.Circuit;
using LogikUI.Interop;
using LogikUI.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using LogikSimulation;
using LogikUI;

namespace Logik.Gates
{
    class AndGate : IComponentGraphics, ILogicComponent
    {
        // Indices for the ports
        public string Name => "And Gate";
        public ComponentType Type => ComponentType.And;
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

        public bool Contains(InstanceData data, Vector2d point)
        {
            Rect rect = new Rect(data.Position * CircuitEditor.DotSpacing, Vector2i.One * CircuitEditor.DotSpacing);
            rect = rect.Rotate(data.Position * CircuitEditor.DotSpacing, data.Orientation);
            return rect.Contains(point);
        }

        public void GetPortLocations(Span<Vector2i> ports)
        {
            ports[0] = new Vector2i(-3, 1);
            ports[1] = new Vector2i(-3, -1);
            ports[2] = new Vector2i(0, 0);
        }

        public void GetPorts(Span<Vector2i> ports)
        {
            ports[0] = new Vector2i(-3, 1);
            ports[1] = new Vector2i(-3, -1);
            ports[2] = new Vector2i(0, 0);
        }
        
        // FIXME: Cleanup and possibly split draw into a 'outline' and 'fill'
        // call so we can do more efficient cairo rendering.
        public void Draw(Context cr, InstanceData data)
        {
            using var transform = IComponentGraphics.ApplyComponentTransform(cr, data);

            //foreach (var gate in instances)
            {
                // FIXME: Scale these with CircuitEditor.DotSpacing!
                cr.MoveTo(-30,-15);
                cr.RelLineTo(15, 0);
                cr.RelCurveTo(20, 0, 20, 30, 0, 30);
                cr.RelLineTo(-15, 0);
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
            throw new NotImplementedException();
        }
    }
}
