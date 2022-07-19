using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Listma.Configuration;
using Listma;

namespace Listma
{
    /// <summary>
    /// Helper class for runtime statechart building in "fluent interface" style
    /// </summary>
    public static class StatechartBuilder
    {
        #region Bag classes
        /// <summary>
        /// Bag class for state building
        /// </summary>
        public class StateBag
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="s">state</param>
            /// <param name="c">statechart</param>
            public StateBag(State s, Statechart c)
            {
                State = s;
                ParentChart = c;
            }
            internal State State;
            internal Statechart ParentChart;
        }

        /// <summary>
        /// Bag class for transition building
        /// </summary>
        public class TransitionBag
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="t">transition</param>
            /// <param name="s">state bag</param>
            public TransitionBag(Transition t, StateBag s)
            {
                Transition = t;
                StateBag = s;
            }
            internal Transition Transition;
            internal StateBag StateBag;
        }

        /// <summary>
        /// Bag class for notification building
        /// </summary>
        public class NotificationBag
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="n">notification</param>
            /// <param name="t">transition bag</param>
            public NotificationBag(Notification n, TransitionBag t)
            {
                Notification = n;
                TransitionBag = t;
            }
            internal Notification Notification;
            internal TransitionBag TransitionBag;
        }

        /// <summary>
        /// Bag class for UI element permissions building
        /// </summary>
        public class UIElementBag
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="e">UIElement</param>
            /// <param name="s">state bag</param>
            public UIElementBag(UIElement e, StateBag s)
            {
                UIElement = e;
                StateBag = s;
            }

            internal UIElement UIElement;
            internal StateBag StateBag;
        }

        #endregion

        /// <summary>
        /// Creates statechart for given entity workflow and registers it in the configuration
        /// </summary>
        /// <param name="config">configuration provider</param>
        /// <param name="workflow">the entity workflow for which statechart is created </param>
        /// <returns>creating statechart</returns>
        public static Statechart BuildStatechart(this IConfigProvider config, EntityWorkflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException("workflow");
            Statechart s = new Statechart();
            s.Id = workflow.StatechartId;
            config.RegisterStatechart(s);
            return s;
        }

        #region State building

        /// <summary>
        /// Adds state to statechart and begins state building 
        /// </summary>
        /// <param name="s">statechart</param>
        /// <param name="id">state Id</param>
        /// <param name="title">state title</param>
        /// <param name="initial">can state be initial</param>
        /// <returns>state bag</returns>
        public static StateBag WithState(this Statechart s, string id, string title, bool? initial)
        {
            if (id.IsNullOrEmpty()) throw new ArgumentNullException("id");
            if (title.IsNullOrEmpty()) title = id;
            State state = new State();
            state.Id = id;
            state.Title = title;
            state.Initial = initial ?? false;
            s.AddState(state);
            return new StateBag(state, s);
        }

        /// <summary>
        /// Defines runtime handler for state enter event
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call contect type</typeparam>
        /// <param name="s">state bag</param>
        /// <param name="action">The action delegate for event handling</param>
        /// <returns>state bag</returns>
        public static StateBag EnterHandledBy<EntityType, ContextType>(this StateBag s, Action<EntityType, ContextType> action)
        {
            s.State.RuntimeEnterHandler = new RuntimeHandler<EntityType, ContextType>(action);
            return s;
        }

        /// <summary>
        /// Defines runtime handler for state enter event
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call contect type</typeparam>
        /// <param name="s">state bag</param>
        /// <param name="handler">The class implements IHandler interface</param>
        /// <returns>state bag</returns>
        public static StateBag EnterHandledBy<EntityType, ContextType>(this StateBag s, IHandler<EntityType, ContextType> handler)
        {
            s.State.RuntimeEnterHandler = handler;
            return s;
        }

        /// <summary>
        /// Defines runtime handler for state exit event
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="s">state bag</param>
        /// <param name="action">genegic action delegate for event handling</param>
        /// <returns>state bag</returns>
        public static StateBag ExitHandledBy<EntityType, ContextType>(this StateBag s, Action<EntityType, ContextType> action)
        {
            s.State.RuntimeExitHandler = new RuntimeHandler<EntityType, ContextType>(action);
            return s;
        }

        /// <summary>
        /// Defines runtime handler for state exit event
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="s">state bag</param>
        /// <param name="handler">The class implements IHandler interface</param>
        /// <returns>state bag</returns>
        public static StateBag ExitHandledBy<EntityType, ContextType>(this StateBag s, IHandler<EntityType, ContextType> handler)
        {
            s.State.RuntimeExitHandler = handler;
            return s;
        }

        /// <summary>
        /// Returning method for finishing state building
        /// </summary>
        /// <param name="s">state bag</param>
        /// <returns>statechart</returns>
        public static Statechart Ret(this StateBag s)
        {
            return s.ParentChart;
        }

        #endregion

        #region Transition building
        /// <summary>
        /// Adds transition to state and begins transition building
        /// </summary>
        /// <param name="s">state bag</param>
        /// <param name="id">transition Id</param>
        /// <param name="title">transition title</param>
        /// <returns>transition bag</returns>
        public static TransitionBag WithTransition(this StateBag s, string id, string title)
        {
            if (id.IsNullOrEmpty()) throw new ArgumentNullException("id");
            if (title.IsNullOrEmpty()) title = id;
            Transition t = new Transition();
            t.Id = id;
            t.Title = title;
            s.State.AddTransition(t);
            return new TransitionBag(t, s);
        }
        /// <summary>
        /// Returning method for finishing transition building
        /// </summary>
        /// <param name="t">transition bag</param>
        /// <returns>state bag</returns>
        public static StateBag Ret(this TransitionBag t)
        {
            if (t.Transition.TargetState.IsNullOrEmpty())
                throw new WorkflowException("TargetState is a required transition attribute.");
            return t.StateBag;
        }

        /// <summary>
        /// Defines transition target state
        /// </summary>
        /// <param name="t">transition bag</param>
        /// <param name="stateId">target state Id</param>
        /// <returns>transition bag</returns>
        public static TransitionBag ToState(this TransitionBag t, string stateId)
        {
            if (stateId.IsNullOrEmpty()) throw new ArgumentNullException("stateId");
            t.Transition.TargetState = stateId;
            return t;
        }

        /// <summary>
        /// Defines runtime handler for transition
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="t">transition bag</param>
        /// <param name="handler">The class implements IHandler or ITransitionHandler interface</param>
        /// <returns>transition bag</returns>
        public static TransitionBag HandledBy<EntityType, ContextType>(this TransitionBag t, IHandler<EntityType, ContextType> handler)
        {
            t.Transition.RuntimeHandler = handler;
            return t;
        }

        /// <summary>
        /// Defines runtime handler for transition
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="t">transition bag</param>
        /// <param name="action">generic action delegate for event handling</param>
        /// <returns></returns>
        public static TransitionBag HandledBy<EntityType, ContextType>(this TransitionBag t, Action<EntityType, ContextType> action)
        {
            t.Transition.RuntimeHandler = new RuntimeHandler<EntityType, ContextType>(action);
            return t;
        }

        /// <summary>
        /// Defines runtime handler for transition
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="t">transition bag</param>
        /// <param name="preValidateAction">generic action delegate for ITransitionHandler.PreValidate method</param>
        /// <param name="executeAction">generic action delegate for IHandler.Execute method</param>
        /// <param name="confirmStateChangeFunc">generic function delegate for ITransitionHandler.ConfirmStateChange method</param>
        /// <returns>transition bag</returns>
        public static TransitionBag HandledBy<EntityType, ContextType>(this TransitionBag t,
            Action<EntityType> preValidateAction,
            Action<EntityType, ContextType> executeAction,
            Func<EntityType, string, bool> confirmStateChangeFunc)
        {
            t.Transition.RuntimeHandler = new RuntimeTransitionHandler<EntityType, ContextType>
                (preValidateAction, executeAction, confirmStateChangeFunc);
            return t;
        }

        /// <summary>
        /// Defines transition's performer
        /// </summary>
        /// <param name="t">transition bag</param>
        /// <param name="role">role name</param>
        /// <returns>transition bag</returns>
        public static TransitionBag PerformedBy(this TransitionBag t, string role)
        {
            if (role.IsNullOrEmpty()) throw new ArgumentNullException("role");
            t.Transition.AddPerformer(role);
            return t; 
        }

        /// <summary>
        /// Add notification to transition and begins build notification
        /// </summary>
        /// <param name="t">transition bag</param>
        /// <param name="templateId">notification template Id</param>
        /// <returns>notofocation bag</returns>
        public static NotificationBag WithNotification(this TransitionBag t, string templateId)
        {
            if (templateId.IsNullOrEmpty()) throw new ArgumentNullException("templateId");
            Notification n = new Notification();
            n.TemplateId = templateId;
            t.Transition.AddNotification(n);
            return new NotificationBag(n, t);
        }

        /// <summary>
        /// Returning method for finishing notofocation building
        /// </summary>
        /// <param name="n">notification bag</param>
        /// <returns>transition bag</returns>
        public static TransitionBag Ret(this NotificationBag n)
        {
            return n.TransitionBag;
        }

        /// <summary>
        /// Adds "to" role recipient 
        /// </summary>
        /// <param name="n">notification bag</param>
        /// <param name="role">role name</param>
        /// <returns>notification bag</returns>
        public static NotificationBag ToRole(this NotificationBag n, string role)
        {
            if (role.IsNullOrEmpty()) throw new ArgumentNullException("role");
            n.Notification.AddTo(new Recipient(role, null));
            return n;
        }

        /// <summary>
        /// Adds "to" address recipient
        /// </summary>
        /// <param name="n">notification bag</param>
        /// <param name="address">address</param>
        /// <returns>notification bag</returns>
        public static NotificationBag ToAddress(this NotificationBag n, string address)
        {
            if (address.IsNullOrEmpty()) throw new ArgumentNullException("address");
            n.Notification.AddTo(new Recipient(null, address));
            return n;
        }

        /// <summary>
        /// Adds "cc" role recipient 
        /// </summary>
        /// <param name="n">notification bag</param>
        /// <param name="role">role name</param>
        /// <returns>notification bag</returns>
        public static NotificationBag CcRole(this NotificationBag n, string role)
        {
            if (role.IsNullOrEmpty()) throw new ArgumentNullException("role");
            n.Notification.AddCc(new Recipient(role, null));
            return n;
        }

        /// <summary>
        /// Adds "cc" address recipient
        /// </summary>
        /// <param name="n">notification bag</param>
        /// <param name="address">address</param>
        /// <returns>notification bag</returns>
        public static NotificationBag CcAddress(this NotificationBag n, string address)
        {
            if (address.IsNullOrEmpty()) throw new ArgumentNullException("address");
            n.Notification.AddCc(new Recipient(null, address));
            return n;
        }

        /// <summary>
        /// Defines runtime notification message handler 
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="n">notification bag</param>
        /// <param name="resolveAddressFunc">generic func delegate for method INotifyHandler.ResolveAddress</param>
        /// <param name="parseMessageTemplateAction">generic action delegate for method INotifyHandler.ParseMessageTemplate </param>
        /// <returns>notofication bag</returns>
        public static NotificationBag HandledBy<EntityType, ContextType>(this NotificationBag n,
            Func<string, EntityType, ContextType, string[]> resolveAddressFunc,
            Action<NotifyMessage, NotifyTemplate, EntityType, ContextType> parseMessageTemplateAction)
        {
            n.Notification.RuntimeHandler = new RuntimeNotifyHandler<EntityType, ContextType>(resolveAddressFunc, parseMessageTemplateAction);
            return n;
        }

        /// <summary>
        /// Defines runtime notification message handler 
        /// </summary>
        /// <typeparam name="EntityType">entity type</typeparam>
        /// <typeparam name="ContextType">call context type</typeparam>
        /// <param name="n">notification bag</param>
        /// <param name="handler">INotifyHandler implementation object</param>
        /// <returns>notification bag</returns>
        public static NotificationBag HandledBy<EntityType, ContextType>(this NotificationBag n, INotifyHandler<EntityType, ContextType> handler)
        {
            n.Notification.RuntimeHandler = handler;
            return n;
        }

        #endregion

        #region NotifyTemplate building
        /// <summary>
        /// Adds notify messge template to statechart
        /// </summary>
        /// <param name="s">statechart</param>
        /// <param name="templateId">template Id</param>
        /// <param name="subjectTemplate">subject template</param>
        /// <param name="bodyTemplate">body template</param>
        /// <returns>statechart</returns>
        public static Statechart WithNotifyTemplate(this Statechart s, string templateId, string subjectTemplate, string bodyTemplate)
        {
            s.AddNotifyTemplate(templateId, subjectTemplate, bodyTemplate);
            return s;
        }
        #endregion

        #region UIPermissions

        /// <summary>
        /// Defines UI element and begins build UIElement
        /// </summary>
        /// <param name="s">statechart</param>
        /// <param name="uiElement">UI element name</param>
        /// <returns>UIElement bag</returns>
        public static UIElementBag DefinePermissionsFor(this StateBag s, string uiElement)
        {
            UIElement e = new UIElement();
            e.Name = uiElement;
            s.State.AddUIElement(e);
            return new UIElementBag(e, s);
        }

        /// <summary>
        /// Returning method for finishing notofocation building
        /// </summary>
        /// <param name="e">UIElement bag</param>
        /// <returns>state bag</returns>
        public static StateBag Ret(this UIElementBag e)
        {
            return e.StateBag;
        }

        /// <summary>
        /// Defines permission for UI element 
        /// </summary>
        /// <param name="e">UIElement bag</param>
        /// <param name="role">role name</param>
        /// <param name="level">permission level</param>
        /// <returns>UIElement bag</returns>
        public static UIElementBag ForRole(this UIElementBag e, string role, UIPermissionLevel level)
        {
            e.UIElement.AddPermission(role, level);
            return e;
        }

        #endregion
    }

    
}
