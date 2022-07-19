using System;

using Listma;

namespace Listma
{
    sealed class StateMachine<EntityType>
    {
        IWorkflowAdapter<EntityType> _entity;
        IConfigProvider _config;
        Statechart _statechart;

        public StateMachine(IWorkflowAdapter<EntityType> entity, Statechart statechart, IConfigProvider config)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (statechart == null) throw new ArgumentNullException("statechart");
            if (config == null) throw new ArgumentNullException("config");
            _entity = entity;
            _statechart = statechart;
            _config = config;
        }

        public event SendMessageEventHandler SendMessage;  

        internal void DoStep<ContextType>(string transitionId, ContextType context)
        {

            State state = _statechart.GetState(_entity.CurrentState);
            if (state == null) throw new WorkflowException("State '{0}' do not exist in the '{1}' statechart.", _entity.CurrentState, _entity.StatechartId);
            Transition t = state.GetTransition(transitionId);
            if (t == null) throw new WorkflowException("Transition '{0}' do not exist in the state '{1}' of '{2}' statechart.", transitionId, _entity.CurrentState, _entity.StatechartId);
            string targetState = _entity.CurrentState;
            if (!AuthorizationService.Authorize(_entity, t, _config, context)) return;
            IHandler<EntityType, ContextType> handler = null;
            ITransitionHandler<EntityType, ContextType> tHandler = null;
            if (t.RuntimeHandler != null)
            {
                handler = t.RuntimeHandler as IHandler<EntityType, ContextType>;
                tHandler = t.RuntimeHandler as ITransitionHandler<EntityType, ContextType>; 
            }
            else if (t.Handler != string.Empty)
            {
                handler = ReflectedTypeCache.GetInstance<IHandler<EntityType, ContextType>>(t.Handler, new Type[]{typeof(EntityType), typeof(ContextType)});
                tHandler = handler as ITransitionHandler<EntityType, ContextType>;
            }
            //prevalidate
            if (tHandler != null) tHandler.PreValidate(_entity.Entity);
            //state exit
            OnStateExit(context, state);
            //step handler execute
            if (handler != null) handler.Execute(_entity.Entity, context);
            //define target state
            if (tHandler != null)
            {
                if (tHandler.ConfirmStateChange(_entity.Entity, t.TargetState)) targetState = t.TargetState;
            }
            else
            {
                targetState = t.TargetState;
            }
            //notification
            NotificationHandle<ContextType>(context, t);
            //change state
            if (targetState != _entity.CurrentState)
            {
                State target = _statechart.GetState(targetState);
                if (target == null) throw new WorkflowException("Target state '{0}' do not exist in the '{1}' statechart.", targetState, _entity.StatechartId);
                _entity.SetCurrentState(targetState);
                OnStateEnter(context, target);
            }
        }

        
        private void OnStateExit<ContextType>(ContextType context, State state)
        {
            IHandler<EntityType, ContextType> exitHandler = null;
            if (state.RuntimeExitHandler != null)
            {
                exitHandler = state.RuntimeExitHandler as IHandler<EntityType, ContextType>;
            }
            else if (!state.OnExitHandler.IsNullOrEmpty())
            {
                exitHandler = ReflectedTypeCache.GetInstance<IHandler<EntityType, ContextType>>(state.OnExitHandler, new Type[] { typeof(EntityType), typeof(ContextType) });
            }
            if(exitHandler != null) exitHandler.Execute(_entity.Entity, context);
        }

        private void OnStateEnter<ContextType>(ContextType context, State state)
        {
            IHandler<EntityType, ContextType> enterHandler = null;
            if (state.RuntimeEnterHandler != null)
            {
                enterHandler = state.RuntimeEnterHandler as IHandler<EntityType, ContextType>;
            }
            else if (!state.OnEnterHandler.IsNullOrEmpty())
            {
                enterHandler = ReflectedTypeCache.GetInstance<IHandler<EntityType, ContextType>>(state.OnEnterHandler, new Type[] { typeof(EntityType), typeof(ContextType) });
            }
            if (enterHandler != null) enterHandler.Execute(_entity.Entity, context);
        }

        private void NotificationHandle<ContextType>(ContextType context, Transition t)
        {
            for (int i = 0; i < t.Notifications.Length; i++)
            {
                INotifyHandler<EntityType, ContextType> nHandler = null;
                if (t.Notifications[i].RuntimeHandler != null)
                {
                    nHandler = t.Notifications[i].RuntimeHandler as INotifyHandler<EntityType, ContextType>;
                }
                else if (!t.Notifications[i].Handler.IsNullOrEmpty())
                {
                    nHandler = ReflectedTypeCache.GetInstance<INotifyHandler<EntityType, ContextType>>(t.Notifications[i].Handler, new Type[] { typeof(EntityType), typeof(ContextType) });
                }
                NotifyTemplate template = _statechart.GetNotifyTemplate(t.Notifications[i].TemplateId);
                if (template == null) throw new WorkflowException("Notify Template '{0}' has not been found.", t.Notifications[i].TemplateId); 
                NotifyProcessor<EntityType, ContextType> np = new NotifyProcessor<EntityType, ContextType>(_entity.Entity, context, t.Notifications[i], template, nHandler);
                NotifyMessage m = np.Process();
                if (m != null && SendMessage != null) SendMessage(m);
            }
        }

        
    }
}
