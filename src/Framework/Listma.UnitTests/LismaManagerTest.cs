using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Listma;
using Listma.Test;

namespace Listma.UnitTests
{

    [TestFixture]
    public class ListmaManagerTest
    {
        string configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestsData\\listma.config");

        #region Guard Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEntityWorkflowGuardTest1()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w1 = lm.GetWorkflow<Order>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEntityWorkflowGuardTest2()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w1 = lm.GetWorkflow<Order>(new Order(), "");
        }

        [Test]
        public void GetEntityWorkflowGuardTest3()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w1 = lm.GetWorkflow<Order>(Order.GetOrder());
            Assert.IsNotNull(w1);
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void GetEntityWorkflowGuardTest4()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<DateTime> w1 = lm.GetWorkflow<DateTime>(DateTime.Now);
         }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoStepGuardTest1()
        {
            ListmaManager lm = new ListmaManager(configFile);
            lm.DoStep<Order>(null, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoStepGuardTest2()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w1 = lm.GetWorkflow<Order>(Order.GetOrder());
            lm.DoStep<Order>(w1, "");
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void DoStepGuardTest3()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w1 = lm.GetWorkflow<Order>(Order.GetOrder());
            lm.DoStep<Order>(w1, "UnexistentTransition");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAvailableTransitionsTest()
        {
            ListmaManager lm = new ListmaManager(configFile);
            lm.GetAvailableTransitions<Order>(null);
        }

        [Test]
        public void ConstructorGuardTest1()
        {
            IConfigProvider p = new ConfigProvider(Listma.Configuration.ListmaConfigurationData.Load(configFile));
            ListmaManager lm = new ListmaManager(p);
            Assert.IsNotNull(lm);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorGuardTest2()
        {
            IConfigProvider p = null;
            ListmaManager lm = new ListmaManager(p);
            Assert.IsNotNull(lm);
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void ConstructorGuardTest3()
        {
            string n = null;
            ListmaManager lm = new ListmaManager(n);
            Assert.IsNotNull(lm);
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void StartWorkflowGuardTest1()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w = lm.StartWorkflow(Order.GetOrder(), (object)null, "UnExistentState");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartWorkflowGuardTest2()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w = lm.StartWorkflow((Order)null, (object)null, "UnExistentState");
        }

        [Test]
        public void StartWorkflowGuardTest3()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w = lm.StartWorkflow(Order.GetOrder(), new TestContext(), typeof(Order).FullName, new StateInfo("Draft"));
            Assert.IsInstanceOfType(typeof(Order), w.Entity);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartWorkflowGuardTest4()
        {
            ListmaManager lm = new ListmaManager(configFile);
            IWorkflowAdapter<Order> w = lm.StartWorkflow((Order)null, (TestContext)null, typeof(Order).FullName, new StateInfo("Draft"));
            Assert.IsInstanceOfType(typeof(Order), w.Entity);
        }


        #endregion
    }
}
