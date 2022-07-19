using System;
using Listma.Configuration;
using Listma;
using System.Collections.Generic;


namespace Listma
{
    /// <summary>
    /// Provides facade for Listma functional
    /// </summary>
    public sealed class ListmaManager
    {
        IConfigProvider _configProvider;

        #region constructors

        /// <summary>
        /// Default constructor. Creates instance inicializing with default instance of ConfigProvider.   
        /// </summary>
        /// <remarks>
        /// This constructor finds configuration file "listma.config" in the current AppDomain.BaseDirectory
        /// </remarks>
        public ListmaManager()
        {
            _configProvider = ConfigProvider.GetDefault();
        }

        /// <summary>
        /// Creates instance initializing with specifing configuration file  
        /// </summary>
        /// <param name="configFileName">configuration file name</param>
        public ListmaManager(string configFileName)
        {
            if (configFileName.IsNullOrEmpty())
                _configProvider = ConfigProvider.GetDefault();
            else
                _configProvider = new ConfigProvider(ListmaConfigurationData.Load(configFileName));
        }

        /// <summary>
        /// Creates instance initializing with specifing IConfigProvider instance
        /// </summary>
        /// <param name="configProvider"></param>
        public ListmaManager(IConfigProvider configProvider)
        {
            if (configProvider == null)
                throw new ArgumentNullException("configProvider");
            _configProvider = configProvider;
        }

        #endregion

        #region events
        /// <summary>
        /// Occurs when a transition notification message needs for send
        /// </summary>
        public event SendMessageEventHandler SendMessage;

        private void OnSendMessage(NotifyMessage message)
        {
            if (SendMessage != null) SendMessage(message);
        }
        #endregion

        #region GetAvailableTransitions

        /// <summary>
        /// Returns array of transitions which available for current entity's state and current user
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity workflow adapter</param>
        /// <param name="context">call context object</param>
        /// <returns>array of TransitionInfo objects</returns>
        public TransitionInfo[] GetAvailableTransitions<EntityType, ContextType>(IWorkflowAdapter<EntityType> entity, ContextType context)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Statechart chart = _configProvider.GetStatechart(entity.StatechartId);
            State state = chart.GetState(entity.CurrentState);
            List<TransitionInfo> list = new List<TransitionInfo>();
            for (int i = 0; i < state.Transitions.Length; i++)
                if (AuthorizationService.Authorize(entity, state.Transitions[i], _configProvider, context))
                    list.Add(new TransitionInfo(state.Transitions[i]));
                
            return list.ToArray();
        }

        /// <summary>
        /// Returns array of transitions which available for current entity's state and current user
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">entity workflow adapter</param>
        /// <returns>array of TransitionInfo objects</returns>
        public TransitionInfo[] GetAvailableTransitions<EntityType>(IWorkflowAdapter<EntityType> entity)
        {
            return GetAvailableTransitions<EntityType, object>(entity, null);
        }
        #endregion

        #region GetWorkflowStates
        /// <summary>
        /// Returns states of given entity workflow
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">business entity</param>
        /// <param name="initialOnly">if true, specifies only initial states for return</param>
        /// <returns>array of states are specified wor this entity type workflow</returns>
        public StateInfo[] GetWorkflowStates<EntityType>(EntityType entity, bool initialOnly) 
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            return GetWorkflowStates(entity.GetType().FullName, initialOnly);
        }
        /// <summary>
        /// Returns states of given entity workflow
        /// </summary>
        /// <param name="entityType">entity type string</param>
        /// <param name="initialOnly">if true, specifies only initial states for return</param>
        /// <returns>array of states are specified wor this entity type workflow</returns>
        public StateInfo[] GetWorkflowStates(string entityType, bool initialOnly)
        {
            EntityWorkflow workflow = _configProvider.GetEntityWorkflow(entityType);
            Statechart statechart = _configProvider.GetStatechart(workflow.StatechartId);
            return EnumerateStates(statechart, initialOnly);
        }
        
        /// <summary>
        /// Returns states of given entity workflow
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entityWorkflow">entity workflow adapter</param>
        /// <param name="initialOnly">if true, specifies only initial states for return</param>
        /// <returns>array of states are specified wor this entity type workflow</returns>
        public StateInfo[] GetWorkflowStates<EntityType>(IWorkflowAdapter<EntityType> entityWorkflow, bool initialOnly)
        {
            if (entityWorkflow == null)
                throw new ArgumentNullException("entityWorkflow");
            Statechart statechart = _configProvider.GetStatechart(entityWorkflow.StatechartId);
            return EnumerateStates(statechart, initialOnly);
        }
        #endregion

        #region StartWorkflow
        /// <summary>
        /// Starts workflow for given entity and initializes entity state according with workflow configuration
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity instance</param>
        /// <param name="context">call context object</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> StartWorkflow<EntityType, ContextType>(EntityType entity, ContextType context)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            return StartWorkflow<EntityType, ContextType>(entity, context, entity.GetType().FullName);
        }

        /// <summary>
        /// Starts workflow for given entity and initializes entity state according with workflow configuration
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity instance</param>
        /// <param name="context">call context object</param>
        /// <param name="entityType">entity type string</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> StartWorkflow<EntityType, ContextType>(EntityType entity, ContextType context, string entityType)
        {
            if(entity == null)
                throw new ArgumentNullException("entity");
            EntityWorkflow w = _configProvider.GetEntityWorkflow(entity.GetType().FullName);
            Statechart s = _configProvider.GetStatechart(w.StatechartId);
            IWorkflowAdapter<EntityType> res = EntityFactoryService.StartWorkflow(entity,
                _configProvider.GetWorkflowFactory<EntityType>(entityType), w, s, context);
            return res;
                
        }
        /// <summary>
        /// Starts workflow for given entity and initializes entity state according with workflow configuration
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity instance</param>
        /// <param name="context">call context object</param>
        /// <param name="initialState">initial state Id</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> StartWorkflow<EntityType, ContextType>(EntityType entity, ContextType context, StateInfo initialState)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            EntityWorkflow w = _configProvider.GetEntityWorkflow(entity.GetType().FullName);
            Statechart s = _configProvider.GetStatechart(w.StatechartId);
            IWorkflowAdapter<EntityType> res = EntityFactoryService.StartWorkflow(entity,
                _configProvider.GetWorkflowFactory<EntityType>(entity.GetType().FullName),
                w, s, initialState, context);
            return res;
        }

        /// <summary>
        /// Starts workflow for given entity and initializes entity state according with workflow configuration
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity instance</param>
        /// <param name="context">call context object</param>
        /// <param name="entityType">entity type string</param>
        /// <param name="initialState">initial state Id</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> StartWorkflow<EntityType, ContextType>(EntityType entity, ContextType context, string entityType, StateInfo initialState)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            EntityWorkflow w = _configProvider.GetEntityWorkflow(entityType);
            Statechart s = _configProvider.GetStatechart(w.StatechartId);
            IWorkflowAdapter<EntityType> res = EntityFactoryService.StartWorkflow(entity,
                _configProvider.GetWorkflowFactory<EntityType>(entityType),
                w, s, initialState, context);
            return res;
        }
        #endregion

        #region GetWorkflow
        /// <summary>
        /// Returns workflow adapter for given entity
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">entity</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> GetWorkflow<EntityType>(EntityType entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            return GetWorkflow(entity, entity.GetType().FullName); 
        }

        /// <summary>
        /// Returns workflow adapter for given entity
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="entityType">entity type string</param>
        /// <returns>entity workflow adapter instance</returns>
        public IWorkflowAdapter<EntityType> GetWorkflow<EntityType>(EntityType entity, string entityType)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if(entityType.IsNullOrEmpty())
                throw new ArgumentNullException("entityType");
            IWorkflowFactory<EntityType> factory = _configProvider.GetWorkflowFactory<EntityType>(entityType);
            EntityWorkflow workflow = _configProvider.GetEntityWorkflow(entityType);
            IWorkflowAdapter<EntityType> result = EntityFactoryService.GetWorkflow(entity, factory, workflow);
            return result;
        }
        #endregion

        #region DoStep
        /// <summary>
        /// Does workflow step and moves entity into next state 
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="transitionId">statechart transition Id</param>
        /// <param name="context">context object</param>
        public void DoStep<EntityType, ContextType>(IWorkflowAdapter<EntityType> entity, string transitionId, ContextType context)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (transitionId.IsNullOrEmpty())
                throw new ArgumentNullException("transitionId");
            Statechart statechart = _configProvider.GetStatechart(entity.StatechartId);
            StateMachine<EntityType> machine = new StateMachine<EntityType>(entity, statechart, _configProvider);
            machine.SendMessage += new SendMessageEventHandler(OnSendMessage);
            machine.DoStep(transitionId, context);
        }

        /// <summary>
        /// Does workflow step and moves entity into next state 
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="transitionId">statechart transition Id</param>
        public void DoStep<EntityType>(IWorkflowAdapter<EntityType> entity, string transitionId)
        {
            DoStep<EntityType, object>(entity, transitionId, null);
        }
        #endregion

        #region GetPermissionProvider
        /// <summary>
        /// Returns IPermissionProvider for manage UI permissions 
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <param name="entity">entity</param>
        /// <returns>IPermissionProvider instance</returns>
        public IPermissionProvider GetPermissionProvider<EntityType>(IWorkflowAdapter<EntityType> entity) 
        {
            return GetPermissionProvider<EntityType, object>(entity, (object)null);
        }

        /// <summary>
        /// Returns IPermissionProvider for manage UI permissions 
        /// </summary>
        /// <typeparam name="EntityType">type of statefull entity</typeparam>
        /// <typeparam name="ContextType">type of call context</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="context">context object</param>
        /// <returns>IPermissionProvider instance</returns>
        public IPermissionProvider GetPermissionProvider<EntityType, ContextType>(IWorkflowAdapter<EntityType> entity, ContextType context)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            IRoleProvider<EntityType, ContextType> roleProvider = _configProvider.GetRoleProvider<EntityType, ContextType>(entity.EntityType);
            Statechart statechart = _configProvider.GetStatechart(entity.StatechartId);
            return new PermissionProvider<EntityType, ContextType>(entity, roleProvider, statechart, _configProvider.DefaultPermissionLevel, context);
        }
        #endregion

        #region private

        private static StateInfo[] EnumerateStates(Statechart statechart, bool initialOnly)
        {
            List<StateInfo> list = new List<StateInfo>();
            foreach (State s in statechart.GetStates(initialOnly))
                list.Add(new StateInfo(s));
            return list.ToArray();
        }

        #endregion
    }
}
