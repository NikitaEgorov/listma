using System;
using Listma;
using NUnit.Framework;
using Listma.Utils;

namespace Listma.UnitTests.Metadata
{
    [TestFixture]
    public class StatechartTest
    {
        Statechart GetStatechart()
        {
            return new Statechart()
            {
                Id = "TestStatechart",
                States = new State[]{
                new State(){
                Id = "One",
                Title="First State",
                OnEnterHandler = "OneEnterHandler",
                OnExitHandler = "OneExitHandler",
                Description = new Description(){Source = "src", Text="description"},
                Transitions = new Transition[]{
                    new Transition(){
                    Id = "T1",
                    Title = "To Second State",
                    TargetState = "Two",
                    Handler = "T1Handler",
                    Performers = new Performer[]{
                        new Performer(){Role="Role1"},
                        new Performer(){Role="Role2"}
                        }
                    },
                    new Transition(){
                    Id = "T2",
                    Title = "Roundtrip",
                    TargetState = "One",
                    Handler = "T2Handler"}
                }},
                new State(){
                Id = "Two",
                Title="Second State",
                OnEnterHandler = "OneEnterHandler",
                OnExitHandler = "OneExitHandler",
                Description = new Description(){Source = "src", Text="description"},
                Transitions = new Transition[]{
                    new Transition(){
                    Id = "T3",
                    Title = "To First State",
                    TargetState = "One",
                    Handler = "T3Handler"},
                    new Transition(){
                    Id = "T4",
                    Title = "Roundtrip",
                    TargetState = "Two",
                    Handler = "T2Handler"}
                }}}
            };
        }
        [Test]
        public void StatechartSerializationTest()
        {
            Statechart s1 = GetStatechart();
            string xml = XmlUtility.Obj2XmlStr(s1);
            Statechart s2 = XmlUtility.XmlStr2Obj<Statechart>(xml);
            Assert.AreEqual(s1.Id, s2.Id);
            Assert.AreEqual(s1.States.Length, s2.States.Length);
            Assert.AreEqual(s1.States[0].Id, s2.States[0].Id);
            Assert.AreEqual(s1.States[0].Title, s2.States[0].Title);
            Assert.AreEqual(s1.States[0].Initial, s2.States[0].Initial);
            Assert.AreEqual(s1.States[0].Description.Source, s2.States[0].Description.Source);
            Assert.AreEqual(s1.States[0].Description.Text, s2.States[0].Description.Text);
            Assert.AreEqual(s1.States[0].OnEnterHandler, s2.States[0].OnEnterHandler);
            Assert.AreEqual(s1.States[0].OnExitHandler, s2.States[0].OnExitHandler);
            Assert.AreEqual(s1.States[0].Transitions.Length, s2.States[0].Transitions.Length);
            Assert.AreEqual(s1.States[0].Transitions[0].Id, s2.States[0].Transitions[0].Id);
            Assert.AreEqual(s1.States[0].Transitions[0].Title, s2.States[0].Transitions[0].Title);
            Assert.AreEqual(s1.States[0].Transitions[0].TargetState, s2.States[0].Transitions[0].TargetState);
            Assert.AreEqual(s1.States[0].Transitions[0].Handler, s2.States[0].Transitions[0].Handler);
            Assert.AreEqual(s1.States[0].Transitions[0].Performers.Length, s2.States[0].Transitions[0].Performers.Length);
            Assert.AreEqual(s1.States[0].Transitions[0].Performers[0].Role, s2.States[0].Transitions[0].Performers[0].Role);
        }

    }
}
