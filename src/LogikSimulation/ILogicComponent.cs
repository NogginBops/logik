using LogikCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogikSimulation
{
    public interface ILogicComponent : IComponent
    {
        public void Evaluate(Span<Value> State);
    }

    public interface IInteractableComponent : IComponent
    {
        // FIXME: Input parameters
        public void Press();
        public void Release();
    }
}
