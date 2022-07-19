using System;
using Listma;

namespace Listma.Test
{
    public class ContextHandler : IHandler<Order, TestContext>
    {
        #region IHandler Members

        public void Execute(Order entity, TestContext context)
        {
            Console.WriteLine("Context is '{0}',  Order state {1}", context.Text, entity.State);
            context.Text += " has been done";
        }

        #endregion
    }
}
