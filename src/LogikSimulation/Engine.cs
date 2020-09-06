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
        public ComponentID ID;
        public ComponentType Type;
        public Value[] State;
    }

    public class Engine
    {
        public long CurrentTime;

        // What is a dirty instance?
        // A dirty instance

        public readonly Dictionary<SubnetID, Subnet> Subnets = new Dictionary<SubnetID, Subnet>();

        public readonly Dictionary<ComponentType, ILogicComponent> CompImplementations = new Dictionary<ComponentType, ILogicComponent>();

        public readonly Dictionary<ComponentID, ComponentData> Components = new Dictionary<ComponentID, ComponentData>();
        public readonly PriorityQueue<ComponentID> FreeComponentIDs = new PriorityQueue<ComponentID>();

        public readonly Dictionary<(ComponentID CompID, int Port), SubnetID> SubnetConnections = new Dictionary<(ComponentID CompID, int Port), SubnetID>();

        public Engine(ILogicComponent[] logicImpls)
        {
            CompImplementations = logicImpls.ToDictionary(kvp => kvp.Type);
        }

        public void Init()
        { }

        public void Exit()
        { }

        public bool AddSubnet(SubnetID subnet)
        {
            if (Subnets.ContainsKey(subnet))
            {
                Console.WriteLine($"The subnet id: {subnet} already exists!");
                return false;
            }

            Subnet net = new Subnet(subnet);
            Subnets.Add(subnet, net);

            return true;
        }

        public bool RemoveSubnet(SubnetID subnet)
        {
            bool removed = Subnets.Remove(subnet, out Subnet net);

            if (removed == false) return false;

            // FIXME: Remove all connections to this subnet

            // Maybe do something here with 'net'

            return true;
        }

        public ComponentID AddComponent(ComponentType type)
        {
            // Either generate a new id or take a freed one
            ComponentID id = FreeComponentIDs.Count == 0 ?
                (ComponentID)(Components.Count + 1) :
                FreeComponentIDs.Dequeue();

            if (CompImplementations.TryGetValue(type, out var impl) == false)
                return ComponentID.Invalid;

            ComponentData comp;
            comp.ID = id;
            comp.Type = type;
            comp.State= new Value[impl.NumberOfPorts];
            Array.Fill(comp.State, Value.Floating);
            Components.Add(id, comp);
            return id;
        }

        public bool RemoveComponent(ComponentID component)
        {
            bool removed = Components.Remove(component, out var data);

            // Remove all connections this component has
            for (int i = 0; i < data.State.Length; i++)
            {
                SubnetConnections.Remove((component, i));
            }

            if (removed) FreeComponentIDs.Enqueue(component);

            return removed;
        }

        public bool Link(ComponentID component, int port, SubnetID subnet)
        {
            // Remove any existing connections for this port
            SubnetConnections.Remove((component, port));

            SubnetConnections.Add((component, port), subnet);

            UpdateComponent(component);

            return true;
        }

        public bool Unlink(ComponentID component, int port, SubnetID subnet)
        {
            if (SubnetConnections.TryGetValue((component, port), out var net) == false)
                return false;

            // The subnet we said we are removing is 
            SubnetConnections.Remove((component, port));

            return true;
        }

        private void UpdateComponent(ComponentID id)
        {
            var component = Components[id];

            var impl = CompImplementations[component.Type];

            impl.Evaluate(component.State);

            for (int i = 0; i < component.State.Length; i++)
            {
                if (SubnetConnections.TryGetValue((component.ID, i), out var subnetID))
                {
                    var net = Subnets[subnetID];

                    net.Value = Value.Resolve(net.Value, component.State[i]);
                }
            }
        }

        public ValueState SubnetState(SubnetID subnet)
        {
            if (Subnets.TryGetValue(subnet, out var net) == false)
                return ValueState.Floating;

            return net.Value.GetValue(0);
        }

        // FIXME: The state of a port should be a Value and not ValueState
        public ValueState PortState(ComponentID component, int port)
        {
            if (Components.TryGetValue(component, out var compData) == false)
                return ValueState.Floating;

            return compData.State[port].GetValue(0);
        }
    }
}
