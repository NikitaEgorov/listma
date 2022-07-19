using System;
using NUnit.Framework;
using Listma;
using Listma.Configuration;
using Listma.Test;
using System.Threading;
using System.IO;
using System.Security.Principal;
using System.Collections.Generic;

namespace Listma.UnitTests
{
    [TestFixture]
    public class IntegrationTests
    {
                
        [Test]
        public void SimpleWorkflowScenario()
        {
            Order order = Order.GetOrder();
            TestContext ctx = new TestContext() { Text = "Step" };
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config"));

            IWorkflowAdapter<Order> w = lm.GetWorkflow(order);
            TransitionInfo[] transitions = lm.GetAvailableTransitions(w);
            Assert.AreEqual(2, transitions.Length);
            lm.DoStep(w, transitions[0].Id, ctx);
            Assert.AreEqual("Processing", w.CurrentState);
            Assert.AreEqual("Step has been done", ctx.Text, "Result of transition handler work");
            Assert.AreEqual(2, order.History.Count, "Results of OnEnterHandler and OnExitHandler work");
            Assert.AreEqual("Draft exit", order.History[0]);
            Assert.AreEqual("Processing enter", order.History[1]);
        }

        [Test]
        public void StateAndTransitionHandlerTest()
        {
            Order order = Order.GetOrder();
            TestContext ctx = new TestContext() { Text = "Step" };
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config"));

            IWorkflowAdapter<Order> w = lm.StartWorkflow(order, ctx);//, new StateInfo("Draft"));
            Assert.AreEqual(1, order.History.Count, "Results of OnEnterHandler on workflow start");
            Assert.AreEqual("Draft enter", order.History[0]);

            order.Product = string.Empty;// in this case expect that handler throws exception 
            try
            {
                lm.DoStep(w, "Cancel", ctx);
            }
            catch(ApplicationException ex)
            {
                Assert.AreEqual("Order.Product is empty", ex.Message);
            }
            Assert.AreEqual("Step", ctx.Text, "Transition handler run");
            Assert.AreEqual(OrderState.Draft, order.State);
            Assert.AreEqual(1, order.History.Count);

            order.Product = "CD-RW";
            order.Count = 10M; // in this case expect that handler don't change order state
            lm.DoStep(w, "Cancel", ctx);
            Assert.AreEqual(OrderState.Draft, order.State);
            Assert.AreEqual("Step has been done", ctx.Text, "Result of transition handler work");
            Assert.AreEqual(2, order.History.Count);
            
            order.Count = 0M; // in this case expect that all handlers run
            ctx.Text = "Step";
            lm.DoStep(w, "Cancel", ctx);
            Assert.AreEqual(OrderState.Canceled, order.State);
            Assert.AreEqual("Step has been done", ctx.Text, "Result of transition handler work");
            Assert.AreEqual(4, order.History.Count);
            Assert.AreEqual("Draft exit", order.History[2]);
            Assert.AreEqual("Canceled enter", order.History[3]);

        }

        [Test]
        public void TwoWorkflowForOneEntity()
        {
            Order order = Order.GetOrder();
            TestContext ctx = new TestContext() { Text = "Step" };
            ListmaManager wm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config"));
            IWorkflowAdapter<Order> w1 = wm.GetWorkflow<Order>(order);
            IWorkflowAdapter<Order> w2 = wm.GetWorkflow<Order>(order, "OrderApproval");
            Assert.AreEqual("Draft", w1.CurrentState);
            Assert.AreEqual("", w2.CurrentState);
            order.ApproveState = "WaitingApprove";
            order.Count = 0M; // cancel available if Order.Total == 0 only
            wm.DoStep(w1, "Cancel", ctx);
            if (order.State == OrderState.Canceled)
                wm.DoStep(w2, "Reject", ctx);
            else
                wm.DoStep(w2, "Approve", ctx);
            Assert.AreEqual("Rejected", w2.CurrentState);
        }

        [Test]
        public void StartWorkflowAndEntityInitialization()
        {
            TestEntity entity = new TestEntity();
            Assert.AreEqual("", entity.Statechart);
            Assert.AreEqual("", entity.State);

            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config"));
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null);
            Assert.AreEqual("Initial2", entity.State);
            Assert.AreEqual("StartWorkflowTest", entity.Statechart);
            workflow = lm.StartWorkflow(entity, (object)null, new StateInfo("Initial1"));
            Assert.AreEqual("Initial1", entity.State);
            Assert.AreEqual("StartWorkflowTest", entity.Statechart);
        }

        [Test]
        public void GetAvailableStatesAndTransitions()
        {
            TestEntity entity = new TestEntity();
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config"));
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null, new StateInfo("Initial1"));
            StateInfo[] states = lm.GetWorkflowStates(entity, true);
            Assert.AreEqual(2, states.Length);
            states = lm.GetWorkflowStates(entity, false);
            Assert.AreEqual(4, states.Length);
            states = lm.GetWorkflowStates(workflow, false);
            Assert.AreEqual(4, states.Length);
            states = lm.GetWorkflowStates(entity.GetType().FullName, false);
            Assert.AreEqual(4, states.Length);
            TransitionInfo[] transitions = lm.GetAvailableTransitions(workflow);
            Assert.AreEqual(3, transitions.Length);
            lm.DoStep(workflow, "ToInitial2");
            transitions = lm.GetAvailableTransitions(workflow);
            Assert.AreEqual(2, transitions.Length);
            lm.DoStep(workflow, "ToFinal1");
            transitions = lm.GetAvailableTransitions(workflow);
            Assert.AreEqual(0, transitions.Length);
        }

        [Test]
        public void Performers()
        {
            Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new GenericIdentity("user"), new string[] { "Role1" });
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\PerformersTest.config"));
            TestEntity entity = new TestEntity();
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null);
            TransitionInfo[] transitions = lm.GetAvailableTransitions(workflow);
            Assert.AreEqual(1, transitions.Length);
            Assert.AreEqual("T1", transitions[0].Id);
            Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new GenericIdentity("user"), new string[] { "Role1", "Role2" });
            transitions = lm.GetAvailableTransitions(workflow);
            Assert.AreEqual(2, transitions.Length);
        }

        [Test]
        public void NotAuthorizedForTransition()
        {
            Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new GenericIdentity("user"), new string[] { "Role1" });
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\PerformersTest.config"));
            TestEntity entity = new TestEntity();
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null);
            string state = workflow.CurrentState;
            lm.DoStep(workflow, "T2");
            Assert.AreEqual(state, workflow.CurrentState);
            lm.DoStep(workflow, "T1");
            Assert.AreNotEqual(state, workflow.CurrentState);

        }

        [Test]
        public void NotificationWithoutHandler()
        {
            List<NotifyMessage> list = new List<NotifyMessage>();
            TestEntity entity = new TestEntity();
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\NotificationTest.config"));
            lm.SendMessage += delegate(NotifyMessage m) { list.Add(m); };
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null);
            lm.DoStep(workflow, "ToFinal1");

            Assert.AreEqual(2, list.Count);
            NotifyMessage message = list[0];
            Assert.AreEqual("address1@mail.ru; ", message.To);
            Assert.AreEqual("address2@mail.ru; ", message.Cc);
            Assert.AreEqual("<subject>Notify1</subject>", message.Subject);
            Assert.AreEqual("<body>Notify1</body>", message.Body);

            message = list[1];
            Assert.AreEqual("address2@mail.ru; address1@mail.ru; ", message.To);
            Assert.AreEqual("address3@mail.ru; ", message.Cc);
            Assert.AreEqual("Notify2 subject", message.Subject);
            Assert.AreEqual("Notify2 body", message.Body);
        }

        [Test]
        public void NotificationWithHandler()
        {
            List<NotifyMessage> list = new List<NotifyMessage>();
            TestEntity entity = new TestEntity();
            TestContext context = new TestContext();
            ListmaManager lm = new ListmaManager(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\NotificationTest.config"));
            lm.SendMessage += delegate(NotifyMessage m) { list.Add(m); };
            IWorkflowAdapter<TestEntity> workflow = lm.StartWorkflow(entity, (object)null);
            lm.DoStep(workflow, "ToFinal2", context);

            Assert.AreEqual(1, list.Count);
            NotifyMessage message = list[0];
            Assert.AreEqual("Role1@mail.com; ", message.To);
            Assert.AreEqual("Role2@mail.com; ", message.Cc);
            Assert.AreEqual("Notify subject handled Test", message.Subject);
            Assert.AreEqual("Notify2 body handled Test", message.Body);
        }

        [Test]
        public void ConfigureInCode()
        {
            EntityWorkflow orderWorkflow = new EntityWorkflow(typeof(Order).FullName, "OrderWorkflow1", "State");
            orderWorkflow.RegisterWorkflowFactoryType(typeof(OrderWorkflowFactory));
            ConfigProvider config = new ConfigProvider();
            config.SetStatechartDir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData"));
            config.RegisterEntityWorkflow(orderWorkflow);
            ListmaManager lm = new ListmaManager(config);
            IWorkflowAdapter<Order> adapter = lm.StartWorkflow(Order.GetOrder(), new TestContext());
            Assert.IsInstanceOfType(typeof(Order), adapter.Entity);
        }

        [Test]
        public void BuildStatechart()
        {
            IConfigProvider config = new ConfigProvider();
            EntityWorkflow orderWorkflow = new EntityWorkflow(typeof(Order).FullName, "OrderWorkflow1", "State");
            config.BuildStatechart(orderWorkflow)
                .WithState("New", "New", true)
                    .WithTransition("Activate", "Activate")
                        .ToState("Active")
                        .PerformedBy("Owner")
                        .PerformedBy("Manager")
                        .WithNotification("OnActivation")
                            .ToAddress("user@mail.com")
                            .ToRole("Reviewer")
                            .CcRole("Administrator")
                            .CcAddress("user2@mail.com")
                        .Ret()
                    .Ret()
                    .WithTransition("Close", "Close")
                        .ToState("Closed")
                    .Ret()
                    .DefinePermissionsFor("Number")
                        .ForRole("Owner", UIPermissionLevel.Write)
                        .ForRole("*", UIPermissionLevel.Read)
                    .Ret()
                    .DefinePermissionsFor("Address")
                        .ForRole("Owner", UIPermissionLevel.Write)
                        .ForRole("*", UIPermissionLevel.Read)
                    .Ret()
                .Ret()
                .WithState("Active", "Active", false).Ret()
                .WithState("Closed", "Closed", false).Ret()
                .WithNotifyTemplate("OnActivation", "Order {0} has been activated.", "Order {0} has been activated.");
            Statechart sc = config.GetStatechart(orderWorkflow.StatechartId);

            Assert.IsNotNull(sc);
            Assert.AreEqual(3, sc.States.Length);
            Assert.AreEqual("Active", sc.States[1].Id);
            Assert.AreEqual(2, sc.States[0].Transitions.Length);
            Assert.AreEqual(2, sc.States[0].Transitions[0].Performers.Length);
            Assert.AreEqual(2, sc.States[0].Transitions[0].Notifications[0].To.Length);
            Assert.AreEqual(2, sc.States[0].Transitions[0].Notifications[0].Cc.Length);
            Assert.AreEqual(2, sc.States[0].UIPermissions.Length);
            Assert.AreEqual(2, sc.States[0].UIPermissions[0].Permissions.Length);
            Assert.AreEqual(1, sc.NotifyTemplates.Length);
        }

        [Test]
        public void RuntimeHandlerTest()
        {
            List<string> Log = new List<string>();
            IConfigProvider config = new ConfigProvider();
            EntityWorkflow workflow = new EntityWorkflow(typeof(Order).FullName, "OrderWorkflow1", "State");
            config.RegisterEntityWorkflow(workflow);
            config.BuildStatechart(workflow)
                .WithState("Draft", "Draft", true)
                    .ExitHandledBy<Order, TestContext>((o, c) => Log.Add(o.State.ToString() + " exit"))
                    .WithTransition("Do", "Do").ToState("Archive")
                        .HandledBy<Order, TestContext>(o => { Log.Add("Prevalidate order " + o.Number); },
                        (o, c) =>
                        {
                            Console.WriteLine("Context is '{0}',  Order state {1}", c.Text, o.State);
                            c.Text += " has been done";
                        },
                        (o, s) => { Log.Add("State is changed to " + s); return true; })
                        .WithNotification("Notification1")
                            .HandledBy<Order, TestContext>((role, o, ctx) =>
                            {
                                return new string[] { role + "@mail.com" };
                            },
                            (message, tempalte, o, ctx) =>
                            {
                                message.Subject = tempalte.Subject;
                                message.Body = tempalte.Body;
                            })
                            .ToRole("Role1")
                            .CcRole("Role2")
                        .Ret()
                    .Ret()
                .Ret()
                .WithState("Archive", "Archive", false)
                    .EnterHandledBy<Order, TestContext>((o, c) => Log.Add(o.State.ToString() + " enter"))
                .Ret()
                .WithNotifyTemplate("Notification1", "Subject", "Body");
            
            ListmaManager manager = new ListmaManager(config);
            NotifyMessage testMessage = null;
            manager.SendMessage += m =>
            {
                testMessage = m;
            };
            Order order = Order.GetOrder();
            TestContext context = new TestContext();
            IWorkflowAdapter<Order> orderWorkflow = manager.StartWorkflow(order, context);
            TransitionInfo[] transitions = manager.GetAvailableTransitions(orderWorkflow);

            Assert.AreEqual(1, transitions.Length);

            manager.DoStep(orderWorkflow, transitions[0].Id, context);

            Assert.AreEqual("Archive", orderWorkflow.CurrentState);
            Assert.AreEqual("Test has been done", context.Text, "Result of transition handler work");
            Assert.AreEqual("Prevalidate order 1/1", Log[0]);
            Assert.AreEqual("Draft exit", Log[1]);
            Assert.AreEqual("State is changed to Archive", Log[2]);
            Assert.AreEqual("Archive enter", Log[3]);

            Assert.AreEqual("Role1@mail.com; ", testMessage.To);
            Assert.AreEqual("Role2@mail.com; ", testMessage.Cc);
            Assert.AreEqual("Subject", testMessage.Subject);
            Assert.AreEqual("Body", testMessage.Body);
        }

        void manager_SendMessage(NotifyMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
