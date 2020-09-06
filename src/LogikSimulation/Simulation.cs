using System;
using System.Collections.Generic;
using System.Text;
using LogikCore;

namespace LogikSimulation
{
    public class CSharpSimulation : ISimulation
    {
        public Engine Engine;

        public CSharpSimulation(ILogicComponent[] logicImpls)
        {
            Engine = new Engine(logicImpls);
        }

        public void Init() => Engine.Init();

        public void Exit() => Engine.Exit();

        public bool AddSubnet(SubnetID subnet) => Engine.AddSubnet(subnet);

        public bool RemoveSubnet(SubnetID subnet) => Engine.RemoveSubnet(subnet);

        public ComponentID AddComponent(ComponentType componentType) => Engine.AddComponent(componentType);

        public bool RemoveComponent(ComponentID component) => Engine.RemoveComponent(component);

        public bool Link(ComponentID component, int port, SubnetID subnet) => Engine.Link(component, port, subnet);

        public bool Unlink(ComponentID component, int port, SubnetID subnet) => Engine.Unlink(component, port, subnet);

        public void Tick() => throw new NotImplementedException();

        public ValueState SubnetState(SubnetID subnet) => Engine.SubnetState(subnet);

        public ValueState PortState(ComponentID component, int port) => Engine.PortState(component, port);

        public ValueState PressComponent(ComponentID componentId) => throw new NotImplementedException();

        public ValueState ReleaseComponent(ComponentID componentId) => throw new NotImplementedException();
    }
}
