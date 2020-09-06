using LogikCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace LogikSimulation
{
    public struct ComponentData
    {
        public int ID;
        public ComponentType Type;
        public Value[] State;
    }

    public class Engine
    {
        public long CurrentTime;

        // What is a dirty instance?
        // A dirty instance

        public readonly Dictionary<int, Subnet> Subnets = new Dictionary<int, Subnet>();

        public readonly Dictionary<ComponentType, ILogicComponent> CompImplementations = new Dictionary<ComponentType, ILogicComponent>();

        public readonly Dictionary<int, ComponentData> Components = new Dictionary<int, ComponentData>();

        public readonly PriorityQueue<int> FreeComponentIDs = new PriorityQueue<int>();

        public Engine(ILogicComponent[] logicImpls)
        {
            CompImplementations = logicImpls.ToDictionary(kvp => kvp.Type);
        }

        public void Init()
        { }

        public void Exit()
        { }

        public bool AddSubnet(int id)
        {
            if (Subnets.ContainsKey(id))
            {
                Console.WriteLine($"The subnet id: {id} already exists!");
                return false;
            }

            Subnet net;
            net.ID = id;
            net.Value = Value.Floating;
            Subnets.Add(id, net);

            return true;
        }

        public bool RemoveSubnet(int id)
        {
            bool removed = Subnets.Remove(id, out Subnet net);

            if (removed == false) return false;

            // Maybe do something here with 'net'

            return true;
        }

        public int AddComponent(ComponentType type)
        {
            // Either generate a new id or take a freed one
            int id = FreeComponentIDs.Count == 0 ?
                Components.Count + 1 :
                FreeComponentIDs.Dequeue();

            if (CompImplementations.TryGetValue(type, out var impl) == false)
                return 0;

            ComponentData comp;
            comp.ID = id;
            comp.Type = type;
            comp.State= new Value[impl.NumberOfPorts];
            Array.Fill(comp.State, Value.Floating);
            Components.Add(id, comp);
            return id;
        }

        public bool RemoveComponent(int componentId)
        {
            bool removed = Components.Remove(componentId, out var data);
            
            if (removed) FreeComponentIDs.Enqueue(componentId);

            return removed;
        }

        public ValueState SubnetState(int subnet)
        {
            if (Subnets.TryGetValue(subnet, out var net) == false)
                return ValueState.Floating;

            return net.Value.GetValue(0);
        }

        // FIXME: The state of a port should be a Value and not ValueState
        public ValueState PortState(int component, int port)
        {
            if (Components.TryGetValue(component, out var compData) == false)
                return ValueState.Floating;

            return compData.State[port].GetValue(0);
        }
    }
}
