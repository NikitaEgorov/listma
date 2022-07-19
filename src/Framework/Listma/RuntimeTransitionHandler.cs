using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma
{
    internal class RuntimeTransitionHandler<EntityType, ContextType> : ITransitionHandler<EntityType, ContextType>
    {
        public RuntimeTransitionHandler(Action<EntityType> preValidateAction,
            Action<EntityType, ContextType> executeAction,
            Func<EntityType, string, bool> confirmStateChangeFunc)
        {
            PreValidateAction = preValidateAction;
            ExecuteAction = executeAction;
            ConfirmStateChangeFunc = confirmStateChangeFunc;
        }

        Action<EntityType> PreValidateAction;
        Action<EntityType, ContextType> ExecuteAction;
        Func<EntityType, string, bool> ConfirmStateChangeFunc;


        #region ITransitionHandler<EntityType,ContextType> Members

        public void PreValidate(EntityType entity)
        {
            if (PreValidateAction != null) PreValidateAction(entity);
        }

        public bool ConfirmStateChange(EntityType entity, string targetState)
        {
            if (ConfirmStateChangeFunc != null)
                return ConfirmStateChangeFunc(entity, targetState);
            else return true;
        }

        #endregion

        #region IHandler<EntityType,ContextType> Members

        public void Execute(EntityType entity, ContextType context)
        {
            if (ExecuteAction != null) ExecuteAction(entity, context);
        }

        #endregion
    }
}
