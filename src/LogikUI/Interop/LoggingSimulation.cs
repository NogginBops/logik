using LogikUI.Simulation;
using System;
using System.Collections.Generic;
using System.Text;
using LogikCore;

namespace LogikUI.Interop
{
    class LoggingSimulation : ISimulation
    {
        public ISimulation Simulation;

        public LoggingSimulation(ISimulation sim)
        {
            Simulation = sim;
        }

        public void Print(string str)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Logic] {str}");
            Console.ForegroundColor = c;
        }

        public void Init()
        {
            Simulation.Init();
            Print($"Init()");
        }

        public void Exit()
        {
            Simulation.Exit();
            Print($"Exit()");
        }

        public bool AddSubnet(int subnetId)
        {
            var ret = Simulation.AddSubnet(subnetId);
            Print($"AddSubnet(Subnet: {subnetId}) -> {ret}");
            return ret;
        }

        public bool RemoveSubnet(int subnetId)
        {
            var ret = Simulation.RemoveSubnet(subnetId);
            Print($"RemoveSubnet(Subnet: {subnetId}) -> {ret}");
            return ret;
        }

        public int AddComponent(ComponentType componentType)
        {
            var ret = Simulation.AddComponent(componentType);
            Print($"AddComponent(Type: {componentType}) -> {ret}");
            return ret;
        }

        public bool RemoveComponent(int componentId)
        {
            var ret = Simulation.RemoveComponent(componentId);
            Print($"RemoveComponent(Component: {componentId}) -> {ret}");
            return ret;
        }

        public bool Link(int componentId, int port, int subnetId)
        {
            var ret = Simulation.Link(componentId, port, subnetId);
            Print($"Link(Component: {componentId}, Port: {port}, Subnet: {subnetId}) -> {ret}");
            return ret;
        }

        public bool Unlink(int componentId, int port, int subnetId)
        {
            var ret = Simulation.Unlink(componentId, port, subnetId);
            Print($"Unlink(Component: {componentId}, Port: {port}, Subnet: {subnetId}) -> {ret}");
            return ret;
        }

        public void Tick()
        {
            Simulation.Tick();
            Print($"Tick()");
        }

        public ValueState SubnetState(int subnet)
        {
            var ret = Simulation.SubnetState(subnet);
            //Print($"SubnetState(Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public ValueState PortState(int component, int port)
        {
            var ret = Simulation.PortState(component, port);
            //Print($"SubnetState(Component: {component}, Port: {port}) -> {ret}");
            return ret;
        }

        public ValueState PressComponent(int componentId)
        {
            var ret = Simulation.PressComponent(componentId);
            Print($"PressComponent(Component: {componentId}) -> {ret}");
            return ret;
        }

        public ValueState ReleaseComponent(int componentId)
        {
            var ret = Simulation.ReleaseComponent(componentId);
            Print($"ReleaseComponent(Component: {componentId}) -> {ret}");
            return ret;
        }
    }
}
