using System;
using Listma;

namespace Listma.Test
{
    public class OrderWorkflowFactory : IWorkflowFactory<Order>
    {
        #region IWorkflowFactory Members

        public IWorkflowAdapter<Order> GetWorkflow(Order entity, string stateMap)
        {
            if (stateMap != "State") throw new ApplicationException("Wrong state map");
            return (IWorkflowAdapter<Order>) new OrderWorkflowAdapter(entity);
        }

        public IWorkflowAdapter<Order> GetWorkflow(Order entity, string stateMap, string statechartMap)
        {
            return GetWorkflow(entity, stateMap);
        }

        #endregion
    }

    public class OrderWorkflowAdapter : IWorkflowAdapter<Order>
    {
        Order entity;
        string statechartId = string.Empty;
        public OrderWorkflowAdapter(object order)
        {
            entity = (Order)order;
        }
        #region IWorkflowAdapter<Order> Members

        public string EntityType
        {
            get { return entity.GetType().FullName; }
        }

        public string CurrentState
        {
            get { return entity.State.ToString(); }
        }

        public string StatechartId
        {
            get { return statechartId; }
        }

        public void SetEntityType(string entityType)
        {
            
        }

        public void SetCurrentState(string newState)
        {
            entity.State = (OrderState)Enum.Parse(typeof(OrderState), newState);
        }

        public void SetStatechartId(string _statechartId)
        {
            statechartId = _statechartId;
        }

        public Order Entity
        {
            get { return entity; }
        }

        #endregion
    }

}
