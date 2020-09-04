﻿using Cairo;
using Gtk;
using LogikCore;
using LogikUI.Circuit;
using LogikUI.Component;
using System;
using System.Collections.Generic;
using System.Text;
using Orientation = LogikCore.Orientation;

namespace LogikUI.Toolbar
{
    class ComponentTool : BasicTool
    {
        public IComponent BaseComponent;
        public bool PlacingComponent = false;
        public bool DraggingComponent = false;
        public Vector2i StartPosition;
        public Vector2i VisualPosition;
        public Orientation CompOrientation;

        // FIXME: Technically we can get the name of the component from the ComponentType
        // So we shouldn't need to have the name argument here because it should be able to be derived.
        public ComponentTool(ComponentType type, string name, CircuitEditor circuitEditor, Gtk.Toolbar toolbar) 
            : base(Util.Icon.GetComponentImage(type), name, circuitEditor, toolbar)
        {
            if (circuitEditor.Scene.Gates.Components.TryGetValue(type, out var component) == false)
            {
                throw new InvalidOperationException($"There is no ICompoennt for the component type '{type}'");
            }
            BaseComponent = component!;
        }

        public override void Select(CircuitEditor editor)
        {
            PlacingComponent = true;
            editor.DrawingArea.QueueDraw();
        }

        public override void DeSelect(CircuitEditor editor)
        {
            PlacingComponent = false;
            editor.DrawingArea.QueueDraw();
        }

        // FIXME: Keyboard and general mouse movement events!!
        public override bool KeyPressed(CircuitEditor editor, Gdk.EventKey eventKey)
        {
            bool consumed = false;

            var modifiers = eventKey.State & Accelerator.DefaultModMask;
            if (modifiers == Gdk.ModifierType.None)
            {
                // No modifiers are pressed
                if (eventKey.Key == Gdk.Key.Up)
                {
                    CompOrientation = LogikCore.Orientation.North;
                    consumed = true;
                }
                if (eventKey.Key == Gdk.Key.Down)
                {
                    CompOrientation = LogikCore.Orientation.South;
                    consumed = true;
                }
                if (eventKey.Key == Gdk.Key.Left)
                {
                    CompOrientation = LogikCore.Orientation.West;
                    consumed = true;
                }
                if (eventKey.Key == Gdk.Key.Right)
                {
                    CompOrientation = LogikCore.Orientation.East;
                    consumed = true;
                }
            }

            if (consumed) editor.DrawingArea.QueueDraw();
            return consumed;
        }

        public override void MouseMoved(CircuitEditor editor, Vector2d mousePos)
        {
            if (PlacingComponent && DraggingComponent == false)
            {
                StartPosition = editor.RoundToGrid(mousePos);
                VisualPosition = StartPosition;
            }
            
            editor.DrawingArea.QueueDraw();
        }

        public override void GestureStart(CircuitEditor editor, Vector2d dragStartPos)
        {
            StartPosition = editor.RoundToGrid(dragStartPos);
            VisualPosition = StartPosition;
            DraggingComponent = true;

            editor.DrawingArea.QueueDraw();
        }

        public override void GestureUpdate(CircuitEditor editor, Vector2d offset)
        {
            VisualPosition = StartPosition + editor.RoundDistToGrid(offset);

            editor.DrawingArea.QueueDraw();
        }

        public override void GestureEnd(CircuitEditor editor, Vector2d endOffset)
        {
            VisualPosition = StartPosition + editor.RoundDistToGrid(endOffset);
            StartPosition = VisualPosition;

            var transaction = editor.Scene.Gates.CreateAddGateTransaction(editor.Scene.Wires,
                    InstanceData.Create(BaseComponent.Type, VisualPosition, CompOrientation)
                );

            editor.Scene.PushTransaction(transaction);

            Console.WriteLine($"Created gate transaction: \n{transaction}");

            StartPosition = VisualPosition;
            DraggingComponent = false;

            editor.DrawingArea.QueueDraw();
        }

        public override void Draw(CircuitEditor editor, Context cr)
        {
            if (PlacingComponent)
            {
                BaseComponent.Draw(cr, InstanceData.Create(BaseComponent.Type, VisualPosition, CompOrientation));
            }
        }
    }
}
