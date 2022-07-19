using System;
using System.Reflection;


namespace Listma
{
    class ReflectionEntityWorkflow<T> : IWorkflowAdapter<T>
    {

        private MemberInfo _statechartMember;
        private MemberInfo _stateMemeber;
        private string _statechartId;
        private string _entityType;
        private T _entity;

        public ReflectionEntityWorkflow(T entity,
            string stateMemeberName)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (stateMemeberName.IsNullOrEmpty())
                throw new ArgumentNullException("stateMemeberName");
            _entity = entity;
            MemberInfo[] list = entity.GetType().GetMember(stateMemeberName);
            if (list.Length == 0) throw new WorkflowException("Wrong state mapping. Member '{0}.{0}' is not exist.", entity.GetType().FullName, stateMemeberName);
            _stateMemeber = list[0];
            ValidateMemberInfo(_stateMemeber);
            _entityType = _entity.GetType().FullName;
        }

        public ReflectionEntityWorkflow(T entity,
            string stateMemeberName,
            string statechartIdMemberName)
            : this(entity, stateMemeberName)
        {
            MemberInfo[] list = entity.GetType().GetMember(statechartIdMemberName);
            if (list.Length > 0)
            {
                _statechartMember = list[0];
                ValidateMemberInfo(_statechartMember);
            }
        }

        private void ValidateMemberInfo(MemberInfo member)
        {
            if (!(member is FieldInfo) && !(member is PropertyInfo))
                throw new WorkflowException("Wrong state mapping '{0}'. Must be field or property name.", _stateMemeber.Name);
        }


        #region IEntityWorkflow<T> Members
        public T Entity
        {
            get { return _entity; }
        }

        public string CurrentState
        {
            get
            {
                if (_stateMemeber is FieldInfo)
                {
                    return ((FieldInfo)_stateMemeber).GetValue(_entity).ToString();
                }
                else
                {
                    return ((PropertyInfo)_stateMemeber).GetValue(_entity, new object[] { }).ToString();
                }

            }
        }

        public void SetCurrentState(string state)
        {
            if (_stateMemeber is FieldInfo)
            {
                FieldInfo field = (FieldInfo)_stateMemeber;
                if (field.FieldType.IsEnum)
                    field.SetValue(_entity, Enum.Parse(field.FieldType, state, true));
                else
                    field.SetValue(_entity, Convert.ChangeType(state, field.FieldType));
            }
            else
            {
                PropertyInfo prop = (PropertyInfo)_stateMemeber;
                if (prop.PropertyType.IsEnum)
                    prop.SetValue(_entity, Enum.Parse(prop.PropertyType, state, true), new object[] { });
                else
                    prop.SetValue(_entity, Convert.ChangeType(state, prop.PropertyType), new object[] { });
            }
        }

        public void SetStatechartId(string statechartId)
        {
            if (_statechartMember != null)
            {
                if (_statechartMember is FieldInfo)
                    ((FieldInfo)_statechartMember).SetValue(Entity, statechartId);
                else
                    ((PropertyInfo)_statechartMember).SetValue(Entity, statechartId, new object[] { });
            }
            else
                _statechartId = statechartId;

        }

        public string StatechartId
        {
            get
            {
                if (_statechartMember == null) return _statechartId;
                if (_statechartMember is FieldInfo)
                {
                    return (string)((FieldInfo)_statechartMember).GetValue(Entity);
                }
                else
                {
                    return (string)((PropertyInfo)_statechartMember).GetValue(Entity, new object[] { });
                }
            }
        }

        public string EntityType
        {
            get { return _entityType; }
        }

        public void SetEntityType(string entityType)
        {
            _entityType = entityType;
        }

        #endregion

    }  
}
