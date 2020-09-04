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

        public Rect GetBounds(InstanceData data);

        public bool Contains(InstanceData data, Vector2d point)
        {
            Rect rect = GetBounds(data);
            // FIXME CircuitEditor.DotSpacing
            rect = rect.Rotate(data.Position /* * CircuitEditor.DotSpacing*/, data.Orientation);
            return rect.Contains(point);
        }

        public void GetPorts(Span<Vector2i> ports);
        
        public void Draw(Context cr, InstanceData data);

        // FIXME: There might exist a better place for this...
        static ComponentTransform ApplyComponentTransform(Context cr, InstanceData data)
        {
            var transform = new ComponentTransform(cr);
            // FIXME CircuitEditor.DotSpacing
            cr.Translate(data.Position.X/* * CircuitEditor.DotSpacing*/, data.Position.Y/* * CircuitEditor.DotSpacing*/);
            cr.Rotate(data.Orientation.ToAngle());
            return transform;
        }

        readonly ref struct ComponentTransform
        {
            public readonly Matrix ResetMatrix;
            public readonly Context Context;

            public ComponentTransform(Context context)
            {
                ResetMatrix = context.Matrix;
                Context = context;
            }

            public void Dispose()
            {
                Context.Matrix = ResetMatrix;
            }
        }

        static void DrawRoundPort(Context cr, InstanceData data, Span<Vector2i> ports, int index)
        {
            // If this component has an ID get the port state, otherwise floating
            var state = data.ID != 0 ?
                // FIXME: ISimulation
                ((ISimulation)null!).PortState(data.ID, index) :
                ValueState.Floating;
            // FIXME: Wires.GetValueColor
            var color = default(Cairo.Color); // Wires.GetValueColor(new Value(state));

            // FIXME CircuitEditor.DotSpacing
            var port = ports[index] /* * CircuitEditor.DotSpacing*/;

            cr.Arc(port.X, port.Y, 2, 0, Math.PI * 2);
            cr.ClosePath();
            cr.SetSourceColor(color);
            cr.Fill();

            //cr.MoveTo(port);
            //cr.ShowText(index.ToString());
        }

        // FIXME: Tripple-check that this does the correct transformation
        // This is important because this information is never displayed
        // so we will never visually notice it's wrong...
        static void TransformPorts(InstanceData data, Span<Vector2i> ports)
        {
            for (int i = 0; i < ports.Length; i++)
            {
                ports[i] = data.Orientation switch
                {
                    Orientation.East =>  data.Position + ports[i],
                    Orientation.South => data.Position + new Vector2i(-ports[i].Y, ports[i].X),
                    Orientation.West =>  data.Position + new Vector2i(-ports[i].X, -ports[i].Y),
                    Orientation.North => data.Position + new Vector2i(ports[i].Y, ports[i].X),
                    _ => throw new InvalidEnumArgumentException(nameof(data.Orientation), (int)data.Orientation, typeof(Orientation)),
                };
            }
        }
    }
}
