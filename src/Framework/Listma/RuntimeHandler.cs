using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma
{
    internal class RuntimeHandler<EntityType, ContextType> : IHandler<EntityType, ContextType>
    {
        public RuntimeHandler(Action<EntityType, ContextType> action)
        {
            _executeAction = action;
        }
        private Action<EntityType, ContextType> _executeAction;

        #region IHandler<EntityType, ContextType> Members

        public void Execute(EntityType entity, ContextType context)
        {
            if (_executeAction != null) _executeAction(entity, context);
        }

        #endregion
    }
}
