using System;
using System.Collections.Generic;
using System.Text;
using LogikCore;

namespace LogikUI.Simulation
{
    class Simulation : ISimulation
    {



        public void Init() => throw new NotImplementedException();

        public void Exit() => throw new NotImplementedException();

        public bool AddSubnet(int subnetId) => throw new NotImplementedException();

        public bool RemoveSubnet(int subnetId) => throw new NotImplementedException();

        public int AddComponent(ComponentType componentType) => throw new NotImplementedException();

        public bool RemoveComponent(int componentId) => throw new NotImplementedException();

        public bool Link(int componentId, int port, int subnetId) => throw new NotImplementedException();

        public bool Unlink(int componentId, int port, int subnetId) => throw new NotImplementedException();

        public void Tick() => throw new NotImplementedException();

        public ValueState SubnetState(int subnet) => throw new NotImplementedException();

        public ValueState PortState(int component, int port) => throw new NotImplementedException();

        public ValueState PressComponent(int componentId) => throw new NotImplementedException();

        public ValueState ReleaseComponent(int componentId) => throw new NotImplementedException();
    }
}
