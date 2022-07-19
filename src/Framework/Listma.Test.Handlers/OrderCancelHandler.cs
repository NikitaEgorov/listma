using System;
using Listma;

namespace Listma.Test
{
    class OrderCancelHandler : ITransitionHandler<Order, TestContext>
    {
        #region ITransitionHandler<Order,TestContext> Members

        public void PreValidate(Order entity)
        {
            if (entity.Product == string.Empty)
                throw new ApplicationException("Order.Product is empty");
        }

        public bool ConfirmStateChange(Order entity, string targetState)
        {
            return entity.Total == 0M; 
        }

        #endregion

        #region IHandler<Order,TestContext> Members

        public void Execute(Order entity, TestContext context)
        {
            context.Text += " has been done";
        }

        #endregion
    }
}
