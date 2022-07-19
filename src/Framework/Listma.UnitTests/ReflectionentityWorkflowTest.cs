using System;
using NUnit.Framework;

using Listma;

namespace Listma.UnitTests
{
    [TestFixture]
    public class ReflectionentityWorkflowTest
    {
        public const string STATE_FIELD_NAME = "_State";
        public const string StatechartID_FIELD_NAME = "_StatechartId";
        public const string StatechartID_PROP_NAME = "StatechartId";
        public const string StatechartID = "NativeValue";
        public const string StatechartID2 = "NewStatechart";

        [Test]
        public void ReflectionStatesTest()
        {
            VariousState entity = new VariousState();
            entity.StateIntField = 1;
            entity.StateStringField = "One";
            entity.StateEnumField = EnumState.One;

            ReflectionEntityWorkflow<VariousState> adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateIntField");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("1", adapter.CurrentState);
            adapter.SetCurrentState("2");
            Assert.AreEqual("2", adapter.CurrentState);
            Assert.AreEqual(2, entity.StateIntField);

            adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateIntProp");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("2", adapter.CurrentState);
            adapter.SetCurrentState("3");
            Assert.AreEqual("3", adapter.CurrentState);
            Assert.AreEqual(3, entity.StateIntProp);

            adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateStringField");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("One", adapter.CurrentState);
            adapter.SetCurrentState("Two");
            Assert.AreEqual("Two", adapter.CurrentState);
            Assert.AreEqual("Two", entity.StateStringField);

            adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateStringProp");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("Two", adapter.CurrentState);
            adapter.SetCurrentState("One");
            Assert.AreEqual("One", adapter.CurrentState);
            Assert.AreEqual("One", entity.StateStringField);

            adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateEnumField");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("One", adapter.CurrentState);
            adapter.SetCurrentState("Two");
            Assert.AreEqual("Two", adapter.CurrentState);
            Assert.AreEqual(EnumState.Two, entity.StateEnumField);

            adapter = new ReflectionEntityWorkflow<VariousState>(entity, "StateEnumProp");
            Assert.AreEqual(entity, adapter.Entity);
            Assert.AreEqual("Two", adapter.CurrentState);
            adapter.SetCurrentState("One");
            Assert.AreEqual("One", adapter.CurrentState);
            Assert.AreEqual(EnumState.One, entity.StateEnumField);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void WrongArgument1Test()
        {
            ReflectionEntityWorkflow<VariousState> target = new ReflectionEntityWorkflow<VariousState>(null, null);
            Assert.IsNotNull(target);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void WrongArgument2Test()
        {
            ReflectionEntityWorkflow<VariousState> target = new ReflectionEntityWorkflow<VariousState>(new VariousState(), null);
            Assert.IsNotNull(target);
        }

        [Test, ExpectedException(typeof(WorkflowException))]
        public void WrongArgument3Test()
        {
            ReflectionEntityWorkflow<VariousState> target = new ReflectionEntityWorkflow<VariousState>(new VariousState(), "NotexsistentField");
            Assert.IsNotNull(target);
        }

        [Test, ExpectedException(typeof(WorkflowException))]
        public void WrongArgument4Test()
        {
            ReflectionEntityWorkflow<VariousState> target = new ReflectionEntityWorkflow<VariousState>(new VariousState(), "Dummy");
            Assert.IsNotNull(target);
        }

        [Test]
        public void AdapterFieldsTest()
        {
            FPEntity entity = new FPEntity();
            ReflectionEntityWorkflow<FPEntity> adapter = new ReflectionEntityWorkflow<FPEntity>(
                entity, STATE_FIELD_NAME, StatechartID_FIELD_NAME);
            Assert.AreEqual("1", adapter.CurrentState);
            Assert.AreEqual(StatechartID, adapter.StatechartId);
        }

        [Test]
        public void AdapterPropertiesTest()
        {
            FPEntity entity = new FPEntity();
            ReflectionEntityWorkflow<FPEntity> adapter = new ReflectionEntityWorkflow<FPEntity>(
                entity, STATE_FIELD_NAME, StatechartID_PROP_NAME);
            Assert.AreEqual("1", adapter.CurrentState);
            Assert.AreEqual(StatechartID, adapter.StatechartId);
            adapter = new ReflectionEntityWorkflow<FPEntity>(
                entity, STATE_FIELD_NAME, StatechartID_PROP_NAME);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WrongParamsTest()
        {
            ReflectionEntityWorkflow<FPEntity> adapter = new ReflectionEntityWorkflow<FPEntity>(
                new FPEntity(),STATE_FIELD_NAME, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WrongParamsTest2()
        {
            ReflectionEntityWorkflow<FPEntity> adapter = new ReflectionEntityWorkflow<FPEntity>(
                new FPEntity(), null, null);
        }
    }
    internal class VariousState
    {
        public int StateIntField;
        public int StateIntProp
        {
            get { return StateIntField; }
            set { StateIntField = value; }
        }

        public String StateStringField;
        public String StateStringProp
        {
            get { return StateStringField; }
            set { StateStringField = value; }
        }

        public EnumState StateEnumField;
        public EnumState StateEnumProp
        {
            get { return StateEnumField; }
            set { StateEnumField = value; }
        }

        public void Dummy() { throw new NotImplementedException(); }

    }

    internal enum EnumState { One, Two }

    internal class FPEntity
    {
        public int _State = 1;
        public string _StatechartId = ReflectionentityWorkflowTest.StatechartID;
        public string StatechartId { get { return _StatechartId; } set { _StatechartId = value; } }
    }

      
}
