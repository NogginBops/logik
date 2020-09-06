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

        public bool AddSubnet(int subnetId) => Engine.AddSubnet(subnetId);

        public bool RemoveSubnet(int subnetId) => Engine.RemoveSubnet(subnetId);

        public int AddComponent(ComponentType componentType) => Engine.AddComponent(componentType);

        public bool RemoveComponent(int componentId) => Engine.RemoveComponent(componentId);

        public bool Link(int componentId, int port, int subnetId) => throw new NotImplementedException();

        public bool Unlink(int componentId, int port, int subnetId) => throw new NotImplementedException();

        public void Tick() => throw new NotImplementedException();

        public ValueState SubnetState(int subnet) => Engine.SubnetState(subnet);

        public ValueState PortState(int component, int port) => Engine.PortState(component, port);

        public ValueState PressComponent(int componentId) => throw new NotImplementedException();

        public ValueState ReleaseComponent(int componentId) => throw new NotImplementedException();
    }
}
