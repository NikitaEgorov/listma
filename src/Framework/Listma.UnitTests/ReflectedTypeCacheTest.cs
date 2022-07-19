using System;
using NUnit.Framework;

using Listma;
using Listma.Test;

namespace Listma.UnitTests
{
    [TestFixture]
    public class ReflectedTypeCacheTest
    {
        [Test]
        public void CacheTest()
        {
            string typeName = "Listma.Test.ContextHandler, Listma.Test.Handlers";
            IHandler<Order, TestContext> handler1 = ReflectedTypeCache.GetInstance<IHandler<Order, TestContext>>(typeName, null);
            IHandler<Order, TestContext> handler2 = ReflectedTypeCache.GetInstance<IHandler<Order, TestContext>>(typeName, null);
            Assert.AreEqual("Listma.Test.ContextHandler", handler1.GetType().FullName);
            Assert.AreNotSame(handler1, handler2);
        }

        [Test]
        public void ConstructGenericType()
        {
            string typeName = "Listma.RoleProvider`2, Listma";
            IRoleProvider<Order, TestContext> p1 = ReflectedTypeCache.GetInstance<IRoleProvider<Order, TestContext>>(typeName,
                new Type[] { typeof(Order), typeof(TestContext) });
            Assert.IsNotNull(p1);
            IRoleProvider<Order, object> p2 = ReflectedTypeCache.GetInstance<IRoleProvider<Order, object>>(typeName,
                new Type[] { typeof(Order), typeof(object) });
            Assert.IsNotNull(p1);
        }

        [Test]
        [ExpectedException(typeof(WorkflowException))]
        public void TypeNameGuardTest()
        {
            IHandler<Order, TestContext> handler = ReflectedTypeCache.GetInstance<IHandler<Order, TestContext>>("UnexistentTypeName, UnexistentAssembly", null);
            Assert.IsNotNull(handler);
        }
    }
}
