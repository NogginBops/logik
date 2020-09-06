using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using LogikUI.Simulation;
using LogikCore;

namespace LogikUI.Interop
{
    class ExternSimulation : ISimulation
    {
        public RustData data;

        public void Init() => data = RustLogic.Init();

        public void Exit() => RustLogic.Exit(data);

        public bool AddSubnet(SubnetID subnet) => RustLogic.AddSubnet(data, (int)subnet);

        public bool RemoveSubnet(SubnetID subnet) => RustLogic.RemoveSubnet(data, (int)subnet);

        public ComponentID AddComponent(ComponentType componentType) => (ComponentID)RustLogic.AddComponent(data, componentType);

        public bool RemoveComponent(ComponentID component) => RustLogic.RemoveComponent(data, (int)component);

        public bool Link(ComponentID component, int port, SubnetID subnet) => RustLogic.Link(data, (int)component, port, (int)subnet);

        public bool Unlink(ComponentID component, int port, SubnetID subnet) => RustLogic.Unlink(data, (int)component, port, (int)subnet);

        public void Tick() => RustLogic.Tick(data);

        public ValueState SubnetState(SubnetID subnet) => RustLogic.SubnetState(data, (int)subnet);

        public ValueState PortState(ComponentID component, int port) => RustLogic.PortState(data, (int)component, port);

        public ValueState PressComponent(ComponentID component) => RustLogic.PressComponent(data, (int)component);

        public ValueState ReleaseComponent(ComponentID component) => RustLogic.ReleaseComponent(data, (int)component);
    }

    static class RustLogic
    {
        const string Lib = "logik_simulation";

        const CallingConvention CallingConv = CallingConvention.Cdecl;

        [DllImport(Lib, EntryPoint = "init", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern RustData Init();

        [DllImport(Lib, EntryPoint = "exit", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern void Exit(RustData data);
        
        [DllImport(Lib, EntryPoint = "add_subnet", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern bool AddSubnet(RustData data, int subnetId);
        
        [DllImport(Lib, EntryPoint = "remove_subnet", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern bool RemoveSubnet(RustData data, int subnetId);
        
        [DllImport(Lib, EntryPoint = "add_component", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern int AddComponent(RustData data, ComponentType componentType);
        
        [DllImport(Lib, EntryPoint = "remove_component", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern bool RemoveComponent(RustData data, int componentId);
        
        [DllImport(Lib, EntryPoint = "link", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern bool Link(RustData data, int componentId, int port, int subnetId);
        
        [DllImport(Lib, EntryPoint = "unlink", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern bool Unlink(RustData data, int componentId, int port, int subnetId);
        
        [DllImport(Lib, EntryPoint = "tick", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern void Tick(RustData data);

        [DllImport(Lib, EntryPoint = "subnet_state", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern ValueState SubnetState(RustData data, int subnet);

        [DllImport(Lib, EntryPoint = "port_state", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern ValueState PortState(RustData data, int component, int port);

        [DllImport(Lib, EntryPoint = "press_component", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern ValueState PressComponent(RustData data, int componentId);

        [DllImport(Lib, EntryPoint = "release_component", ExactSpelling = true, CallingConvention = CallingConv)]
        public static extern ValueState ReleaseComponent(RustData data, int componentId);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RustData
    {
        public IntPtr Handle;
    }
}
