using System;
using System.Collections.Generic;
using System.Text;

namespace LogikCore
{
    public interface ISimulation
    {
        public void Init();

        public void Exit();

        public bool AddSubnet(int subnetId);

        public bool RemoveSubnet(int subnetId);

        public int AddComponent(ComponentType componentType);

        public bool RemoveComponent(int componentId);

        public bool Link(int componentId, int port, int subnetId);

        public bool Unlink(int componentId, int port, int subnetId);

        public void Tick();

        public ValueState SubnetState(int subnet);

        public ValueState PortState(int component, int port);

        public ValueState PressComponent(int componentId);

        public ValueState ReleaseComponent(int componentId);
    }
}
