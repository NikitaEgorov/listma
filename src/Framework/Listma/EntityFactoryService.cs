using System;
using Listma.Configuration;
using Listma;


namespace Listma
{
    internal static class EntityFactoryService
    {
        internal static IWorkflowAdapter<T> GetWorkflow<T>(T entity, IWorkflowFactory<T> factory, EntityWorkflow workflow)
        {
            IWorkflowAdapter<T> res;
            if (workflow.StatechartMap.IsNullOrEmpty())
            {
                res = factory.GetWorkflow(entity, workflow.StateMap);
                res.SetStatechartId(workflow.StatechartId);
            }
            else
                res = factory.GetWorkflow(entity, workflow.StateMap, workflow.StatechartMap);
            res.SetEntityType(workflow.EntityType);
            return res;
        }

        internal static IWorkflowAdapter<T>  StartWorkflow<T, C>(T entity, IWorkflowFactory<T> factory, EntityWorkflow workflow, Statechart statechart, C context)
        {
            IWorkflowAdapter<T> res = GetWorkflow<T>(entity, factory, workflow);
            Init(res, workflow, statechart, workflow.InitialState, context);
            return res;
           
        }
        internal static IWorkflowAdapter<T> StartWorkflow<T, C>(T entity, IWorkflowFactory<T> factory, EntityWorkflow workflow, Statechart statechart, StateInfo initialState, C context)
        {
            IWorkflowAdapter<T> res = GetWorkflow<T>(entity, factory, workflow);
            Init(res, workflow, statechart, initialState.Id, context);
            return res;
        }

        private static  void Init<T, C>(IWorkflowAdapter<T> res, EntityWorkflow w, Statechart s, string initialState, C context)
        {
            res.SetStatechartId(w.StatechartId);
            if (initialState.IsNullOrEmpty() || initialState == "*")
            {
                foreach (State st in s.GetStates(true))
                {
                    initialState = st.Id;
                    break;
                }
                if (initialState == null)
                    throw new WorkflowException("Configuration error. Statechart '{0}' do not contain states marked as 'initial'.", s.Id);
            }
            State state = s.GetState(initialState);
            if (state == null)
                throw new WorkflowException("Configuration error. InitialState '{0}' of '{1}' entity workflow do not exist in '{2}' statechart.",
                    initialState, w.EntityType, s.Id);
            else
            {
                res.SetCurrentState(initialState);
                if (!state.OnEnterHandler.IsNullOrEmpty())
                {
                    IHandler<T, C> enterHandler = ReflectedTypeCache.GetInstance<IHandler<T, C>>(state.OnEnterHandler, new Type[] { typeof(T), typeof(C) });
                    enterHandler.Execute(res.Entity, context);
                }
            }
        }
    }
}
