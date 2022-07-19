using System;
using NUnit.Framework;

using Listma;
using Listma.Configuration;


namespace Listma.UnitTests
{
    [TestFixture]
    public class EntityFactoryTest
    {
        [Test]
        public void FactoryTest()
        {
            ReflectionFactory<FPEntity> f = new ReflectionFactory<FPEntity>();
            FPEntity entity = new FPEntity();
            IWorkflowAdapter<FPEntity> w = EntityFactoryService.GetWorkflow<FPEntity>(entity, f, GetEntityWorkflow(false));
            Assert.AreEqual("ValueFromConfig", w.StatechartId);
            w = EntityFactoryService.GetWorkflow<FPEntity>(entity, f, GetEntityWorkflow(true));
            Assert.AreEqual(ReflectionentityWorkflowTest.StatechartID, w.StatechartId);
            Assert.AreEqual("1", w.CurrentState);
        }

        private static EntityWorkflow GetEntityWorkflow(bool fullmap)
        {
            EntityWorkflow w = new EntityWorkflow(typeof(FPEntity).FullName, "ValueFromConfig", "_State");
            w.SetInitialState("2");
            if(fullmap)
                w.SetStatechartMap("StatechartId");
            return w;
        }
    }
}
