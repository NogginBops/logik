using Gdk;
using LogikCore;
using LogikUI.Circuit;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogikUI.Transaction
{
    public class GateTransaction : Transaction
    {
        // FIXME: We might want to store something else...?
        public bool RemoveComponent;
        public InstanceData Gate;

        // FIXME: This feels a bit like a hack...
        public ConnectionPointsTransaction? ConnectionPointWireEdits;

        public GateTransaction(bool remove, InstanceData created, ConnectionPointsTransaction? wireEdits)
        {
            RemoveComponent = remove;
            Gate = created;
            ConnectionPointWireEdits = wireEdits;
        }

        public override string ToString()
        {
            return $"Gate: ({Gate}) {(RemoveComponent ? "removed" : "added")}, {(ConnectionPointWireEdits != null ? $"Wires changed: ({ConnectionPointWireEdits})" : "No wires changed.")}";
        }
    }
}
