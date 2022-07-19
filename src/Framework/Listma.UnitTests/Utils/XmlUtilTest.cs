using System;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using NUnit.Framework;
using Listma.Utils;
using System.Xml;
using System.Diagnostics;

namespace Listma.UnitTests.Utils
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
    [TestFixture]
	public class XmlUtilityTest
	{
		[Test]
		public void SerializeTest()
		{
			DataClass obj = new DataClass();
            string xml = XmlUtility.Obj2XmlStr(obj, DataClass.XmlNamespace);
			Console.WriteLine(xml);
            DataClass clone = XmlUtility.XmlStr2Obj<DataClass>(xml);

            Assert.IsNotNull(clone);
            Assert.AreEqual(clone.ID, obj.ID);
            Assert.AreEqual(clone.Name, obj.Name); // and so on
		}

        [Test]
        public void DeSerializeNullTest()
        {
            DataClass obj = XmlUtility.XmlStr2Obj<DataClass>(string.Empty);
            Assert.IsNotNull(obj);
            obj = XmlUtility.XmlStr2Obj<DataClass>(null);
            Assert.IsNull(obj);

            DateTime date = XmlUtility.XmlStr2Obj<DateTime>(string.Empty);
            Assert.IsNotNull(date);
            date = XmlUtility.XmlStr2Obj<DateTime>(null);
            Assert.IsNotNull(date);
        }

        [Test]
        public void SerializeNullTest()
        {
            string xml = XmlUtility.Obj2XmlStr(null);
            Assert.AreEqual(string.Empty, xml);
            xml = XmlUtility.Obj2XmlStr(null, null);
            Assert.AreEqual(string.Empty, xml);
        }
        [Test]
        public void InheritanceTest()
        {
            DataClass obj = new DataClass();
            obj.Child = new GrandChildClass();
            XmlAttributes attrs = new XmlAttributes();
            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "GrandChildClass";
            attr.Type = typeof(GrandChildClass);
            attrs.XmlElements.Add(attr);
            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
            attrOverrides.Add(typeof(DataClass), "Child", attrs);

            XmlSerializer sr = new XmlSerializer(typeof(DataClass), attrOverrides);
            StringBuilder sb = new StringBuilder();
            StringWriter w = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);

            sr.Serialize(w, obj);

            string xml = sb.ToString();
            
            Console.WriteLine(xml);
            DataClass clone = XmlUtility.XmlStr2Obj<DataClass>(xml);

            Assert.IsNotNull(clone);
            Assert.AreEqual(clone.ID, obj.ID);
            Assert.AreEqual(clone.Name, obj.Name); // and so on
        }

        [Test]
        public void Obj2XmlDomTest()
        {
            DataClass obj = new DataClass();
            XmlElement xml = (XmlElement)XmlUtility.Obj2XmlDom(obj, DataClass.XmlNamespace);
            Assert.IsNotNull(xml);
            Assert.AreEqual("Just Name", xml.Attributes["Name"].Value);

        }

        [Test]
        public void CompareTest()
        {
            DateTime mark1 = DateTime.Now;
            for(int i=0;i<1000;i++) SerializeDirect();
            TimeSpan span = DateTime.Now-mark1;
            Debug.WriteLine(String.Format("Direct serialization time {0}", span));

            mark1 = DateTime.Now;
            for (int i = 0; i < 1000; i++) SerializeCache();
            span = DateTime.Now-mark1;
            Debug.WriteLine(String.Format("Cache serialization time {0}", span));


        }

        void SerializeDirect()
        {
            DataClass obj = new DataClass();
            XmlSerializer sr = new XmlSerializer(typeof(DataClass));
            StringBuilder sb = new StringBuilder();
            StringWriter w = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
            sr.Serialize(
                w,
                obj,
                new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));
        }
        void SerializeCache()
        {
            DataClass obj = new DataClass();
            XmlUtility.Obj2XmlStr(obj);
        }
	}
}
