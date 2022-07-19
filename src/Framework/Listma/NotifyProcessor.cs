using System;
using System.Collections;
using System.Text;


namespace Listma
{
    internal class NotifyProcessor<T, C>
    {
        Hashtable _to;
        Hashtable _cc;
        
        Notification _notification;
        NotifyTemplate _template;
        T _entity;
        C _ctx;
        INotifyHandler<T, C> _handler;

        internal NotifyProcessor(T entity, C context, Notification n, NotifyTemplate t, INotifyHandler<T,C> handler)
        {
            _to = new Hashtable();
            _cc = new Hashtable();
            _notification = n;
            _template = t;
            _entity = entity;
            _ctx = context;
            _handler = handler;
        }

        internal NotifyMessage Process()
        {
            foreach (Recipient r in _notification.To) AddTo(r);
            if (_to.Count == 0) return null;
            foreach (Recipient r in _notification.Cc) AddCc(r);
            NotifyMessage message = InternalParse(GetTo(), GetCc(), _template); 
            return message;
        }

        private string[] InternalResolveAddress(string role)
        {
            if (_handler != null) return _handler.ResolveAddress(role, _entity, _ctx);
            else return new string[] { };
        }

        private NotifyMessage InternalParse(string to, string cc, NotifyTemplate template)
        {
            NotifyMessage message = new NotifyMessage(to, cc, null, null);
            if (_handler != null)
                _handler.ParseMessageTemplate(message, template, _entity, _ctx);
            else
            {
                message.Subject = template.Subject;
                message.Body = template.Body;
            }
            return message;
        }

        private string GetCc()
        {
            StringBuilder s = new StringBuilder();
            foreach (object key in _cc.Keys)
            {
                if (!_to.ContainsKey(key))
                    s.AppendFormat("{0}; ", _cc[key]);
            }
            return s.ToString();
        }

        private string GetTo()
        {
            StringBuilder s = new StringBuilder();
            foreach (object key in _to.Keys)
                s.AppendFormat("{0}; ", _to[key]);
            return s.ToString();
        }



        void AddTo(Recipient rec)
        {
            AddRecipient(_to, rec);
        }

        void AddCc(Recipient rec)
        {
            AddRecipient(_cc, rec);
        }

        void AddRecipient(Hashtable source, Recipient rec)
        {

            if (!rec.Role.IsNullOrEmpty())
            {
                foreach (string addr in InternalResolveAddress(rec.Role))
                    source[addr] = addr;
            }
            else if (!rec.Address.IsNullOrEmpty())
            {
                source[rec.Address] = rec.Address;
            }
        }
    }
}
