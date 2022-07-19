using System;
using Listma;

namespace Listma.Test
{
    class StateExitHandler : IHandler<Order, TestContext>
    {
        #region IHandler<Order,TestContext> Members

        public void Execute(Order entity, TestContext context)
        {
            entity.History.Add(entity.State.ToString() + " exit"); 
        }

        #endregion
    }

    class StateEnterHandler : IHandler<Order, TestContext>
    {
        #region IHandler<Order,TestContext> Members

        public void Execute(Order entity, TestContext context)
        {
            entity.History.Add(entity.State.ToString() + " enter");
        }

        #endregion
    }
}
