using System;

using Listma;

namespace Listma.Test
{
    public class NotifyHandler : INotifyHandler<TestEntity, TestContext>
    {
        #region INotifyHandler<TestEntity,TestContext> Members

        public string[] ResolveAddress(string role, TestEntity entity, TestContext context)
        {
            return new string[] { role + "@mail.com" };
        }

        public void ParseMessageTemplate(NotifyMessage message, NotifyTemplate template, TestEntity entity, TestContext context)
        {
            message.Subject = template.Subject.Replace("${Handler}", "handled " + context.Text);
            message.Body = template.Body.Replace("${Handler}", "handled " + context.Text );
        }

        #endregion
    }
}
