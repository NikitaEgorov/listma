using Listma.Configuration;
using NUnit.Framework;
using System.IO;
using System;
using Listma.Test;

namespace Listma.UnitTests
{
    [TestFixture]
    public class ListmaConfigurationTest
    {
        [Test]
        public void ListmaConfigurationLoadTest()
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, "IntegrationTestsData\\Listma.config");
            ListmaConfigurationData target = ListmaConfigurationData.Load(fileName);
            Assert.IsNotNull(target);
            Assert.AreEqual("IntegrationTestsData", target.StatechartDir);
            Assert.AreEqual(3, target.Entities.Length);
            Assert.AreEqual(typeof(Order).FullName, target.Entities[0].EntityType);
            Assert.AreEqual("Listma.ReflectionFactory`1", target.Entities[1].WorkflowFactoryClass);
            Assert.AreEqual("Listma.RoleProvider`2", target.Entities[0].RoleProviderClass);
            Assert.AreEqual("OrderApprovalWorkflow1", target.Entities[1].StatechartId);
            Assert.AreEqual("*", target.Entities[1].InitialState);
            Assert.AreEqual("ApproveState", target.Entities[1].StateMap);
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void ListmaConfigurationExceptionTest()
        {
            ListmaConfigurationData target = ListmaConfigurationData.Load(" ");
            Assert.IsNotNull(target);
        }

 
    }
}
