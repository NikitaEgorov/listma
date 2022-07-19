using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Listma
{
    internal class RuntimeNotifyHandler<EntityType, ContextType> : INotifyHandler<EntityType, ContextType>
    {
        public RuntimeNotifyHandler(Func<string, EntityType, ContextType, string[]> resolveAddressFunc,
            Action<NotifyMessage, NotifyTemplate, EntityType, ContextType> parseMessageTemplateAction)
        {
            ResolveAddressFunc = resolveAddressFunc;
            ParseMessageTemplateAction = parseMessageTemplateAction;
        }

        Func<string, EntityType, ContextType, string[]> ResolveAddressFunc;
        Action<NotifyMessage, NotifyTemplate, EntityType, ContextType> ParseMessageTemplateAction;

        #region INotifyHandler<EntityType,ContextType> Members

        public string[] ResolveAddress(string role, EntityType entity, ContextType context)
        {
            if (ResolveAddressFunc != null)
                return ResolveAddressFunc(role, entity, context);
            else
                return new string[] { };
        }

        public void ParseMessageTemplate(NotifyMessage message, NotifyTemplate template, EntityType entity, ContextType context)
        {
            if (ParseMessageTemplateAction != null)
                ParseMessageTemplateAction(message, template, entity, context);
        }

        #endregion
    }
}
