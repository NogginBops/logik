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

        public bool AddSubnet(SubnetID subnet)
        {
            var ret = Simulation.AddSubnet(subnet);
            Print($"AddSubnet(Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public bool RemoveSubnet(SubnetID subnet)
        {
            var ret = Simulation.RemoveSubnet(subnet);
            Print($"RemoveSubnet(Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public ComponentID AddComponent(ComponentType componentType)
        {
            var ret = Simulation.AddComponent(componentType);
            Print($"AddComponent(Type: {componentType}) -> {ret}");
            return ret;
        }

        public bool RemoveComponent(ComponentID component)
        {
            var ret = Simulation.RemoveComponent(component);
            Print($"RemoveComponent(Component: {component}) -> {ret}");
            return ret;
        }

        public bool Link(ComponentID component, int port, SubnetID subnet)
        {
            var ret = Simulation.Link(component, port, subnet);
            Print($"Link(Component: {component}, Port: {port}, Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public bool Unlink(ComponentID component, int port, SubnetID subnet)
        {
            var ret = Simulation.Unlink(component, port, subnet);
            Print($"Unlink(Component: {component}, Port: {port}, Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public void Tick()
        {
            Simulation.Tick();
            Print($"Tick()");
        }

        public ValueState SubnetState(SubnetID subnet)
        {
            var ret = Simulation.SubnetState(subnet);
            //Print($"SubnetState(Subnet: {subnet}) -> {ret}");
            return ret;
        }

        public ValueState PortState(ComponentID component, int port)
        {
            var ret = Simulation.PortState(component, port);
            //Print($"SubnetState(Component: {component}, Port: {port}) -> {ret}");
            return ret;
        }

        public ValueState PressComponent(ComponentID component)
        {
            var ret = Simulation.PressComponent(component);
            Print($"PressComponent(Component: {component}) -> {ret}");
            return ret;
        }

        public ValueState ReleaseComponent(ComponentID component)
        {
            var ret = Simulation.ReleaseComponent(component);
            Print($"ReleaseComponent(Component: {component}) -> {ret}");
            return ret;
        }
    }
}
