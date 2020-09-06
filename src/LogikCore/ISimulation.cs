using System;
using System.Collections.Generic;
using System.Text;

namespace LogikCore
{
    public interface ISimulation
    {
        public void Init();

        public void Exit();

        public bool AddSubnet(SubnetID subnet);

        public bool RemoveSubnet(SubnetID subnet);

        public ComponentID AddComponent(ComponentType componentType);

        public bool RemoveComponent(ComponentID component);

        public bool Link(ComponentID component, int port, SubnetID subnet);

        public bool Unlink(ComponentID component, int port, SubnetID subnet);

        public void Tick();

        public ValueState SubnetState(SubnetID subnet);

        public ValueState PortState(ComponentID component, int port);

        public ValueState PressComponent(ComponentID component);

        public ValueState ReleaseComponent(ComponentID component);
    }
}
