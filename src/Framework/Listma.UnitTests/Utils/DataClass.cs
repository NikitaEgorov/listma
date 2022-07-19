using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace Listma.UnitTests
{
	/// <summary>
	/// Summary description for DataClass.
	/// </summary>
	[XmlRoot("Data", Namespace=DataClass.XmlNamespace )]
    [XmlInclude(typeof(ChildClass))]
    public class DataClass
	{
		public const string XmlNamespace = "urn:MyDataClass"; 
		public DataClass(){}
        
        [XmlElement]
        public DataClass Child;

		[XmlAttribute]
		public string ID = Guid.NewGuid().ToString();
		[XmlAttribute]
		public string Name = "Just Name";
		[XmlElement("Reserved")]
		public Decimal Count = 10;
		[XmlIgnore]
		public DateTime Date = DateTime.Now;
        [XmlElement("Line")]
        public string[] Lines = new string[] { "Line one", "Line two", "Line three" };

        //public ArrayList List = new ArrayList();
        
        public List<DataClass> List = new List<DataClass>();

        [XmlElement]
		public string Secret = string.Empty;

        [XmlIgnore]
        public string MySecret
        {
            get
            {
                if (Secret != string.Empty && Secret != null)
                    return UnicodeEncoding.Unicode.GetString(Convert.FromBase64String(Secret));
                else return Secret;
            }
            set
            {
                if (value != null && value != String.Empty)
                    Secret = Convert.ToBase64String(UnicodeEncoding.Unicode.GetBytes(value));
                else Secret = value;
            }
        }

	}

    [XmlType(Namespace=DataClass.XmlNamespace)]
    public class ChildClass : DataClass
    {
        public string ParentName;
    }

    [XmlType(Namespace = DataClass.XmlNamespace)]
    public class GrandChildClass : DataClass
    {
    }
}
