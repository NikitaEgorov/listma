using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Listma.Utils
{

	/// <summary>
	/// Helper class for XML serialization
	/// </summary>
	public static class XmlUtility
	{
		/// <summary>
		/// Serilizes object to XML string
		/// </summary>
		public static string Obj2XmlStr(object obj, string nameSpace)
		{
			if (obj == null) return string.Empty;
			XmlSerializer sr = SerializerCache.GetSerializer(obj.GetType()); 
			
			StringBuilder sb = new StringBuilder();
			StringWriter w = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
			
			sr.Serialize(
				w, 
				obj, 
				new XmlSerializerNamespaces(
				new XmlQualifiedName[] 
					{
						new XmlQualifiedName("", nameSpace)
					}
				));
			return sb.ToString();
		}

		/// <summary>
        /// Serilizes object to XML string
		/// </summary>
		public static string Obj2XmlStr(object obj)
		{
			if (obj == null) return string.Empty;
			XmlSerializer sr = SerializerCache.GetSerializer(obj.GetType()); 
			
			StringBuilder sb = new StringBuilder();
			StringWriter w = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
			
			sr.Serialize(
				w, 
				obj, 
				new XmlSerializerNamespaces( new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) } ) );

			return sb.ToString();
		}

		/// <summary>
        /// Deserilizes XML string to object
		/// </summary>
		/// <param name="xml">xml string</param>
		/// <returns></returns>
		public static T XmlStr2Obj<T>(string xml) 
		{
			if (xml == null) return default(T);
			if (xml == string.Empty) return (T)Activator.CreateInstance(typeof(T));

			StringReader reader = new StringReader(xml);
			XmlSerializer sr = SerializerCache.GetSerializer(typeof(T));
			return (T)sr.Deserialize(reader);
		}
		
		/// <summary>
        ///Deserilizes XML string to XmlElement
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
        public static IXPathNavigable XmlStr2XmlDom(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc.DocumentElement;
		}

		/// <summary>
        /// Deserilizes XML string to XmlElement
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="NameSpace"></param>
		/// <returns></returns>
		public static  IXPathNavigable Obj2XmlDom(object obj, string NameSpace)
		{
			return XmlStr2XmlDom(Obj2XmlStr(obj, NameSpace));
		}

		
		
	}

	/// <summary>
	/// Cashe for used XmlSerilizer instances
	/// </summary>
	internal class SerializerCache
	{
		private static Hashtable hash = new Hashtable();
		public static XmlSerializer GetSerializer(Type type)
		{
			XmlSerializer res = null;
			lock(hash)
			{
				res = hash[type.FullName] as XmlSerializer;
				if(res == null) 
				{
					res = new XmlSerializer(type);
					hash[type.FullName] = res;
				}
			}
			return res;
		}
	}

}
